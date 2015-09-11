namespace Fanex.CustomerSetting.MatrixClient.Configurations
{
    using System;
    using System.Configuration;

    public class SettingMatrixConfiguration : ConfigurationSection
    {
        private static SettingMatrixConfiguration settings
            = ConfigurationManager.GetSection("SettingMatrixConfiguration") as SettingMatrixConfiguration;

        public static SettingMatrixConfiguration Settings
        {
            get
            {
                return settings;
            }
        }

        [ConfigurationProperty("ServiceUri", IsRequired = true)]
        public Uri ServiceUri
        {
            get { return (Uri)this["ServiceUri"]; }
            set { this["ServiceUri"] = value; }
        }

        [ConfigurationProperty("CdnUri", IsRequired = true)]
        public Uri CdnUri
        {
            get { return (Uri)this["CdnUri"]; }
            set { this["CdnUri"] = value; }
        }
    }
}