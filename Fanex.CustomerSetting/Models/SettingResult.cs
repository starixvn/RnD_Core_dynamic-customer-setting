namespace Fanex.CustomerSetting.Models
{
    using Fanex.CustomerSetting.Enumerations;

    public class SettingResult : Result
    {
        public SettingResult(SettingType settingType, Result result)
            : base(result)
        {
            SettingType = settingType;
        }

        public SettingType SettingType { get; private set; }
    }
}