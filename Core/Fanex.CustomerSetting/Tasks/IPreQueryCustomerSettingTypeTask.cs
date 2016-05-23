namespace Fanex.CustomerSetting.Tasks
{
    using Models;
    using Models.Requests;

    public interface IPreQueryCustomerSettingTypeTask
    {
        void Invoke(ISettingContext settingContext, CustomerSettingQuery query);
    }
}