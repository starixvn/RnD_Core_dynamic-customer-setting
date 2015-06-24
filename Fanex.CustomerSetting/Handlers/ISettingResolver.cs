namespace Fanex.CustomerSetting.Handlers
{
    using Fanex.CustomerSetting.DataAccesses;
    using Fanex.CustomerSetting.Models;

    public interface ISettingResolver
    {
        ISettingDataAccess ResolveSettingDataAccess(SettingType settingType);

        SettingPreTasks ResolveSettingPreTasks(SettingType settingType);

        SettingPostTasks ResolveSettingPostTasks(SettingType settingType);
    }
}