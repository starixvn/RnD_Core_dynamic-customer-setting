namespace Fanex.CustomerSetting.Tasks
{
    using Models;
    using Models.Requests;

    public interface IPreUpdateCustomerSettingTask
    {
        void Invoke(ISettingContext settingContext, UpdateCustomerSettingCommand command);
    }
}