namespace Fanex.CustomerSetting.Models.PlugableTasks
{
    using Fanex.CustomerSetting.Models.Requests;

    public interface IPreQueryCustomerSettingTypeTask
    {
        void Invoke(ISettingContext settingContext, CustomerSettingQuery query);
    }
}