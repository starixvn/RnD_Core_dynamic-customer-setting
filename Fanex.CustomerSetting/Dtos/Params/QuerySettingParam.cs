namespace Fanex.CustomerSetting.Dtos.Params
{
    using System;
    using Fanex.CustomerSetting.Constants;
    using Fanex.CustomerSetting.Enumerations;

    public class QuerySettingParam
    {
        private int _customerId;
        private UserLevel _customerLevel;
        private byte _productId;
        private UserLevel _uplineLevel;

        public QuerySettingParam(
            int customerId, 
            UserLevel customerLevel, 
            byte productId, 
            UserLevel uplineLevel = null)
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

        public UserLevel CustomerLevel
        {
            get
            {
                return _customerLevel;
            }

            set
            {
                var customerLevelValue = value.Value;
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

        public UserLevel UplineLevel
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
                    if (uplineLevelValue < Level.Agent || uplineLevelValue > Level.Super)
                    {
                        throw new ArgumentOutOfRangeException("value", "Upline Level is out of value range.");
                    }
                    
                    //// If current context is editting super/master/agent then Upline level should be null.
                    if (_customerLevel != UserLevel.Member)
                    {
                        throw new ArgumentOutOfRangeException("value", "Upline should be null when getting customer setting is not member");
                    }
                }
                else if (_customerLevel == UserLevel.Member)
                {
                    //// When editting member, upline level should have value
                    throw new ArgumentNullException("value", "Upline cannot be null when getting member setting.");
                }

                _uplineLevel = value;
            }
        }
    }
}