namespace Fanex.CustomerSetting.Dtos.Params
{
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Models;

    public class UpdatingSettingParams
    {
        public ISetting Setting { get; set; }

        public CustomerLevel? UplineLevel { get; set; }
    }
}