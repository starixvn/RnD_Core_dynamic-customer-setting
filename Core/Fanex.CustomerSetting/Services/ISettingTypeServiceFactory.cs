namespace Fanex.CustomerSetting.Services
{
    using Fanex.CustomerSetting.Models;

    public interface ISettingTypeServiceFactory
    {
        ISettingTypeService GetInstance(SettingType settingType);
    }
}