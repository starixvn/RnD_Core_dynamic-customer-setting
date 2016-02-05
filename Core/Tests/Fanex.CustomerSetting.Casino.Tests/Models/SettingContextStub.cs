namespace Fanex.CustomerSetting.Casino.Tests
{
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Models.Enumerations;

    public class SettingContextStub : ISettingContext
    {
        private User _actor = new User
        {
            Id = 100,
            Level = Level.Company,
            Name = "Admin"
        };

        private byte _systemId = 1;

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
            get
            {
                return _systemId;
            }

            set
            {
                _systemId = value;
            }
        }

        public string Domain
        {
            get { return "Domain Name"; }
        }
    }
}