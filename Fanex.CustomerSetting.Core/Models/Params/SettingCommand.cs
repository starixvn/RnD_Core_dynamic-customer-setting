namespace Fanex.CustomerSetting.Core.Models.Params
{
    using Fanex.CustomerSetting.Core.Enumerations;

    public class SettingCommand
    {
        public ISetting Setting { get; set; }

        public CustomerLevel? UplineLevel { get; set; }
    }
}