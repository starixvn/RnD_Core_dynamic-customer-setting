namespace Fanex.CustomerSetting.Configurations
{
    using System.Configuration;

    public class CustomerSettings : ConfigurationSection
    {
        private const string CustomerSettingsName = "CustomerSettings";
        private const string SystemIdName = "SystemId";
        private const string DomainName = "DomainName";
        private const string CustomValidationServiceTypeName = "CustomValidationServiceType";

        private static CustomerSettings _settings = ConfigurationManager.GetSection(CustomerSettingsName) as CustomerSettings;

        public static CustomerSettings Settings
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

        [ConfigurationProperty(DomainName, IsRequired = true)]
        public string Domain
        {
            get { return (string)this[DomainName]; }
            set { this[DomainName] = value; }
        }

        [ConfigurationProperty(CustomValidationServiceTypeName, IsRequired = false)]
        public string CustomValidationServiceType
        {
            get { return (string)this[CustomValidationServiceType]; }
            set { this[CustomValidationServiceType] = value; }
        }
    }
}