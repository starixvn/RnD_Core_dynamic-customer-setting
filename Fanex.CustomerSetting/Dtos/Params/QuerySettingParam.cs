namespace Fanex.CustomerSetting.Dtos.Params
{
    using System;
    using Fanex.CustomerSetting.Enumerations;

    public class QuerySettingParam
    {
        private int _customerId;
        private CustomerLevel _customerLevel;
        private byte _productId;
        private CustomerLevel? _uplineLevel;

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

                if (value != Enumerations.CustomerLevel.Member
                    && _uplineLevel != null)
                {
                    throw new ArgumentOutOfRangeException("UplineLevel", "Upline should be null when get other customer setting exception member.");
                }

                _customerLevel = value;
            }
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

        public CustomerLevel? UplineLevel
        {
            get
            {
                return _uplineLevel;
            }

            set
            {
                if (value != null)
                {
                    byte uplineLevelValue = (byte)value.Value;
                    if (uplineLevelValue <= 0 || uplineLevelValue > (byte)CustomerLevel.Super)
                    {
                        throw new ArgumentOutOfRangeException("UplineLevel", "Upline Level is out of value range.");
                    }

                    if (_customerLevel != CustomerLevel.Member)
                    {
                        throw new ArgumentOutOfRangeException("UplineLevel", "Upline should be null when getting customer setting is not member");
                    }
                }

                if (_customerLevel == CustomerLevel.Member && value == null)
                {
                    throw new ArgumentOutOfRangeException("UplineLevel", "Upline cannot be null when getting member setting.");
                }

                _uplineLevel = value;
            }
        }
    }
}