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

        [ConfigurationProperty("SecretKey", IsRequired = true, DefaultValue = "U6BUFHtVcq5SPQcTl7tHLBG/LF6d5YVRKHrCHD9M+Vw=")]
        public string SecretKey
        {
            get { return (string)this["SecretKey"]; }
            set { this["SecretKey"] = value; }
        }

        [ConfigurationProperty("EnableRequestLogging", IsRequired = false, DefaultValue = true)]
        public bool EnableRequestLogging
        {
            get { return (bool)this["EnableRequestLogging"]; }
            set { this["EnableRequestLogging"] = value; }
        }
    }
}