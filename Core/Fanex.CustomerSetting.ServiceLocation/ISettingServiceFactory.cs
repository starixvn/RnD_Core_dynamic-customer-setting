namespace Fanex.CustomerSetting.ServiceLocation
{
    using Fanex.CustomerSetting.Services;
    using Models;

    public interface ISettingServiceFactory
    {
        IQuerySettingService CreateQueryService(byte system, byte productId, ISettingContext settingContext);

        IUpdateSettingService CreateUpdateService(byte system, byte productId, ISettingContext settingContext);
    }
}