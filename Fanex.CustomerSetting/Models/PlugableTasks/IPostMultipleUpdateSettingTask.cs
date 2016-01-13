namespace Fanex.CustomerSetting.Models.PlugableTasks
{
    using Requests;
    using Results;

    public interface IPostMultipleUpdateSettingTask
    {
        void Invoke(ISettingContext settingContext, MultipleUpdateCustomerSettingCommand command, IResult result);
    }
}