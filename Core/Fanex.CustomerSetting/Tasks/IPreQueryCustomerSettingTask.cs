namespace Fanex.CustomerSetting.Tasks
{
    using Models;
    using Models.Requests;

    public interface IPreQueryCustomerSettingTask
    {
        void Invoke(ISettingContext settingContext, CustomerSettingQuery query);
    }
}