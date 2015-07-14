namespace Fanex.CustomerSetting.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Fanex.Common.Helper;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Models;
    using Fanex.Logging;

    public class LoggingService : ILoggingService
    {
        private readonly ISettingContext _settingContext;
        private readonly ILogger _logger;

        public LoggingService(
            ISettingContext settingContext,
            ILogger logger = null)
        {
            Check.Argument.IsNotNull(settingContext, "settingContext");

            _settingContext = settingContext;
            _logger = logger ?? Logger.Log;
        }

        public void LogQueryCustomerSettingTypeError(
            int customerId,
            CustomerLevel? uplineLevel,
            byte productId,
            SettingType settingType,
            Exception exception)
        {
            var errorMessage = string.Join(
                string.Empty,
                GetSettingContextInformation("Query Customer Setting Type Exception"),
                GetCustomerInformation(customerId, uplineLevel),
                GetProductInformation(productId),
                GetSettingTypeInformation(settingType),
                GetExceptionMessage(exception));

            _logger.Error(errorMessage, exception);
        }

        public void LogUpdatingCustomerSettingTypeError(
            int customerId,
            CustomerLevel? uplineLevel,
            byte productId,
            SettingType settingType,
            Exception exception)
        {
            var errorMessage = string.Join(
                string.Empty,
                GetSettingContextInformation("Updating Customer Setting Type Exception"),
                GetCustomerInformation(customerId, uplineLevel),
                GetProductInformation(productId),
                GetSettingTypeInformation(settingType),
                GetExceptionMessage(exception));

            _logger.Error(errorMessage, exception);
        }

        public void LogUpdatingMultipleCustomersSettingTypeError(
            IEnumerable<int> customerIds,
            CustomerLevel? uplineLevel,
            byte productId,
            SettingType settingType,
            Exception exception)
        {
            var errorMessage = string.Join(
                string.Empty,
                GetSettingContextInformation("Updating Multiple Customers Setting Type Exception"),
                GetCustomersInformation(customerIds, uplineLevel),
                GetProductInformation(productId),
                GetSettingTypeInformation(settingType),
                GetExceptionMessage(exception));

            _logger.Error(errorMessage, exception);
        }

        public void LogValidatingUpdatingError(
            int customerId,
            CustomerLevel? uplineLevel,
            byte productId,
            Exception exception)
        {
            var errorMessage = string.Join(
                string.Empty,
                GetSettingContextInformation("Validating Update Customer Setting Exception"),
                GetCustomerInformation(customerId, uplineLevel),
                GetProductInformation(productId),
                GetExceptionMessage(exception));

            _logger.Error(errorMessage, exception);
        }

        public void LogValidatingUpdatingMultipleError(
            IEnumerable<int> customerIds,
            CustomerLevel? uplineLevel,
            byte productId,
            Exception exception)
        {
            var errorMessage = string.Join(
                string.Empty,
                GetSettingContextInformation("Validating Update Multiple Customers Setting Exception"),
                GetCustomersInformation(customerIds, uplineLevel),
                GetProductInformation(productId),
                GetExceptionMessage(exception));

            _logger.Error(errorMessage, exception);
        }

        private string GetSettingContextInformation(string actionName)
        {
            var settingContextInfo = new StringBuilder();

            settingContextInfo.AppendLine(actionName);

            settingContextInfo.AppendLine(string.Format("Actor Id: {0}", _settingContext.Actor.Id));
            settingContextInfo.AppendLine(string.Format("Actor Name: {0}", _settingContext.Actor.Name));
            settingContextInfo.AppendLine(string.Format("Actor Level: {0}", _settingContext.Actor.Level.DisplayName));
            settingContextInfo.AppendLine(string.Format("Sub Account Id: {0}", _settingContext.Actor.SubAccountId));

            settingContextInfo.AppendLine(string.Format("Language: {0}", _settingContext.Language));
            settingContextInfo.AppendLine(string.Format("IP: {0}", _settingContext.IP));
            settingContextInfo.AppendLine(string.Format("System: {0}", _settingContext.System));
            settingContextInfo.AppendLine(string.Format("Domain: {0}", _settingContext.Domain));

            return settingContextInfo.ToString();
        }

        private string GetSettingTypeInformation(SettingType settingType)
        {
            var settingTypeInfo = new StringBuilder();

            settingTypeInfo.AppendLine(string.Format("Setting Type Id: {0}", settingType.Id));
            settingTypeInfo.AppendLine(string.Format("Setting Type Name: {0}", settingType.Name));
            settingTypeInfo.AppendLine(string.Format("Setting Type Level: {0}", settingType.SettingLevel));
            settingTypeInfo.AppendLine(string.Format("Setting Type Template: {0}", settingType.SettingTemplate));

            return settingTypeInfo.ToString();
        }

        private string GetProductInformation(byte productId)
        {
            return string.Format("ProductId: {0}\r\n", productId);
        }

        private string GetCustomerInformation(int customerId, CustomerLevel? uplineLevel)
        {
            var customerInfo = new StringBuilder();

            customerInfo.AppendLine(string.Format("Customer Id: {0}", customerId));

            if (uplineLevel.HasValue)
            {
                customerInfo.AppendLine(GetUplineInformation(uplineLevel));
            }

            return customerInfo.ToString();
        }

        private string GetCustomersInformation(IEnumerable<int> customerIds, CustomerLevel? uplineLevel)
        {
            var customerInfo = new StringBuilder();

            customerInfo.AppendLine(string.Format("Customer Ids: {0}", string.Join(",", customerIds)));

            if (uplineLevel.HasValue)
            {
                customerInfo.AppendLine(GetUplineInformation(uplineLevel));
            }

            return customerInfo.ToString();
        }

        private string GetUplineInformation(CustomerLevel? uplineLevel)
        {
            return string.Format("Upline Level: {0}", Enum.GetName(typeof(CustomerLevel), uplineLevel.Value));
        }

        private string GetExceptionMessage(Exception ex)
        {
            return string.Format("Exception Message: {0}\r\n", ex.Message);
        }
    }
}