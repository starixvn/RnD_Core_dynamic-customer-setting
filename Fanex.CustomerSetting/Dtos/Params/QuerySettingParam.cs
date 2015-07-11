namespace Fanex.CustomerSetting.Dtos.Params
{
    using System;
    using Fanex.CustomerSetting.Constants;
    using Fanex.CustomerSetting.Enumerations;

    public class QuerySettingParam
    {
        private int _customerId;
        private CustomerLevel _customerLevel;
        private byte _productId;
        private CustomerLevel? _uplineLevel;

        public QuerySettingParam(
            int customerId, 
            CustomerLevel customerLevel, 
            byte productId, 
            CustomerLevel? uplineLevel = null)
        {
            CustomerId = customerId;
            CustomerLevel = customerLevel;
            ProductId = productId;
            UplineLevel = uplineLevel;
        }

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
                    throw new ArgumentOutOfRangeException("value", "Customer Id cannot be negative or zero.");
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
                if (customerLevelValue < Level.Member || customerLevelValue > Level.Super)
                {
                    throw new ArgumentOutOfRangeException("value", "Invalid customer level.");
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
                    throw new ArgumentOutOfRangeException("value", "Product Id cannot be negative or zero.");
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
                    if (uplineLevelValue < Level.Member || uplineLevelValue > Level.Super)
                    {
                        throw new ArgumentOutOfRangeException("value", "Upline Level is out of value range.");
                    }
                    
                    //// If current context is editting super/master/agent then Upline level should be null.
                    if (_customerLevel != CustomerLevel.Member)
                    {
                        throw new ArgumentOutOfRangeException("value", "Upline should be null when getting customer setting is not member");
                    }
                }
                else if (_customerLevel == CustomerLevel.Member)
                {
                    //// When editting member, upline level should have value
                    throw new ArgumentNullException("value", "Upline cannot be null when getting member setting.");
                }

                _uplineLevel = value;
            }
        }
    }
}