namespace Fanex.CustomerSetting.Tasks
{
    using Models;
    using Models.Requests;

    public interface IPostQueryCustomerSettingTask
    {
        void Invoke(ISettingContext settingContext, CustomerSettingQuery query, ISetting setting);
    }
}