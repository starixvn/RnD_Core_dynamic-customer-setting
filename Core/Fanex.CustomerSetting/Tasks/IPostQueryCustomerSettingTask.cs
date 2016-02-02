namespace Fanex.CustomerSetting.Tasks
{
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Models.Requests;
    public interface IPostQueryCustomerSettingTask
    {
        void Invoke(ISettingContext settingContext, CustomerSettingQuery query, ISetting setting);
    }
}