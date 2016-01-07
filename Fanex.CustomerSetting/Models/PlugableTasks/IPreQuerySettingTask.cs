namespace Fanex.CustomerSetting.Models.PlugableTasks
{
    using Requests;

    public interface IPreQuerySettingTask
    {
        void Invoke(CustomerSettingQuery query);
    }
}