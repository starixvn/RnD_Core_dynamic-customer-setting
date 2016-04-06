namespace Fanex.CustomerSetting.Casino.Models
{
    using Common.Helper;
    using Fanex.CustomerSetting.Models;

    public class CasinoSetting : Setting
    {
        private CustomerProductStatus customerProductStatus;

        public CasinoSetting(ISetting setting, CustomerProductStatus customerProductStatus)
        {
            Check.Argument.IsNotNull(setting, "setting");
            Check.Argument.IsNotNull(customerProductStatus, "customerProductStatus");

            CustomerId = setting.CustomerId;
            CustomerInfo = setting.CustomerInfo;
            CustomerLevel = setting.CustomerLevel;
            ProductId = setting.ProductId;
            SettingTypes = setting.SettingTypes;
            WarningMessage = setting.WarningMessage;

            this.customerProductStatus = customerProductStatus;
        }

        public bool IsNormalMode
        {
            get
            {
                return customerProductStatus.IsSynced && customerProductStatus.IsDownlineSynced;
            }
        }
    }
}