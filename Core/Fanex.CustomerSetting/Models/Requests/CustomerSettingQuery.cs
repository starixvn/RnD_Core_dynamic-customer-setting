namespace Fanex.CustomerSetting.Models.Requests
{
    using System;
    using Enumerations;

    public class CustomerSettingQuery
    {
        private int _customerId;
        private Level _customerLevel;
        private byte _productId;
        private Level _uplineLevel;
        private byte _settingStatus;

        public CustomerSettingQuery(
            int customerId,
            Level customerLevel,
            byte productId,
            byte settingStatus = 0,
            Level uplineLevel = null)
        {
            CustomerId = customerId;
            CustomerLevel = customerLevel;
            ProductId = productId;
            UplineLevel = uplineLevel;
            SettingStatus = settingStatus;
        }

        public CustomerSettingQuery(
            int customerId,
            byte customerLevel,
            byte productId,
            byte settingStatus,
            byte? uplineLevel)
        {
            CustomerId = customerId;
            CustomerLevel = Enumeration.FromValue<Level>(customerLevel);
            ProductId = productId;
            SettingStatus = settingStatus;
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
                if (value < Level.Member || value > Level.Super)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Invalid customer level.");
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
                    throw new ArgumentOutOfRangeException(nameof(value), "Product Id cannot be negative or zero.");
                }

                _productId = value;
            }
        }

        public byte SettingStatus
        {
            get
            {
                return _settingStatus;
            }

            set
            {
                if (value < 0 || value > 1)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Invalid Setting Status.");
                }

                _settingStatus = value;
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
                        throw new ArgumentOutOfRangeException(nameof(value), "Upline Level is out of value range.");
                    }

                    // If current context is editting super/master/agent then Upline level should be null.
                    // TODO: WTF
                    if (_customerLevel.IsNotMemberLevel())
                    {
                        throw new ArgumentOutOfRangeException(nameof(value), "Upline should be null when getting customer setting is not member");
                    }
                }
#pragma warning disable S126 // "if ... else if" constructs should end with "else" clauses
                else if (_customerLevel.IsMemberLevel())
#pragma warning restore S126 // "if ... else if" constructs should end with "else" clauses
                {
                    // When editting member, upline level should have value
                    throw new ArgumentNullException(nameof(value), "Upline cannot be null when getting member setting.");
                }

                _uplineLevel = value;
            }
        }
    }
}