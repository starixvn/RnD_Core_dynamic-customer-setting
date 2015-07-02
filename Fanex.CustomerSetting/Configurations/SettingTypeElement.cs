namespace Fanex.CustomerSetting.Configurations
{
    using System.Configuration;

    public class SettingTypeElement : ConfigurationElement
    {
        private const string SettingTypeIdAttName = "id";
        private const string SettingTemplateIdAttName = "template";
        private const string ProductIdsAttName = "productIds";

        [ConfigurationProperty(SettingTypeIdAttName, IsRequired = true, IsKey = true)]
        public int SettingTypeId
        {
            get
            {
                return (int)this[SettingTypeIdAttName];
            }

            set
            {
                this[SettingTypeIdAttName] = value;
            }
        }

        [ConfigurationProperty(SettingTemplateIdAttName, IsRequired = false, IsKey = true)]
        public int TemplateId
        {
            get
            {
                return (int)this[SettingTemplateIdAttName];
            }

            set
            {
                this[SettingTemplateIdAttName] = value;
            }
        }

        [ConfigurationProperty(ProductIdsAttName, IsRequired = false, IsKey = true)]
        public string ProductIds
        {
            get
            {
                return (string)this[ProductIdsAttName];
            }

            set
            {
                this[ProductIdsAttName] = value;
            }
        }
    }
}