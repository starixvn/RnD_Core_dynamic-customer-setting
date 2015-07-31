namespace Fanex.CustomerSetting.Models
{
    using System;
    using Fanex.CustomerSetting.Models.Enumerations;

    [Serializable]
    public class User : Entity<int>
    {
        private int _id;
        private string _name;
        private int _subAccountId;
        private Level _level;

        public new int Id
        {
            get
            {
                return _id;
            }

            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException("value", "User Id cannot be negative or zero.");
                }

                _id = value;
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentOutOfRangeException("value", "Username cannot be null or empty.");
                }

                _name = value;
            }
        }

        public int SubAccountId
        {
            get
            {
                return _subAccountId;
            }

            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("value", "Sub account id cannot be negative.");
                }

                _subAccountId = value;
            }
        }

        public Level Level
        {
            get
            {
                return _level;
            }

            set
            {
                if (ReferenceEquals(value, null))
                {
                    throw new ArgumentNullException("value");
                }

                if (value == Level.Member)
                {
                    throw new ArgumentOutOfRangeException("value", "Cannot set level Member to user");
                }

                _level = value;
            }
        }
        
        public bool IsCompany
        {
            get
            {
                return this.Level == Level.Company;
            }
        }

        public bool IsNotCompany
        {
            get
            {
                return !this.IsCompany;
            }
        }
    }
}