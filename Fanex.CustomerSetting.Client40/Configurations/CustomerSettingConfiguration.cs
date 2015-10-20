namespace Fanex.CustomerSetting.Client40.Configurations
{
    using System;
    using System.Configuration;

    public class CustomerSettingConfiguration : ConfigurationSection
    {
        private static CustomerSettingConfiguration settings
            = ConfigurationManager.GetSection("CustomerSettingConfiguration") as CustomerSettingConfiguration;

        public static CustomerSettingConfiguration Settings
        {
            get
            {
                return settings;
            }
        }

        [ConfigurationProperty("System", IsRequired = true)]
        public byte System
        {
            get { return (byte)this["System"]; }
            set { this["System"] = value; }
        }

        [ConfigurationProperty("ServiceUris", IsRequired = true)]
        public string ServiceUris
        {
            get { return (string)this["ServiceUris"]; }
            set { this["ServiceUris"] = value; }
        }

        [ConfigurationProperty("SecrectKey", IsRequired = false)]
        public string SecrectKey
        {
            get { return (string)this["SecrectKey"]; }
            set { this["SecrectKey"] = value; }
        }

        [ConfigurationProperty("CdnUris", IsRequired = true)]
        public string CdnUris
        {
            get { return (string)this["CdnUris"]; }
            set { this["CdnUris"] = value; }
        }

        [ConfigurationProperty("CustomCssUrl")]
        public Uri CustomCssUrl
        {
            get { return (Uri)this["CustomCssUrl"]; }
            set { this["CustomCssUrl"] = value; }
        }

        [ConfigurationProperty("ClientLogCategory", IsRequired = false, DefaultValue = "DCS-Setting-Client-Log")]
        public string ClientLogCategory
        {
            get { return (string)this["ClientLogCategory"]; }
            set { this["ClientLogCategory"] = value; }
        }
    }
}