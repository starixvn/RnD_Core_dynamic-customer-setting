namespace Fanex.CustomerSetting.ServiceLocation
{
    using System;
    using System.Linq;
    using System.Collections.Concurrent;
    using Common.Helper;
    using System.Collections;
    using System.Collections.Generic;
    public static class SettingServiceBootstraper
    {
        private static ConcurrentBag<SettingServiceConfiguration> SettingConfiguration = new ConcurrentBag<SettingServiceConfiguration>();
        private static readonly Type QueryServiceDefaultType = Type.GetType("Fanex.CustomerSetting.Services.Impl.QuerySettingService, Fanex.CustomerSetting");
        private static readonly Type UpdateServiceDefaultType = Type.GetType("Fanex.CustomerSetting.Services.Impl.UpdateSettingService, Fanex.CustomerSetting");

        public static void RegisterService(SettingServiceConfiguration config)
        {
            if (IsConfigValid(config))
            {
                SettingConfiguration.Add(config);
            }
        }

        internal static Type GetServiceType(byte system, byte productId, bool isQueryAction)
        {
            Check.Argument.IsNotNegativeOrZero(system, "system");
            Check.Argument.IsNotNegativeOrZero(productId, "productId");

            var config = SettingConfiguration
                .Where(s => s.ProductId == productId)
                .FirstOrDefault(s => s.System == system || !s.System.HasValue);

            if (config == null)
            {
                return (isQueryAction) ? QueryServiceDefaultType : UpdateServiceDefaultType;
            }

            return (isQueryAction) ? config.QueryServiceType : config.UpdateServiceType;
        }

        private static bool IsConfigValid(SettingServiceConfiguration config)
        {
            return (config != null
                && config.IsValid()
                && !IsConfigExisted(config));
        }

        private static bool IsConfigExisted(SettingServiceConfiguration config)
        {
            return SettingConfiguration
                .Where(c => c.ProductId == config.ProductId)
                .Any(c => !c.System.HasValue || c.System.Value == config.System.Value);
        }

        internal static void Clear()
        {
            SettingServiceConfiguration result;
            while (SettingConfiguration.TryTake(out result)) ;
        }
    }
}
