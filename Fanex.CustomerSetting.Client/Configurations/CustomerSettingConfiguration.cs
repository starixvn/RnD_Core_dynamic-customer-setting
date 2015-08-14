namespace Fanex.CustomerSetting.Client.Configurations
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Text;

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
            get { return (string)this["System"]; }
            set { this["System"] = value; }
        }

        [ConfigurationProperty("ServiceUri", IsRequired = true)]
        public string ServiceUri
        {
            get { return (string)this["ServiceUri"]; }
            set { this["ServiceUri"] = value; }
        }

        [ConfigurationProperty("SecrectKey", IsRequired = true)]
        public string SecrectKey
        {
            get { return (string)this["SecrectKey"]; }
            set { this["SecrectKey"] = value; }
        }
    }
}
