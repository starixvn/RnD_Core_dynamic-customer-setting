namespace Fanex.CustomerSetting.Tasks
{
    using Fanex.CustomerSetting.Models.Requests;
    using Models;
    public interface IPreUpdateMultipleCustomersSettingTask
    {
        void Invoke(ISettingContext settingContext, MultipleUpdateCustomerSettingCommand command);
    }
}