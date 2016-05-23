namespace Fanex.CustomerSetting.Tasks
{
    using Models;
    using Models.Requests;

    public interface IPostQueryCustomerSettingTypeTask
    {
        void Invoke(ISettingContext settingContext, CustomerSettingQuery query, SettingType settingTypeData);
    }
}