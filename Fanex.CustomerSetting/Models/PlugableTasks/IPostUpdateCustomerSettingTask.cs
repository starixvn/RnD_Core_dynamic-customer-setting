namespace Fanex.CustomerSetting.Models.PlugableTasks
{
    using Fanex.CustomerSetting.Models.Requests;
    using Fanex.CustomerSetting.Models.Results;

    public interface IPostUpdateCustomerSettingTask
    {
        void Invoke(ISettingContext settingContext, UpdateCustomerSettingCommand command, IResult result);
    }
}