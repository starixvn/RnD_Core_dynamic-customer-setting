namespace Fanex.CustomerSetting.Dtos.Params
{
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Models;

    public class SettingCommand
    {
        public ISetting Setting { get; set; }

        public CustomerLevel? UplineLevel { get; set; }
    }
}