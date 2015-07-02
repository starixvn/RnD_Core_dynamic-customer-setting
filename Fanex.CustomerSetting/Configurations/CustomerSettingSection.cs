namespace Fanex.CustomerSetting.Configurations
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;

    public class CustomerSettingSection : ConfigurationSection
    {
        private const string CustomerSettingsName = "Fanex.CustomerSetting";
        private const string SystemIdName = "SystemId";
        private const string CustomValidationServiceTypeName = "CustomValidationServiceType";
        private const string SettingTypeCollectionName = "SettingTypes";
        private const string ProductNotUsesDefaultSPCacheKeyFormat = "{0}_{1}_{2}";
        private static CustomerSettingSection _settings = ConfigurationManager.GetSection(CustomerSettingsName) as CustomerSettingSection;
        private List<string> _productNotUsesDefaultSPCached = new List<string>();

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

        [ConfigurationProperty(CustomValidationServiceTypeName, IsRequired = false)]
        public string CustomValidationServiceType
        {
            get { return (string)this[CustomValidationServiceType]; }
            set { this[CustomValidationServiceType] = value; }
        }

        [ConfigurationProperty(SettingTypeCollectionName, IsDefaultCollection = false, IsRequired = false)]
        [ConfigurationCollection(typeof(SettingTypeCollection),
            AddItemName = "add")]
        public SettingTypeCollection SettingTypes
        {
            get
            {
                var settingTypeCollection = (SettingTypeCollection)base[SettingTypeCollectionName];

                return settingTypeCollection;
            }
        }

        public bool IsProductDoesNotUseDefaultStoredProcedures(byte settingTypeId, byte settingTemplateId, byte productId)
        {
            if (SettingTypes == null || SettingTypes.Count == 0)
            {
                return true;
            }

            if (!_productNotUsesDefaultSPCached.Any())
            {
                CachedProductNotUseStoredProceduresDefault();
            }

            var cacheKeyFormatted = string.Format(ProductNotUsesDefaultSPCacheKeyFormat, settingTypeId, settingTemplateId, productId);
            return _productNotUsesDefaultSPCached.Contains(cacheKeyFormatted);
        }

        private void CachedProductNotUseStoredProceduresDefault()
        {
            foreach (SettingTypeElement settingType in SettingTypes)
            {
                if (!string.IsNullOrEmpty(settingType.ProductIds))
                {
                    string[] productIds = settingType.ProductIds.Split(',');
                    foreach (string productId in productIds)
                    {
                        _productNotUsesDefaultSPCached.Add(
                            string.Format(
                                ProductNotUsesDefaultSPCacheKeyFormat,
                                settingType.SettingTypeId,
                                settingType.TemplateId,
                                productId));
                    }
                }
            }
        }
    }
}