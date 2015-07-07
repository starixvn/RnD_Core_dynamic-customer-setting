namespace Fanex.CustomerSetting.Models
{
    using System;
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Enumerations;

    [Serializable]
    public class Setting : ISetting
    {
        private CustomerInfo _customerInfo;
        private string _productName;

        public Setting()
        {
        }

        public Setting(CustomerInfo customerInfo, string productName)
        {
            _customerInfo = customerInfo;
            _productName = productName;
        }

        public byte ProductId { get; set; }

        public string ProductName
        {
            get
            {
                return _productName;
            }

            set
            {
                _productName = value;
            }
        }

        public int CustomerId { get; set; }

        public CustomerLevel CustomerLevel { get; set; }

        public CustomerInfo CustomerInfo
        {
            get
            {
                return _customerInfo;
            }

            set
            {
                _customerInfo = value;
            }
        }

        public IEnumerable<SettingType> SettingTypes { get; set; }
    }
}