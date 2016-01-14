namespace Fanex.CustomerSetting.Models.PlugableTasks
{
    using Fanex.CustomerSetting.Models.Requests;

    public interface IPreQueryCustomerSettingTask
    {
        void Invoke(ISettingContext settingContext, CustomerSettingQuery query);
    }
}