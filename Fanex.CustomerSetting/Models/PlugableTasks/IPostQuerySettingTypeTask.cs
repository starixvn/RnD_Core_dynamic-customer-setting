namespace Fanex.CustomerSetting.Models.PlugableTasks
{
    using Requests;

    public interface IPostQuerySettingTypeTask
    {
        void Invoke(ISettingContext settingContext, CustomerSettingQuery query, SettingType settingTypeData);
    }
}