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

        [ConfigurationProperty("ServiceUri", IsRequired = true)]
        public Uri ServiceUri
        {
            get { return (Uri)this["ServiceUri"]; }
            set { this["ServiceUri"] = value; }
        }

        [ConfigurationProperty("SecrectKey", IsRequired = true)]
        public string SecrectKey
        {
            get { return (string)this["SecrectKey"]; }
            set { this["SecrectKey"] = value; }
        }

        [ConfigurationProperty("CdnUri", IsRequired = true)]
        public Uri CdnUri
        {
            get { return (Uri)this["CdnUri"]; }
            set { this["CdnUri"] = value; }
        }

        [ConfigurationProperty("CustomCssUrl")]
        public Uri CustomCssUrl
        {
            get { return (Uri)this["CustomCssUrl"]; }
            set { this["CustomCssUrl"] = value; }
        }
    }
}