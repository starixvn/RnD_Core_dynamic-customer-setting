namespace Fanex.CustomerSetting.WebApi.Models.Request
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Fanex.Common;
    using Fanex.CustomerSetting.Models.Requests;

    public class MultipleUpdateSettingRequest : UpdateSettingRequest
    {
        public string CustomerIds { get; set; }

        public string CustomerNames { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly", Justification = "Reviewed!")]
        internal MultipleUpdateCustomerSettingCommand GetMultipleUpdateCustomerSettingCommand()
        {
            UpdateCustomerSettingCommand updateSingleCommand = GetUpdateCustomerSettingCommand();

            if (string.IsNullOrWhiteSpace(CustomerIds))
            {
                throw new ArgumentNullException("CustomerIds", "CustomerIds cannot be null or empty");
            }

            if (string.IsNullOrWhiteSpace(CustomerNames))
            {
                throw new ArgumentNullException("CustomerNames", "CustomerNames cannot be null or empty");
            }

            return new MultipleUpdateCustomerSettingCommand(
                setting: updateSingleCommand.Setting,
                customerIds: CustomerIds.Split(',').Select(custId => Converter.ToInt(custId)),
                customerNames: CustomerNames.Split(',').ToList(),
                uplineLevel: updateSingleCommand.UplineLevel);
        }
    }
}