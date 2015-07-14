namespace Fanex.CustomerSetting.Tests.Models
{
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Models;

    public class SettingContextStub : ISettingContext
    {
        private User _actor = new User
        {
            Id = 100,
            Level = UserLevel.Company
        };

        private User _subAccount = new User
        {
            Id = 2
        };

        private string _language = "en-US";

        public User Actor
        {
            get
            {
                return _actor;
            }

            set
            {
                _actor = value;
            }
        }

        public User SubAccount
        {
            get
            {
                return _subAccount;
            }

            set
            {
                _subAccount = value;
            }
        }

        public string Language
        {
            get
            {
                return _language;
            }

            set
            {
                _language = value;
            }
        }

        public string IP
        {
            get { return "192.168.1.1"; }
        }

        public byte System
        {
            get { return 1; }
        }

        public string Domain
        {
            get { return "Domain Name"; }
        }
    }
}