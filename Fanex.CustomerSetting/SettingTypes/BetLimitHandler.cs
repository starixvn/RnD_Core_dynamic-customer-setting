namespace Fanex.CustomerSetting.SettingTypes
{
    using Fanex.CustomerSetting.Resources;

    public class BetLimitHandler : ISettingHandler
    {
        public string SuccessMsg
        {
            get
            {
                return Lang.UpdateBetSettingSuccessfully;
            }
        }
    }
}