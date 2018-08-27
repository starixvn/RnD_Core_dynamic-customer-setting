namespace Fanex.CustomerSetting.Matrix.WebApi.Configurations
{
    using System.Configuration;

    public class SettingMatrixApiConfiguration : ConfigurationSection
    {
        public static SettingMatrixApiConfiguration Settings { get; }
            = ConfigurationManager.GetSection("SettingMatrixAPIConfiguration") as SettingMatrixApiConfiguration;

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