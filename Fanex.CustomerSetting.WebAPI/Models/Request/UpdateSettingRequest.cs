namespace Fanex.CustomerSetting.WebAPI.Models.Request
{
    using Fanex.CustomerSetting.Models;

    public class UpdateSettingRequest : SettingRequest
    {
        public Setting Setting { get; set; }
    }
}