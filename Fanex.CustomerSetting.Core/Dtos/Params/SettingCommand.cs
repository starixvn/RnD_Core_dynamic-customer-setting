namespace Fanex.CustomerSetting.Core.Dtos.Params
{
    using Fanex.CustomerSetting.Core.Enumerations;
    using Fanex.CustomerSetting.Core.Models;

    public class SettingCommand
    {
        public ISetting Setting { get; set; }

        public CustomerLevel? UplineLevel { get; set; }
    }
}