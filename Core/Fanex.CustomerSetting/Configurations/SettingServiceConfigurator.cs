namespace Fanex.CustomerSetting.Configurations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common.Helper;
    using Services.Impl;

    public class SettingServiceConfigurator
    {
        private static readonly Type DefaultQueryServiceType = typeof(QuerySettingService);
        private static readonly Type DefaultUpdateServiceType = typeof(UpdateSettingService);
        private static List<SettingServiceConfigItem> configs = new List<SettingServiceConfigItem>();
        private List<byte> systems = new List<byte>();
        private List<byte> products = new List<byte>();
        private Type queryServiceType;
        private Type updateServiceType;

        public SettingServiceConfigurator()
        {
            Clear();
        }

        internal static Type GetServiceType(byte system, byte productId, bool isQueryAction)
        {
            Check.Argument.IsNotNegativeOrZero(system, "system");
            Check.Argument.IsNotNegativeOrZero(productId, "productId");

            var settingTypeServiceConfig = configs                
                .Where(configItem => configItem.ProductIds.Contains(productId) || !configItem.ProductIds.Any())
                .FirstOrDefault(configItem => configItem.Systems.Contains(system) || !configItem.Systems.Any());

            if (settingTypeServiceConfig == null)
            {
                return isQueryAction ? DefaultQueryServiceType : DefaultUpdateServiceType;
            }

            return isQueryAction ? settingTypeServiceConfig.QueryServiceType : settingTypeServiceConfig.UpdateServiceType;
        }

        public SettingServiceConfigurator ForSystems(params byte[] systems)
        {
            this.systems.AddRange(systems);

            return this;
        }

        public SettingServiceConfigurator ForAllSystems()
        {
            systems.Clear();

            return this;
        }

        public SettingServiceConfigurator ForProducts(params byte[] products)
        {
            this.products.AddRange(products);

            return this;
        }

        public SettingServiceConfigurator ForAllProducts()
        {
            products.Clear();

            return this;
        }

        public SettingServiceConfigurator UseQueryServiceType(Type queryServiceType)
        {
            if (ReferenceEquals(queryServiceType, null))
            {
                throw new ArgumentNullException("queryServiceType");
            }

            this.queryServiceType = queryServiceType;

            return this;
        }

        public SettingServiceConfigurator UseUpdateServiceType(Type updateServiceType)
        {
            if (ReferenceEquals(updateServiceType, null))
            {
                throw new ArgumentNullException("updateServiceType");
            }

            this.updateServiceType = updateServiceType;

            return this;
        }

        public void Apply()
        {
            Check.Argument.IsNotNull(queryServiceType, "queryServiceType");
            Check.Argument.IsNotNull(updateServiceType, "updateServiceType");

            var configItem = new SettingServiceConfigItem(
                queryServiceType,
                updateServiceType,
                systems,
                products);

            configs.Add(configItem);

            ResetConfigContext();
        }

        public void Verify()
        {
            configs.ForEach(config =>
            {
                if (!IsConfigValid(config))
                {
                    throw new InvalidOperationException("Something is wrong in setting service configuration");
                }
            });
        }

        public void Clear()
        {
            ResetConfigContext();
            configs.Clear();
        }

        private static bool IsConfigValid(SettingServiceConfigItem config)
        {
            return config != null
                && config.IsValid();                
        }

        private void ResetConfigContext()
        {
            products.Clear();
            systems.Clear();
            updateServiceType = null;
            queryServiceType = null;
        }
    }
}
