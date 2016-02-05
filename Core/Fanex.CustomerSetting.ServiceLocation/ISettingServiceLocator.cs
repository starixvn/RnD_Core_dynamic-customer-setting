namespace Fanex.CustomerSetting.ServiceLocation
{
    using Fanex.CustomerSetting.Services;
    using Models;

    public interface ISettingServiceLocator
    {
        ISettingService GetService(byte system, byte productId, bool isQueryAction, ISettingContext settingContext);
    }
}
