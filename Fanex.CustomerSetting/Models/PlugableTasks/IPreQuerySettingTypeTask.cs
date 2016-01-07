namespace Fanex.CustomerSetting.Models.PlugableTasks
{
    using Requests;

    public interface IPreQuerySettingTypeTask
    {
        void Invoke(ISettingContext settingContext, CustomerSettingQuery query);
    }
}