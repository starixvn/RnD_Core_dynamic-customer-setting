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

    public partial class UpdateSettingService : IUpdateSettingService
    {
        public IResult Update(UpdateMultipleCustomerSettingParam updateMultipleParam)
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

        private IResult Validate(UpdateMultipleCustomerSettingParam updateMultipleParam)
        {
            return _validationService.ValidateUpdateMultipleSettingAction(updateMultipleParam);
        }

        private List<SettingResult> UpdateSettingTypesForMultipleCustomers(UpdateMultipleCustomerSettingParam updateMultipleParam)
        {
            var customerLevel = updateMultipleParam.Setting.CustomerLevel;
            var uplineLevel = updateMultipleParam.UplineLevel;
            var customerIds = updateMultipleParam.CustomerIds;
            var settingTypes = updateMultipleParam.Setting.SettingTypes;
            var productId = updateMultipleParam.Setting.ProductId;
            var results = new List<SettingResult>();

            foreach (var settingType in settingTypes)
            {
                var updateResults = UpdateMultipleCustomersSettingType(customerIds, customerLevel, uplineLevel, productId, settingType);

                results.AddRange(updateResults);
            }

            return results;
        }

        private IEnumerable<SettingResult> UpdateMultipleCustomersSettingType(
            IEnumerable<int> customerIds, 
            Level customerLevel, 
            Level uplineLevel, 
            byte productId, 
            SettingType settingType)
        {
            try
            {
                CheckSettingTypeIsValid(settingType);

                var settingTypeService = _settingTypeServiceFactory.GetInstance(settingType);

                return settingTypeService.UpdateMultipleCustomersSettingType(
                        _settingContext,
                        customerIds,
                        customerLevel,
                        uplineLevel,
                        productId,
                        settingType);
            }
            catch (CustomerSettingException customerSettingException)
            {
                WriteLog(customerIds, customerLevel, uplineLevel, productId, settingType, customerSettingException);

                return new List<SettingResult> 
                { 
                    new SettingResult(Result.AccessDenied.Code, customerSettingException.OriginalMessage, 0, 0, settingType.Id, settingType.Name)
                };
            }
            catch (Exception ex)
            {
                WriteLog(customerIds, customerLevel, uplineLevel, productId, settingType, new CustomerSettingException(ex.Message, ex));

                return new List<SettingResult> { new SettingResult(Result.SystemError, 0, settingType.Id, settingType.Name) };
            }
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

        private void WriteLog(
            IEnumerable<int> customerIds,
            Level customerLevel,
            Level uplineLevel,
            byte productId,
            SettingType customerSettingType,
            CustomerSettingException ex)
        {
            ex.SetSettingContextInfo("Update Multiple Setting", _settingContext);
            ex.SetCustomerInfo(string.Join(",", customerIds), customerLevel, uplineLevel);
            ex.SetProductInfo(productId, customerSettingType);

            _logger.Error(ex.Message, ex);
        }
    }
}
