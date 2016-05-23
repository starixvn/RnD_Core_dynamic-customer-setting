namespace Fanex.CustomerSetting.Tasks
{
    using Models;
    using Models.Requests;

    public interface IPreUpdateMultipleCustomersSettingTask
    {
        void Invoke(ISettingContext settingContext, MultipleUpdateCustomerSettingCommand command);
    }
}