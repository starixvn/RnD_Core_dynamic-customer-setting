namespace Fanex.CustomerSetting.Models.PlugableTasks
{
    using Requests;
    using Results;

    public interface IPostUpdateSettingTask
    {
        void Invoke(ISettingContext settingContext, UpdateCustomerSettingCommand command, IResult result);
    }
}