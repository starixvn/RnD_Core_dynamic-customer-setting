namespace Fanex.CustomerSetting.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common.Helper;
    using Exceptions;
    using Models;
    using Models.Enumerations;
    using Models.Requests;
    using Models.Results;

    public partial class UpdateSettingService : IUpdateSettingService
    {
        public IResult Update(MultipleUpdateCustomerSettingCommand command)
        {
            Check.Argument.IsNotNull(command, "command");
            Check.Argument.IsNotNegativeOrZero(command.Setting.SettingTypes.Count(), "command.Setting.SettingTypes.Count");

            var validationResult = Validate(command);

            if (validationResult.IsNotOk)
            {
                return validationResult;
            }

            // Run plugged tasks before updating settings of mutiple customers.
            _taskService.Run(command.Setting.ProductId, command);

            var updateSettingTypeResults = UpdateSettingTypesForMultipleCustomers(command);
            var result = MapUpdateSettingResultWithCustomerName(updateSettingTypeResults, command.CustomerIds, command.CustomerNames);

            // Run plugged tasks after updating settings of mutiple customers.
            _taskService.Run(command.Setting.ProductId, command, result);

            return result;
        }

        private IResult Validate(MultipleUpdateCustomerSettingCommand command)
        {
            return _validationService.ValidateUpdateMultipleSettingAction(command);
        }

        private List<SettingResult> UpdateSettingTypesForMultipleCustomers(MultipleUpdateCustomerSettingCommand command)
        {
            var customerLevel = command.Setting.CustomerLevel;
            var uplineLevel = command.UplineLevel;
            var customerIds = command.CustomerIds;
            var settingTypes = command.Setting.SettingTypes;
            var productId = command.Setting.ProductId;
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
                _taskService.Run(productId, settingType.Id, customerIds, customerLevel, uplineLevel, settingType);

                var settingTypeService = _settingTypeServiceFactory.GetInstance(settingType);
                var updateSettingResults = settingTypeService.UpdateMultipleCustomerSettingType(
                        _settingContext,
                        customerIds,
                        customerLevel,
                        uplineLevel,
                        productId,
                        settingType);

                _taskService.Run(productId, settingType.Id, customerIds, customerLevel, uplineLevel, settingType, updateSettingResults);

                return updateSettingResults;
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