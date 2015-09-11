namespace Fanex.CustomerSetting.WebApi.Models
{
    using Fanex.CustomerSetting.Models;

    public class SettingContext : ISettingContext
    {
        public SettingContext()
        {
        }

        public User Actor
        {
            get;
            set;
        }

        public string Language
        {
            get;
            set;
        }

        public string IP
        {
            get;
            set;
        }

        public byte System
        {
            get;
            set;
        }

        public string Domain
        {
            get;
            set;
        }
    }
}