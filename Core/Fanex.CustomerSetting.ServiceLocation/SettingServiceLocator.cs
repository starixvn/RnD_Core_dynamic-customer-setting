namespace Fanex.CustomerSetting.ServiceLocation
{
    using System;
    using Fanex.CustomerSetting.Services;
    using Models;

    public class SettingServiceLocator : ISettingServiceLocator
    {
        public ISettingService GetService(byte system, byte productId, bool isQueryAction, ISettingContext settingContext)
        {
            Type type = SettingServiceBootstraper.GetServiceType(system, productId, isQueryAction);

            return Activator.CreateInstance(type, settingContext) as ISettingService;
        }
    }
}
