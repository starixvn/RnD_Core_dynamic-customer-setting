namespace Fanex.CustomerSetting.ServiceLocation
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using Casino.Services;
    using Fanex.CustomerSetting.Services;
    using Fanex.CustomerSetting.Services.Impl;
    using Models;
    using static Products;

    public class SettingServiceLocator : ISettingServiceLocator
    {
        public ISettingService GetService(int productId, bool isQueryAction, ISettingContext settingContext)
        {
            Type type = SettingServiceBootstraper.GetServiceType(productId, isQueryAction);

            return Activator.CreateInstance(type, settingContext) as ISettingService;
        }
    }
}
