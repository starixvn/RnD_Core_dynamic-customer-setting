namespace Fanex.CustomerSetting.ServiceLocation
{
    using Fanex.CustomerSetting.Services;
    using Models;

    public interface ISettingServiceLocator
    {
        ISettingService GetService(int productId, bool isQueryAction, ISettingContext settingContext);
    }
}
