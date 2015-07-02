namespace Fanex.CustomerSetting.Configurations
{
    using System.Configuration;

    public class SettingTypeElement : ConfigurationElement
    {
        private const string SettingTypeIdAttName = "id";
        private const string SettingTemplateIdAttName = "template";
        private const string ProductIdsNotUseDefaultStoreProceduresAttName = "productIdsNotUseDefaultStoreProcedures";

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

        [ConfigurationProperty(ProductIdsNotUseDefaultStoreProceduresAttName, IsRequired = false, IsKey = true)]
        public string ProductIdsNotUseDefaultStoreProcedures
        {
            get
            {
                return (string)this[ProductIdsNotUseDefaultStoreProceduresAttName];
            }

            set
            {
                this[ProductIdsNotUseDefaultStoreProceduresAttName] = value;
            }
        }
    }
}