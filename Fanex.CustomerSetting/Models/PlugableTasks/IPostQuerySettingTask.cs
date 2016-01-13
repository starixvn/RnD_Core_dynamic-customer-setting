namespace Fanex.CustomerSetting.Models.PlugableTasks
{
    using Requests;

    public interface IPostQuerySettingTask
    {
        void Invoke(ISettingContext settingContext, CustomerSettingQuery query, Setting setting);
    }
}