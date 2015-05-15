namespace Fanex.CustomerSetting.SettingTypes
{
    using Fanex.CustomerSetting.Resources;

    public class PositionTakingHandler : ISettingHandler
    {
        public string SuccessMsg
        {
            get
            {
                return Lang.UpdatePositionTakingSuccessfully;
            }
        }
    }
}