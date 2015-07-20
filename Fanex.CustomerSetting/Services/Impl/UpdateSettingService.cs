namespace Fanex.CustomerSetting.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Fanex.Common.Helper;
    using Fanex.CustomerSetting.Dtos.Params;
    using Fanex.CustomerSetting.Dtos.Results;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Exceptions;
    using Fanex.CustomerSetting.Handlers;
    using Fanex.CustomerSetting.Models;
    using Fanex.Logging;

    /// <summary>
    /// Service class for get single customer setting/ update single/multiple setting
    /// Setting includes list of setting types (pt, bet limit, commission ....)
    /// Customer can be super, master, agent, member
    /// So we call {Customer}{SettingType} => means SuperPositionTaking, MasterCommission ...
    /// </summary>
    public class UpdateSettingService : IUpdateSettingService
    {
        private readonly ISettingContext _settingContext;
        private readonly IValidationService _validationService;
        private readonly ILogger _logger;
        private readonly ISettingTypeServiceFactory _settingTypeServiceFactory;

        public UpdateSettingService(
            ISettingContext settingContext,
            IValidationService validationService = null,
            ILogger logger = null,
            ISettingTypeServiceFactory settingTypeServiceFactory = null)
        {
            Check.Argument.IsNotNull(settingContext, "settingContext");
            Check.Argument.IsNotNull(settingContext.Actor, "settingContext.Actor");

            _settingContext = settingContext;
            _validationService = validationService ?? new ValidationService(_settingContext);
            _logger = logger ?? Logger.Log;
            _settingTypeServiceFactory = settingTypeServiceFactory ?? new SettingTypeServiceFactory();
        }

        /// <summary>
        /// Updates the customer setting, includes list of setting types.
        /// </summary>
        /// <param name="updateParam">The update parameters.</param>
        /// <returns>Result after update customer setting.</returns>
        public IResult UpdateCustomerSetting(UpdateSettingParam updateParam)
        {
            Check.Argument.IsNotNull(updateParam, "updateParam");
            Check.Argument.IsNotNegativeOrZero(updateParam.Setting.SettingTypes.Count(), "updateParam.Setting.SettingTypes.Count");

            var validationResult = Validate(updateParam);

            if (!validationResult.IsSuccessful)
            {
                return validationResult;
            }

            return new UpdateSettingResult(UpdateCustomerSettingTypes(updateParam));
        }

        private IResult Validate(UpdateSettingParam updateParam)
        {
            return _validationService.ValidateUpdateSettingAction(updateParam);
        }

        private IEnumerable<SettingResult> UpdateCustomerSettingTypes(UpdateSettingParam updateParam)
        {
            var settingTypes = updateParam.Setting.SettingTypes;
            var customerId = updateParam.Setting.CustomerId;
            var customerLevel = updateParam.Setting.CustomerLevel;
            var uplineLevel = updateParam.UplineLevel;
            var productId = updateParam.Setting.ProductId;
            var settingTypeResults = new List<SettingResult>();
            SettingResult updateSettingTypeResult = null;

            foreach (var settingType in settingTypes)
            {
                try
                {
                    updateSettingTypeResult = UpdateCustomerSettingType(customerId, customerLevel, settingType, productId, uplineLevel);
                }
                catch (Exception ex)
                {
                    updateSettingTypeResult = new SettingResult(
                        Result.SystemError.Code,
                        Result.SystemError.Message,
                        0,
                        customerId,
                        settingType.Id,
                        settingType.Name);

                    LogUpdateCustomerSettingTypeError(customerId, customerLevel, uplineLevel, productId, settingType, ex);
                }

                settingTypeResults.Add(updateSettingTypeResult);
            }

            return settingTypeResults;
        }

        private SettingResult UpdateCustomerSettingType(int customerId, Level customerLevel, SettingType settingType, byte productId, Level uplineLevel)
        {
            var settingTypeService = _settingTypeServiceFactory.GetInstance(settingType);

            return settingTypeService.UpdateCustomerSettingType(_settingContext, customerId, customerLevel, uplineLevel, productId, settingType);
        }

        private void LogUpdateCustomerSettingTypeError(
            int customerId,
            Level customerLevel,
            Level uplineLevel,
            byte productId,
            SettingType customerSettingType,
            Exception ex)
        {
            var customerSettingException = new CustomerSettingException(ex.Message, ex);
            customerSettingException.SetSettingContextInfo("Update Setting", _settingContext);
            customerSettingException.SetCustomerInfo(customerId, customerLevel, uplineLevel);
            customerSettingException.SetProductInfo(productId, customerSettingType);

            _logger.Error(customerSettingException.Message, customerSettingException);
        }

        public IResult UpdateCustomerSetting(UpdateMultipleSettingParam updateMultipleParam)
        {
            Check.Argument.IsNotNull(updateMultipleParam, "updateMultipleParam");
            Check.Argument.IsNotNegativeOrZero(updateMultipleParam.Setting.SettingTypes.Count(), "updateMultipleParam.Setting.SettingTypes.Count");

            var validationResult = Validate(updateMultipleParam);

            if (!validationResult.IsSuccessful)
            {
                return validationResult;
            }

            var results = UpdateSettingTypesForMultipleCustomers(updateMultipleParam);

            return MapUpdateSettingResultWithCustomerName(results, updateMultipleParam.CustomerIds, updateMultipleParam.CustomerNames);
        }

        private IResult Validate(UpdateMultipleSettingParam updateMultipleParam)
        {
            return _validationService.ValidateUpdateMultipleSettingAction(updateMultipleParam);
        }

        private List<SettingResult> UpdateSettingTypesForMultipleCustomers(UpdateMultipleSettingParam updateMultipleParam)
        {
            var customerLevel = updateMultipleParam.Setting.CustomerLevel;
            var uplineLevel = updateMultipleParam.UplineLevel;
            var customerIds = updateMultipleParam.CustomerIds;
            var settingTypes = updateMultipleParam.Setting.SettingTypes;
            var productId = updateMultipleParam.Setting.ProductId;
            var results = new List<SettingResult>();

            foreach (var settingType in settingTypes)
            {
                try
                {
                    var updateResults = UpdateMultipleCustomersSettingType(customerIds, customerLevel, uplineLevel, productId, settingType);

                    results.AddRange(updateResults);
                }
                catch (Exception ex)
                {
                    var errorResult = new SettingResult(
                        Result.SystemError.Code,
                        Result.SystemError.Message,
                        msgType: 0,
                        customerId: 0,
                        settingTypeId: settingType.Id,
                        settingTypeName: settingType.Name);

                    LogUpdateMultipleCustomersSettingTypeError(customerIds, customerLevel, uplineLevel, productId, settingType, ex);

                    results.Add(errorResult);
                }
            }

            return results;
        }

        private IEnumerable<SettingResult> UpdateMultipleCustomersSettingType(IEnumerable<int> customerIds, Level customerLevel, Level uplineLevel, byte productId, SettingType settingType)
        {
            var settingTypeService = _settingTypeServiceFactory.GetInstance(settingType);

            return settingTypeService.UpdateMultipleCustomersSettingType(
                    _settingContext,
                    customerIds,
                    customerLevel,
                    uplineLevel,
                    productId,
                    settingType);
        }

        private UpdateSettingResult MapUpdateSettingResultWithCustomerName(
            IEnumerable<SettingResult> settingResults,
            IEnumerable<int> customerIds,
            IEnumerable<string> customerNames)
        {
            var customerIdNameContainer = GetCustomerIdNameContainer(customerIds, customerNames);

            return MapSettingResultWithCustomerName(customerIdNameContainer, settingResults);
        }

        private UpdateSettingResult MapSettingResultWithCustomerName(
            Dictionary<int, string> customerIdNameContainer,
            IEnumerable<SettingResult> results)
        {
            foreach (SettingResult result in results)
            {
                if (customerIdNameContainer.ContainsKey(result.CustomerId))
                {
                    result.CustomerName = customerIdNameContainer[result.CustomerId];
                }
            }

            return new UpdateSettingResult(results);
        }

        private Dictionary<int, string> GetCustomerIdNameContainer(
            IEnumerable<int> customerIds,
            IEnumerable<string> customerNames)
        {
            var customerIdNameContainer = new Dictionary<int, string>();
            var customerLength = customerIds.Count();

            for (int i = 0; i < customerLength; i++)
            {
                customerIdNameContainer.Add(customerIds.ElementAt(i), customerNames.ElementAt(i));
            }

            return customerIdNameContainer;
        }

        private void LogUpdateMultipleCustomersSettingTypeError(
            IEnumerable<int> customerIds,
            Level customerLevel,
            Level uplineLevel,
            byte productId,
            SettingType customerSettingType,
            Exception ex)
        {
            var customerSettingException = new CustomerSettingException(ex.Message, ex);
            customerSettingException.SetSettingContextInfo("Update Setting", _settingContext);
            customerSettingException.SetCustomerInfo(string.Join(",", customerIds), customerLevel, uplineLevel);
            customerSettingException.SetProductInfo(productId, customerSettingType);

            _logger.Error(customerSettingException.Message, customerSettingException);
        }
    }
}