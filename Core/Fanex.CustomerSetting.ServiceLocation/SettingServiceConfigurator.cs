
namespace Fanex.CustomerSetting.ServiceLocation
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

        internal static Type GetServiceType(byte system, byte productId, bool isQueryAction)
        {
            Check.Argument.IsNotNegativeOrZero(system, "system");
            Check.Argument.IsNotNegativeOrZero(productId, "productId");            

            var config = configs
                .Where(s => s.ProductId == productId)
                .FirstOrDefault(s => s.System == system || !s.System.HasValue);

            if (config == null)
            {
                return isQueryAction ? DefaultQueryServiceType : DefaultUpdateServiceType;
            }

            return isQueryAction ? config.QueryServiceType : config.UpdateServiceType;
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

            var configItem = new SettingServiceConfigItem
            {
                QueryServiceType = queryServiceType,
                UpdateServiceType = updateServiceType
            };

            if (products.Any())
            {
                SetConfigForProductLevel(configItem);
            }
            else
            {
                SetConfigForSystemLevel(configItem);
            }

            ClearCurrentConfigContext();
        }

        public void Verify()
        {
            configs.ForEach(config =>
            {
                if (IsConfigValid(config))
                {
                    throw new InvalidOperationException("Something is wrong in setting service configuration");
                }
            });
        }

        public void Clear()
        {
            ClearCurrentConfigContext();
            configs.Clear();
        }

        private static bool IsConfigValid(SettingServiceConfigItem config)
        {
            return config != null
                && config.IsValid()
                && !IsConfigExisted(config);
        }

        private static bool IsConfigExisted(SettingServiceConfigItem config)
        {
            return configs
                .Where(c => c.ProductId == config.ProductId)
                .Any(c => !c.System.HasValue || c.System.Value == config.System.Value);
        }

        private void ClearCurrentConfigContext()
        {
            this.products.Clear();
            this.systems.Clear();
            this.updateServiceType = null;
            this.queryServiceType = null;
        }

        private void SetConfigForProductLevel(SettingServiceConfigItem configItem)
        {
            products.ForEach(productId =>
            {
                var configOnProduct = (SettingServiceConfigItem)configItem.Clone();
                configOnProduct.ProductId = productId;
                SetConfigForSystemLevel(configOnProduct);

            });
        }

        private void SetConfigForSystemLevel(SettingServiceConfigItem configItem)
        {
            if (systems.Any())
            {
                systems.ForEach(system =>
                {
                    var configOnSystem = (SettingServiceConfigItem)configItem.Clone();
                    configOnSystem.System = system;

                    configs.Add(configOnSystem);
                });
            }
            else
            {
                configs.Add(configItem);
            }
        }
    }
}
