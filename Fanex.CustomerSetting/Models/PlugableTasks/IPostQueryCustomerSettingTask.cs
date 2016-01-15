namespace Fanex.CustomerSetting.Models.PlugableTasks
{
    using Fanex.CustomerSetting.Models.Requests;

    public interface IPostQueryCustomerSettingTask
    {
        void Invoke(ISettingContext settingContext, CustomerSettingQuery query, ISetting setting);
    }
}