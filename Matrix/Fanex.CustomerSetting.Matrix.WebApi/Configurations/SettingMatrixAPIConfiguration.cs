namespace Fanex.CustomerSetting.Matrix.WebApi.Configurations
{
    using System.Configuration;

    public class SettingMatrixApiConfiguration : ConfigurationSection
    {
        public static SettingMatrixApiConfiguration Settings { get; }
            = ConfigurationManager.GetSection("SettingMatrixApiConfiguration") as SettingMatrixApiConfiguration;

        [ConfigurationProperty("SecretKey", IsRequired = true, DefaultValue = "oOcEDP63hAXyWYjJ2DRbOYWLdx/eIaKZ5AWoiunpeMk=")]
        public string SecretKey
        {
            get { return (string)this["SecretKey"]; }
            set { this["SecretKey"] = value; }
        }

        [ConfigurationProperty("FanexRemotingRefService", IsRequired = false)]
        public string FanexRemotingRefService
        {
            get { return this["FanexRemotingRefService"] as string; }
            set { this["FanexRemotingRefService"] = value; }
        }

        [ConfigurationProperty("InstanceName", IsRequired = false, DefaultValue = "DCS")]
        public string InstanceName
        {
            get { return this["InstanceName"] as string; }
            set { this["InstanceName"] = value; }
        }
    }
}