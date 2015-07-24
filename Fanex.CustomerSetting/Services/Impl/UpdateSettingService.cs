namespace Fanex.CustomerSetting.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Common.Helper;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Exceptions;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Params;
    using Fanex.CustomerSetting.Results;
    using Fanex.Logging;

    /// <summary>
    /// Service class for get single customer setting/ update single/multiple setting
    /// Setting includes list of setting types (pt, bet limit, commission ....)
    /// Customer can be super, master, agent, member
    /// So we call {Customer}{SettingType} => means SuperPositionTaking, MasterCommission ...
    /// </summary>
    public partial class UpdateSettingService : IUpdateSettingService
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
        public IResult Update(UpdateCustomerSettingParam updateParam)
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

        private IResult Validate(UpdateCustomerSettingParam updateParam)
        {
            return _validationService.ValidateUpdateSettingAction(updateParam);
        }

        private IEnumerable<SettingResult> UpdateCustomerSettingTypes(UpdateCustomerSettingParam updateParam)
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
                updateSettingTypeResult = UpdateCustomerSettingType(customerId, customerLevel, settingType, productId, uplineLevel);

                settingTypeResults.Add(updateSettingTypeResult);
            }

            return settingTypeResults;
        }

        private SettingResult UpdateCustomerSettingType(int customerId, Level customerLevel, SettingType settingType, byte productId, Level uplineLevel)
        {
            try
            {
                CheckSettingTypeIsValid(settingType);

                var settingTypeService = _settingTypeServiceFactory.GetInstance(settingType);

                return settingTypeService.UpdateCustomerSettingType(_settingContext, customerId, customerLevel, uplineLevel, productId, settingType);
            }
            catch (CustomerSettingException customerSettingException)
            {
                WriteLog(customerId, customerLevel, uplineLevel, productId, settingType, customerSettingException);

                return new SettingResult(Result.AccessDenied.Code, customerSettingException.OriginalMessage, 0, customerId, settingType.Id, settingType.Name);
            }
            catch (Exception ex)
            {
                WriteLog(customerId, customerLevel, uplineLevel, productId, settingType, new CustomerSettingException(ex.Message, ex));

                return new SettingResult(
                    Result.SystemError,
                    customerId,
                    settingType.Id,
                    settingType.Name);
            }
        }

        private void CheckSettingTypeIsValid(SettingType settingType)
        {
            var result = settingType.Validate();

            if (result.IsNotOk)
            {
                throw new CustomerSettingException(result.Message);
            }
        }

        private void WriteLog(
            int customerId,
            Level customerLevel,
            Level uplineLevel,
            byte productId,
            SettingType customerSettingType,
            CustomerSettingException ex)
        {
            ex.SetSettingContextInfo("Update Setting", _settingContext);
            ex.SetCustomerInfo(customerId, customerLevel, uplineLevel);
            ex.SetProductInfo(productId, customerSettingType);

            _logger.Error(ex.Message, ex);
        }
    }
}