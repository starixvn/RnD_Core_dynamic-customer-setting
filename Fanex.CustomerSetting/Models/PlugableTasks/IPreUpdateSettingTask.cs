namespace Fanex.CustomerSetting.Models.PlugableTasks
{
    using Requests;

    public interface IPreUpdateSettingTask
    {
        void Invoke(ISettingContext settingContext, UpdateCustomerSettingCommand command);
    }
}