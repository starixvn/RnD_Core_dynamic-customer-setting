namespace Fanex.CustomerSetting.SettingTypes
{
    using Fanex.CustomerSetting.Resources;

    public class StatusHandler : ISettingHandler
    {
        public string SuccessMsg
        {
            get
            {
                return Lang.UpdateProductStatusSuccessfully;
            }
        }
    }
}