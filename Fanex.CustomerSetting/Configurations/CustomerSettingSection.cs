namespace Fanex.CustomerSetting.Configurations
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;

    public class CustomerSettingSection : ConfigurationSection
    {
        private const string CustomerSettingsName = "Fanex.CustomerSetting";
        private const string SystemIdName = "SystemId";
        private static CustomerSettingSection _settings = ConfigurationManager.GetSection(CustomerSettingsName) as CustomerSettingSection;

        public static CustomerSettingSection Settings
        {
            get
            {
                return _settings;
            }
        }

        [ConfigurationProperty(SystemIdName, IsRequired = true)]
        public byte SystemId
        {
            get { return (byte)this[SystemIdName]; }
            set { this[SystemIdName] = value; }
        }
    }
}