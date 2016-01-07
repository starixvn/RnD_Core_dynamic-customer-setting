namespace Fanex.CustomerSetting.Models.PlugableTasks
{
    using Requests;
    using Results;

    public interface IPostUpdateSettingTask
    {
        void Invoke(UpdateCustomerSettingCommand command, ref IResult result);
    }
}