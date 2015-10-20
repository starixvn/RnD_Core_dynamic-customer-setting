namespace Fanex.CustomerSetting.WebApi.Configurations
{
    using System.Configuration;

    public class CustomerSettingAPIConfiguration : ConfigurationSection
    {
        private static CustomerSettingAPIConfiguration settings
            = ConfigurationManager.GetSection("CustomerSettingAPIConfiguration") as CustomerSettingAPIConfiguration;

        public static CustomerSettingAPIConfiguration Settings
        {
            get
            {
                return settings;
            }
        }

        [ConfigurationProperty("EnableCheckUM", IsRequired = false, DefaultValue = true)]
        public bool EnableCheckUM
        {
            get { return (bool)this["EnableCheckUM"]; }
            set { this["EnableCheckUM"] = value; }
        }

        [ConfigurationProperty("UMAddress", IsRequired = true)]
        public string UMAddress
        {
            get { return (string)this["UMAddress"]; }
            set { this["UMAddress"] = value; }
        }
    }
}