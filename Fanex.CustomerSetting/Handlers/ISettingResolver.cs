namespace Fanex.CustomerSetting.Handlers
{
    using Fanex.CustomerSetting.DataAccesses;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Services;

    public interface ISettingResolver
    {
        ISettingTypeService ResolveSettingTypeServce(SettingType settingType);
    }
}