namespace Fanex.CustomerSetting.Services.Impl
{
    using System;
    using Common.Helper;
    using Configurations;
    using Models;

    public class SettingServiceFactory : ISettingServiceFactory
    {
        public IQuerySettingService CreateQueryService(byte system, byte productId, ISettingContext settingContext)
        {
            CheckValidInputParam(system, productId, settingContext);

            Type type = SettingServiceConfigurator.GetServiceType(system, productId, isQueryAction: true);
            return Activator.CreateInstance(type, settingContext) as IQuerySettingService;
        }

        public IUpdateSettingService CreateUpdateService(byte system, byte productId, ISettingContext settingContext)
        {
            CheckValidInputParam(system, productId, settingContext);

            Type type = SettingServiceConfigurator.GetServiceType(system, productId, isQueryAction: false);
            return Activator.CreateInstance(type, settingContext) as IUpdateSettingService;
        }

        private static void CheckValidInputParam(byte system, byte productId, ISettingContext settingContext)
        {
            Check.Argument.IsNotNegativeOrZero(system, "system");
            Check.Argument.IsNotNegativeOrZero(productId, "productId");
            Check.Argument.IsNotNull(settingContext, "settingContext");
        }
    }
}