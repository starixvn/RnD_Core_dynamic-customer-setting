namespace Fanex.CustomerSetting.ServiceLocation
{
    using System;
    using System.Collections.Concurrent;
    using System.Linq;
    using Common.Helper;

    public static class SettingServiceBootstraper
    {
        private static readonly Type QueryServiceDefaultType = Type.GetType("Fanex.CustomerSetting.Services.Impl.QuerySettingService, Fanex.CustomerSetting");
        private static readonly Type UpdateServiceDefaultType = Type.GetType("Fanex.CustomerSetting.Services.Impl.UpdateSettingService, Fanex.CustomerSetting");
        private static ConcurrentBag<SettingServiceConfiguration> settingConfiguration = new ConcurrentBag<SettingServiceConfiguration>();

        public static void RegisterService(SettingServiceConfiguration config)
        {
            if (IsConfigValid(config))
            {
                settingConfiguration.Add(config);
            }
        }

        internal static Type GetServiceType(byte system, byte productId, bool isQueryAction)
        {
            Check.Argument.IsNotNegativeOrZero(system, "system");
            Check.Argument.IsNotNegativeOrZero(productId, "productId");

            var config = settingConfiguration
                .Where(s => s.ProductId == productId)
                .FirstOrDefault(s => s.System == system || !s.System.HasValue);

            if (config == null)
            {
                return isQueryAction ? QueryServiceDefaultType : UpdateServiceDefaultType;
            }

            return isQueryAction ? config.QueryServiceType : config.UpdateServiceType;
        }

        private static bool IsConfigValid(SettingServiceConfiguration config)
        {
            return config != null
                && config.IsValid()
                && !IsConfigExisted(config);
        }

        private static bool IsConfigExisted(SettingServiceConfiguration config)
        {
            return settingConfiguration
                .Where(c => c.ProductId == config.ProductId)
                .Any(c => !c.System.HasValue || c.System.Value == config.System.Value);
        }

        internal static void Clear()
        {
            SettingServiceConfiguration result;
            while (settingConfiguration.TryTake(out result))
            {
            }
        }
    }
}