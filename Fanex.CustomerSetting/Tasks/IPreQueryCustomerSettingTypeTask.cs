namespace Fanex.CustomerSetting.Tasks
{
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Models.Requests;

    public interface IPreQueryCustomerSettingTypeTask
    {
        void Invoke(ISettingContext settingContext, CustomerSettingQuery query);
    }
}