namespace Fanex.CustomerSetting.WebAPI.Models
{
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Models.Enumerations;
    using Fanex.CustomerSetting.WebAPI.Models.Request;

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