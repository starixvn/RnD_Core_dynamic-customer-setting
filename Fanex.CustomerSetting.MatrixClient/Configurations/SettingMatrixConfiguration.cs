namespace Fanex.CustomerSetting.MatrixClient.Configurations
{
    using System.Configuration;
    using System.Diagnostics.CodeAnalysis;

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

        [SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings", Justification = "Reviewed !"),
        ConfigurationProperty("ServiceUri", IsRequired = true)]
        public string ServiceUri
        {
            get { return (string)this["ServiceUri"]; }
            set { this["ServiceUri"] = value; }
        }
    }
}