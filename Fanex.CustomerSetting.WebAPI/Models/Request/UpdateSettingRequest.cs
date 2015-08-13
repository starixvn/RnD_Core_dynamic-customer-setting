namespace Fanex.CustomerSetting.WebAPI.Models.Request
{
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Models.Enumerations;
    using Fanex.CustomerSetting.Models.Requests;

    public class UpdateSettingRequest : SettingRequest
    {
        public Setting Setting { get; set; }

        internal UpdateCustomerSettingCommand GetUpdateCustomerSettingCommand()
        {
            return new UpdateCustomerSettingCommand
            {
                Setting = this.Setting,
                UplineLevel = (this.UplineLevel == null)
                ? null
                : Enumeration.FromValue<Level>(this.UplineLevel.Value)
            };
        }
    }
}