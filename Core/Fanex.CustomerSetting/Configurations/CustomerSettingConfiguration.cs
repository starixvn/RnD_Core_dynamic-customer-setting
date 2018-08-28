namespace Fanex.CustomerSetting.Configurations
{
    using System.Configuration;

    internal class CustomerSettingConfiguration : ConfigurationSection
    {
        private const string TasksConfigurationPathKey = "TasksConfigurationPath";

        private static readonly CustomerSettingConfiguration configurations
            = ConfigurationManager.GetSection("CustomerSettingConfiguration") as CustomerSettingConfiguration;

        public static CustomerSettingConfiguration Configurations => configurations;

        [ConfigurationProperty(TasksConfigurationPathKey)]
        public string TasksConfigurationPath
        {
            get { return (string)this[TasksConfigurationPathKey]; }
            set { this[TasksConfigurationPathKey] = value; }
        }
    }
}