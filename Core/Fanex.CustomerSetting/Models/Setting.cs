namespace Fanex.CustomerSetting.Models
{
    using System;
    using System.Collections.Generic;
    using Enumerations;
    using Results;

    [Serializable]
    public class Setting : ISetting
    {
        private int _customerId;
        private Level _customerLevel;
        private byte _productId;
        private IEnumerable<SettingType> _settingType;

        public Setting()
        {
        }

        public Setting(
            int customerId,
            Level customerLevel,
            byte productId,
            IEnumerable<SettingType> settingTypes)
        {
            CustomerId = customerId;
            CustomerLevel = customerLevel;
            ProductId = productId;
            SettingTypes = settingTypes;
        }

        public Setting(
            int customerId,
            Level customerLevel,
            CustomerInfo customerInfo,
            byte productId,
            string productName,
            IEnumerable<SettingType> settingTypes)
            : this(customerId, customerLevel, productId, settingTypes)
        {
            CustomerInfo = customerInfo;
            ProductName = productName;
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
                    throw new ArgumentOutOfRangeException(nameof(value), "Product Id cannot be negative or zero.");
                }

                _productId = value;
            }
        }

        public string ProductName { get; protected set; }

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
                    throw new ArgumentOutOfRangeException(nameof(value), "Customer Id cannot be negative or zero.");
                }

                _customerId = value;
            }
        }

        public Level CustomerLevel
        {
            get
            {
                return _customerLevel;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                if (value < Level.Member || value > Level.Super)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Invalid customer level.");
                }

                _customerLevel = value;
            }
        }

        public CustomerInfo CustomerInfo { get; set; }

        public IEnumerable<SettingType> SettingTypes
        {
            get
            {
                if (_settingType == null)
                {
                    _settingType = new List<SettingType>();
                }

                return _settingType;
            }

            set
            {
                _settingType = value;
            }
        }

        public IResult WarningMessage { get; set; }
    }
}