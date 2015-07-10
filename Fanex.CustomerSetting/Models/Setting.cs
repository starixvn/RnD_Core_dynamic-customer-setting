namespace Fanex.CustomerSetting.Models
{
    using System;
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Enumerations;

    [Serializable]
    public class Setting : ISetting
    {
        private int _customerId;
        private CustomerLevel _customerLevel;
        private byte _productId;
        private IEnumerable<SettingType> _settingType;

        public Setting()
        {
        }

        public Setting(
            int customerId,
            CustomerLevel customerLevel,
            CustomerInfo customerInfo,
            byte productId,
            string productName,
            IEnumerable<SettingType> settingTypes)
        {
            CustomerId = customerId;
            CustomerLevel = customerLevel;
            CustomerInfo = customerInfo;
            ProductId = productId;
            ProductName = productName;
            SettingTypes = settingTypes;
        }

        public byte ProductId
        {
            get
            {
                return _productId;
            }

            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException("ProductId", "Product Id cannot be negative or zero.");
                }

                _productId = value;
            }
        }

        public string ProductName { get; private set; }

        public int CustomerId
        {
            get
            {
                return _customerId;
            }

            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException("CustomerId", "Customer Id cannot be negative or zero.");
                }

                _customerId = value;
            }
        }

        public CustomerLevel CustomerLevel
        {
            get
            {
                return _customerLevel;
            }

            set
            {
                var customerLevelValue = (byte)value;
                if (customerLevelValue <= 0 || customerLevelValue > (byte)CustomerLevel.Super)
                {
                    throw new ArgumentOutOfRangeException("CustomerLevel", "Invalid customer level.");
                }

                _customerLevel = value;
            }
        }

        public CustomerInfo CustomerInfo { get; private set; }

        public IEnumerable<SettingType> SettingTypes
        {
            get
            {
                return _settingType;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentOutOfRangeException("SettingTypes", "Setting Types cannot be null.");
                }

                _settingType = value;
            }
        }
    }
}