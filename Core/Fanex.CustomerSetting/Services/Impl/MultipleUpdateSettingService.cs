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
        public virtual IResult Update(MultipleUpdateCustomerSettingCommand command)
        {
            Check.Argument.IsNotNull(command, "command");
            Check.Argument.IsNotNegativeOrZero(command.Setting.SettingTypes.Count(), "command.Setting.SettingTypes.Count");

            var validationResult = Validate(command);

            if (validationResult.IsNotOk)
            {
                return validationResult;
            }

            // Run plugged tasks before updating settings of mutiple customers.
            TaskService.Run(command.Setting.ProductId, command);

            var updateSettingTypeResults = UpdateSettingTypesForMultipleCustomers(command);
            var result = MapUpdateSettingResultWithCustomerName(updateSettingTypeResults, command.CustomerIds, command.CustomerNames);

            // Run plugged tasks after updating settings of mutiple customers.
            TaskService.Run(command.Setting.ProductId, command, result);

            return result;
        }

        protected virtual IResult Validate(MultipleUpdateCustomerSettingCommand command)
        {
            return ValidationService.ValidateUpdateMultipleSettingAction(command);
        }

        protected virtual List<SettingResult> UpdateSettingTypesForMultipleCustomers(MultipleUpdateCustomerSettingCommand command)
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

        protected virtual IEnumerable<SettingResult> UpdateMultipleCustomersSettingType(
            IEnumerable<int> customerIds,
            Level customerLevel,
            Level uplineLevel,
            byte productId,
            SettingType settingType)
        {
            try
            {
                CheckSettingTypeIsValid(settingType);
                TaskService.Run(productId, settingType.Id, customerIds, customerLevel, uplineLevel, settingType);

                var settingTypeService = SettingTypeServiceFactory.GetInstance(settingType);
                var updateSettingResults = settingTypeService.UpdateMultipleCustomerSettingType(
                        SettingContext,
                        customerIds,
                        customerLevel,
                        uplineLevel,
                        productId,
                        settingType);

                TaskService.Run(productId, settingType.Id, customerIds, customerLevel, uplineLevel, settingType, updateSettingResults);

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

        protected virtual UpdateSettingResult MapUpdateSettingResultWithCustomerName(
            IEnumerable<SettingResult> settingResults,
            IEnumerable<int> customerIds,
            IEnumerable<string> customerNames)
        {
            var customerIdNameContainer = GetCustomerIdNameContainer(customerIds, customerNames);

            return MapSettingResultWithCustomerName(customerIdNameContainer, settingResults);
        }

        protected virtual UpdateSettingResult MapSettingResultWithCustomerName(
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

        protected virtual Dictionary<int, string> GetCustomerIdNameContainer(
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

        protected virtual void WriteLog(
            IEnumerable<int> customerIds,
            Level customerLevel,
            Level uplineLevel,
            byte productId,
            SettingType customerSettingType,
            CustomerSettingException ex)
        {
            ex.SetSettingContextInfo("Update Multiple Setting", SettingContext);
            ex.SetCustomerInfo(string.Join(",", customerIds), customerLevel, uplineLevel);
            ex.SetProductInfo(productId, customerSettingType);

            Logger.Error(ex.Message, ex);
        }
    }
}