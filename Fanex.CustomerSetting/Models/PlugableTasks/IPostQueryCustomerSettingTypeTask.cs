namespace Fanex.CustomerSetting.Models.PlugableTasks
{
    using Fanex.CustomerSetting.Models.Requests;

    public interface IPostQueryCustomerSettingTypeTask
    {
        void Invoke(ISettingContext settingContext, CustomerSettingQuery query, SettingType settingTypeData);
    }
}