namespace Fanex.CustomerSetting.Models
{
    using System;
    using Fanex.CustomerSetting.Enumerations;

    [Serializable]
    public class User : Entity<int>
    {
        private int _id;   
        private string _name;
        private int _subAccountId;
        private UserLevel _userLevel;
     

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
                if (string.IsNullOrEmpty(value))
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

        public UserLevel Level
        {
            get
            {
                return _userLevel;
            }

            set
            {
                if ((byte)value < Constants.Level.Agent || (byte)value > Constants.Level.Company)
                {
                    throw new ArgumentOutOfRangeException("value", "User level is invalid.");
                }

                _userLevel = value;
            }
        }

        /// <summary>
        /// Check whether input level is direct of the instance.
        /// </summary>
        /// <param name="downLineLevel"></param>
        /// <returns>true if input level is direct downline of the instance</returns>
        public bool IsDirectDownlineLevel(UserLevel downLineLevel)
        {
            return (byte)Level - (byte)downLineLevel == 1;
        }

        //public bool Is<T>() 
        //{
        //    if (T is UserLevel)
        //    {
        //        return T == Level;
        //    }

        //    throw new InvalidOperationException();
        //}
    }
}