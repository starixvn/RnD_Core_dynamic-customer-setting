namespace Fanex.CustomerSetting.Casino.Models
{
    using Common.Helper;
    using CustomerSetting.Models;

    public class CasinoSetting : Setting
    {
        public CasinoSetting(ISetting setting, bool customerProductSyncedStatus)
        {
            Check.Argument.IsNotNull(setting, "setting");

            CustomerId = setting.CustomerId;
            CustomerInfo = setting.CustomerInfo;
            CustomerLevel = setting.CustomerLevel;
            ProductId = setting.ProductId;
            ProductName = setting.ProductName;
            SettingTypes = setting.SettingTypes;
            WarningMessage = setting.WarningMessage;
            IsNormalMode = customerProductSyncedStatus;
        }

        public bool IsNormalMode { get; }
    }
}