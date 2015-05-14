namespace Fanex.CustomerSetting.Models
{
    using Fanex.CustomerSetting.Enumerations;

    public class NoChangeSettingResult : SettingResult
    {
        public NoChangeSettingResult(SettingType settingType)
            : base(settingType, Result.NoChange)
        {
        }
    }
}