namespace Fanex.CustomerSetting.Models.PlugableTasks
{
    using Fanex.CustomerSetting.Models.Requests;

    public interface IPreUpdateMultipleCustomersSettingTask
    {
        void Invoke(ISettingContext settingContext, MultipleUpdateCustomerSettingCommand command);
    }
}