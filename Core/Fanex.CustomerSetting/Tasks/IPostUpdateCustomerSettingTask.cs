namespace Fanex.CustomerSetting.Tasks
{
    using Models;
    using Models.Requests;
    using Models.Results;

    public interface IPostUpdateCustomerSettingTask
    {
        void Invoke(ISettingContext settingContext, UpdateCustomerSettingCommand command, IResult result);
    }
}