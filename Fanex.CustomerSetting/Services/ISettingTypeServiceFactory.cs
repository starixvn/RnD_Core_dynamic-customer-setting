namespace Fanex.CustomerSetting.Handlers
{
    using Fanex.CustomerSetting.DataAccesses;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Services;

    public interface ISettingTypeServiceFactory
    {
        ISettingTypeService CreateInstance(SettingType settingType);
    }
}