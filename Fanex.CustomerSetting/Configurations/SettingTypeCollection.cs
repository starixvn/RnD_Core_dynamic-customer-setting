namespace Fanex.CustomerSetting.Configurations
{
    using System.Configuration;
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("Microsoft.Design", "CA1010:CollectionsShouldImplementGenericInterface", Justification = "Reviewed !")]
    public class SettingTypeCollection : ConfigurationElementCollection
    {
        public SettingTypeCollection()
        {
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }

        public SettingTypeElement this[int index]
        {
            get
            {
                return (SettingTypeElement)BaseGet(index);
            }

            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }

                BaseAdd(index, value);
            }
        }

        public void Add(SettingTypeElement mappingElement)
        {
            BaseAdd(mappingElement);
        }

        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new SettingTypeElement();
        }

        /// <summary>
        /// Get Element Key
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            var settingTypeElement = element as SettingTypeElement;
            return string.Format("{0}_{1}", settingTypeElement.SettingTypeId, settingTypeElement.TemplateId);
        }
    }
}