namespace Fanex.CustomerSetting.ServiceLocation
{
    using System;
    using Fanex.CustomerSetting.Services;
    using Models;

    public class SettingServiceFactory : ISettingServiceFactory
    {
        public IQuerySettingService GetQueryService(byte system, byte productId, ISettingContext settingContext)
        {
            Type type = SettingServiceConfigurator.GetServiceType(system, productId, isQueryAction: true);
            return Activator.CreateInstance(type, settingContext) as IQuerySettingService;
        }

        public IUpdateSettingService GetUpdateService(byte system, byte productId, ISettingContext settingContext)
        {
            Type type = SettingServiceConfigurator.GetServiceType(system, productId, isQueryAction: false);
            return Activator.CreateInstance(type, settingContext) as IUpdateSettingService;
        }
    }
}