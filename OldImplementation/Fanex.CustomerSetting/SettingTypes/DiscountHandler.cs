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

        public string ErrorMsgPrefix
        {
            get
            {
                return "Discount";
            }
        }
    }
}