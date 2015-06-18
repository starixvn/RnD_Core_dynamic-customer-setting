namespace Fanex.CustomerSetting.SettingTypes
{
    using Fanex.CustomerSetting.Enumerations;

    public interface ISettingHandlerFactory
    {
        ISettingHandler Create(SettingType settingType);
    }
}