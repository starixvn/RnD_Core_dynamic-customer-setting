namespace Fanex.CustomerSetting.Casino.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Fanex.CustomerSetting.Models;

    public class CasinoSetting : Setting
    {
        private bool _isNormalModel = true;

        public CasinoSetting(ISetting setting)
        {
            this.CustomerId = setting.CustomerId;
            this.CustomerInfo = setting.CustomerInfo;
            this.CustomerLevel = setting.CustomerLevel;
            this.ProductId = setting.ProductId;
            this.SettingTypes = setting.SettingTypes;
            this.WarningMessage = setting.WarningMessage;
        }

        public bool IsNormalMode
        {
            get
            {
                return _isNormalModel;
            }
            set
            {
                _isNormalModel = value;
            }
        }
    }
}
