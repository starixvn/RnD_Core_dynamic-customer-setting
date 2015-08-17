namespace Fanex.CustomerSetting.WebAPI.Models.Request
{
    using System.Linq;
    using Fanex.Common;
    using Fanex.CustomerSetting.Models.Requests;

    public class MultipleUpdateSettingRequest : UpdateSettingRequest
    {
        public string CustomerIds { get; set; }

        public string CustomerNames { get; set; }

        internal MultipleUpdateCustomerSettingCommand GetMultipleUpdateCustomerSettingCommand()
        {
            UpdateCustomerSettingCommand updateSingleCommand = GetUpdateCustomerSettingCommand();

            return new MultipleUpdateCustomerSettingCommand
            {
                Setting = updateSingleCommand.Setting,
                UplineLevel = updateSingleCommand.UplineLevel,
                CustomerIds = CustomerIds.Split(',').Select(custId => Converter.ToInt(custId)),
                CustomerNames = CustomerNames.Split(',').ToList()
            };
        }
    }
}