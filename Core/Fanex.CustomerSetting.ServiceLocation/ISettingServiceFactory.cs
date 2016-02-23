namespace Fanex.CustomerSetting.ServiceLocation
{
    using Fanex.CustomerSetting.Services;
    using Models;

    public interface ISettingServiceFactory
    {
        IQuerySettingService GetQueryService(byte system, byte productId, ISettingContext settingContext);

        IUpdateSettingService GetUpdateService(byte system, byte productId, ISettingContext settingContext);
    }
}