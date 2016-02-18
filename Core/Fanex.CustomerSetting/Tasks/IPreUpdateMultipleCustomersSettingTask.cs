namespace Fanex.CustomerSetting.Tasks
{
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Models.Requests;

    public interface IPreUpdateMultipleCustomersSettingTask
    {
        void Invoke(ISettingContext settingContext, MultipleUpdateCustomerSettingCommand command);
    }
}