namespace Fanex.CustomerSetting.Services
{
    using Models;

    public interface ISettingTypeServiceFactory
    {
        ISettingTypeService GetInstance(SettingType settingType);
    }
}