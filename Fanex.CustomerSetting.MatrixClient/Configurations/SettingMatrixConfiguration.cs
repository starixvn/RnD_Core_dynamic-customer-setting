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

        [ConfigurationProperty("ServiceUris", IsRequired = true)]
        public string ServiceUris
        {
            get { return (string)this["ServiceUris"]; }
            set { this["ServiceUris"] = value; }
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

        [ConfigurationProperty("ClientLogCategory", IsRequired = false, DefaultValue = "DCS-Matrix-Client-Log")]
        public string ClientLogCategory
        {
            get { return (string)this["ClientLogCategory"]; }
            set { this["ClientLogCategory"] = value; }
        }
    }
}