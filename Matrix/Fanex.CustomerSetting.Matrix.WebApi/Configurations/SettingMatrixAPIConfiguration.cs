namespace Fanex.CustomerSetting.Matrix.WebApi.Configurations
{
    using System.Configuration;

    public class SettingMatrixAPIConfiguration : ConfigurationSection
    {
        private static SettingMatrixAPIConfiguration settings
            = ConfigurationManager.GetSection("SettingMatrixAPIConfiguration") as SettingMatrixAPIConfiguration;

        public static SettingMatrixAPIConfiguration Settings
        {
            get
            {
                return settings;
            }
        }

        [ConfigurationProperty("SecretKey", IsRequired = true, DefaultValue = "oOcEDP63hAXyWYjJ2DRbOYWLdx/eIaKZ5AWoiunpeMk=")]
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