namespace Fanex.CustomerSetting.Models.PlugableTasks
{
    using Requests;

    public interface IPreMultipleUpdateSettingTask
    {
        void Invoke(ISettingContext settingContext, MultipleUpdateCustomerSettingCommand command);
    }
}