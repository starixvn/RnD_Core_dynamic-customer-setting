namespace Fanex.CustomerSetting.ServiceLocation
{
    using System;
    using System.Collections.Concurrent;
    using Common.Helper;

    public static class SettingServiceBootstraper
    {
        private static ConcurrentDictionary<int, Type[]> SettingConfiguration = new ConcurrentDictionary<int, Type[]>();

        public static void RegisterService(int productId, Type queryServiceType, Type updateServiceType)
        {
            Check.Argument.IsNotNegativeOrZero(productId, "productId");
            Check.Argument.IsNotNull(queryServiceType, "queryServiceType");
            Check.Argument.IsNotNull(updateServiceType, "updateServiceType");

            SettingConfiguration.TryAdd(productId, new Type[] { queryServiceType, updateServiceType });
        }

        internal static Type GetServiceType(int productId, bool isQueryAction)
        {
            if (!SettingConfiguration.ContainsKey(productId))
            {
                throw new ArgumentOutOfRangeException($"Cannot found configuration for Product Id {productId}");
            }

            var config = SettingConfiguration[productId];

            if (config == null)
            {
                throw new Exception($"Cannot find setting config for product id: {productId}");
            }

            return (isQueryAction) ? config[0] : config[1];
        }
    }
}
