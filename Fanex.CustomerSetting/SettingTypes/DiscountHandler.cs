namespace Fanex.CustomerSetting.SettingTypes
{
    using Fanex.CustomerSetting.Resources;

    public class DiscountHandler : ISettingHandler
    {
        public string SuccessMsg
        {
            get
            {
                return Lang.UpdateCommissionSuccessfully;
            }
        }
    }
}