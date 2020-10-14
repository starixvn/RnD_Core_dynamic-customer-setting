namespace Fanex.CustomerSetting.WebApi.Configurations
{
    using System.Configuration;

    public class CustomerSettingApiConfiguration : ConfigurationSection
    {
        public static CustomerSettingApiConfiguration Settings { get; }
            = ConfigurationManager.GetSection("CustomerSettingApiConfiguration") as CustomerSettingApiConfiguration;

        [ConfigurationProperty("SecretKey", IsRequired = true, DefaultValue = "U6BUFHtVcq5SPQcTl7tHLBG/LF6d5YVRKHrCHD9M+Vw=")]
        public string SecretKey
        {
            get { return this["SecretKey"] as string; }
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

        [ConfigurationProperty("SettingStatus", IsRequired = false, DefaultValue = (byte)0)]
        public byte SettingStatus
        {
            get { return (byte)this["SettingStatus"]; }
            set { this["SettingStatus"] = value; }
        }
    }
}