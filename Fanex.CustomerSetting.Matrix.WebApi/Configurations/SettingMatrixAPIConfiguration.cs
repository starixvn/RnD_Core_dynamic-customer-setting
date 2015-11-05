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

        [ConfigurationProperty("SecretKey", IsRequired = true, DefaultValue = "oOcEDP63hAXyWYjJ2DRbOYWLdx/eIaKZ5AWoiunpeMk=")]
        public string SecretKey
        {
            get { return (string)this["SecretKey"]; }
            set { this["SecretKey"] = value; }
        }
    }
}