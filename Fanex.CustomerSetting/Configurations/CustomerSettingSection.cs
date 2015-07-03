namespace Fanex.CustomerSetting.Configurations
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;

    public class CustomerSettingSection : ConfigurationSection
    {
        private const string CustomerSettingsName = "Fanex.CustomerSetting";
        private const string SystemIdName = "SystemId";
        private const string SettingTypeCollectionName = "SettingTypes";
        private const string ProductDoesNotUsesDefaultSPCacheKeyFormat = "{0}_{1}_{2}";
        private static CustomerSettingSection _settings = ConfigurationManager.GetSection(CustomerSettingsName) as CustomerSettingSection;
        private List<string> _productsDontUseDefaultSPsCached = new List<string>();

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

        public bool IsProductNotUseDefaultStoredProcedures(short settingTypeId, byte settingTemplateId, byte productId)
        {
            if (SettingTypes == null || SettingTypes.Count == 0)
            {
                return false;
            }

            if (!_productsDontUseDefaultSPsCached.Any())
            {
                CachedProductNotUseDefaultStoredProcedures();
            }

            var cacheKeyFormatted = string.Format(ProductDoesNotUsesDefaultSPCacheKeyFormat, settingTypeId, settingTemplateId, productId);
            return _productsDontUseDefaultSPsCached.Contains(cacheKeyFormatted);
        }

        private void CachedProductNotUseDefaultStoredProcedures()
        {
            foreach (SettingTypeElement settingType in SettingTypes)
            {
                if (!string.IsNullOrEmpty(settingType.ProductIdsNotUseDefaultStoreProcedures))
                {
                    string[] productIds = settingType.ProductIdsNotUseDefaultStoreProcedures.Split(',');
                    foreach (string productId in productIds)
                    {
                        _productsDontUseDefaultSPsCached.Add(
                            string.Format(
                                ProductDoesNotUsesDefaultSPCacheKeyFormat,
                                settingType.SettingTypeId,
                                settingType.TemplateId,
                                productId));
                    }
                }
            }
        }
    }
}