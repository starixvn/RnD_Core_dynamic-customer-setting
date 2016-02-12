namespace Fanex.CustomerSetting.Models.Requests
{
    using System;
    using Fanex.CustomerSetting.Models.Enumerations;

    public class CustomerSettingQuery
    {
        private int _customerId;
        private Level _customerLevel;
        private byte _productId;
        private Level _uplineLevel;

        public CustomerSettingQuery(
          int customerId,
          Level customerLevel,
          byte productId,
          Level uplineLevel = null)
        {
            CustomerId = customerId;
            CustomerLevel = customerLevel;
            ProductId = productId;
            UplineLevel = uplineLevel;
        }

        public CustomerSettingQuery(
           int customerId,
           byte customerLevel,
           byte productId,
           byte? uplineLevel)
        {
            CustomerId = customerId;
            CustomerLevel = Enumeration.FromValue<Level>(customerLevel);
            ProductId = productId;
            UplineLevel = uplineLevel.HasValue ? Enumeration.FromValue<Level>(uplineLevel.Value) : null;
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

        public Level CustomerLevel
        {
            get
            {
                return _customerLevel;
            }

            set
            {
                if (value < Level.Member || value > Level.Super)
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

        public Level UplineLevel
        {
            get
            {
                return _uplineLevel;
            }

            set
            {
                if (value != null)
                {
                    if (value < Level.Agent || value > Level.Super)
                    {
                        throw new ArgumentOutOfRangeException("value", "Upline Level is out of value range.");
                    }

                    // If current context is editting super/master/agent then Upline level should be null.
                    // TODO: WTF
                    if (_customerLevel.IsNotMemberLevel())
                    {
                        throw new ArgumentOutOfRangeException("value", "Upline should be null when getting customer setting is not member");
                    }
                }
                else if (_customerLevel.IsMemberLevel())
                {
                    // When editting member, upline level should have value
                    throw new ArgumentNullException("value", "Upline cannot be null when getting member setting.");
                }

                _uplineLevel = value;
            }
        }
    }
}