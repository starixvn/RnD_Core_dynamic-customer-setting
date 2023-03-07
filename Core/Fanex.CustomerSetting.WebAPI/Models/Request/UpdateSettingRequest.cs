namespace Fanex.CustomerSetting.WebApi.Models.Request
{
    using CustomerSetting.Models;
    using CustomerSetting.Models.Enumerations;
    using CustomerSetting.Models.Requests;
    using Fanex.CustomerSetting.Configurations;

    public class UpdateSettingRequest : SettingRequest
    {
        public Setting Setting { get; set; }

        internal UpdateCustomerSettingCommand GetUpdateCustomerSettingCommand()
        {
            return new UpdateCustomerSettingCommand(
                setting: Setting,
                settingStatus: AppSettings.SettingStatus,
                uplineLevel: UplineLevel == null ? null : Enumeration.FromValue<Level>(UplineLevel.Value));
        }
    }
}