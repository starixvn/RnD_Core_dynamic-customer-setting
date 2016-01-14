namespace Fanex.CustomerSetting.Models.PlugableTasks
{
    using Fanex.CustomerSetting.Models.Requests;

    public interface IPreUpdateCustomerSettingTask
    {
        void Invoke(ISettingContext settingContext, UpdateCustomerSettingCommand command);
    }
}