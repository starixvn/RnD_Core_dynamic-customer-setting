namespace Fanex.CustomerSetting.WebApi.Models.Request
{
    using Common;
    using CustomerSetting.Models.Requests;
    using Fanex.CustomerSetting.Configurations;
    using System;
    using System.Linq;

    public class MultipleUpdateSettingRequest : UpdateSettingRequest
    {
        public string CustomerIds { get; set; }

        public string CustomerNames { get; set; }

        internal MultipleUpdateCustomerSettingCommand GetMultipleUpdateCustomerSettingCommand()
        {
            UpdateCustomerSettingCommand updateSingleCommand = GetUpdateCustomerSettingCommand();

            if (string.IsNullOrWhiteSpace(CustomerIds))
            {
                throw new InvalidOperationException("Customer Ids cannot be null or empty");
            }

            if (string.IsNullOrWhiteSpace(CustomerNames))
            {
                throw new InvalidOperationException("Customer Names cannot be null or empty");
            }

            return new MultipleUpdateCustomerSettingCommand(
                setting: updateSingleCommand.Setting,
                customerIds: CustomerIds.Split(',').Select(custId => Converter.ToInt(custId)),
                customerNames: CustomerNames.Split(',').ToList(),
                settingStatus: AppSettings.SettingStatus,
                uplineLevel: updateSingleCommand.UplineLevel);
        }
    }
}