namespace Fanex.CustomerSetting.Models.PlugableTasks
{
    using Requests;

    public interface IPreQuerySettingTask
    {
        void Invoke(ISettingContext settingContext, CustomerSettingQuery query);
    }
}