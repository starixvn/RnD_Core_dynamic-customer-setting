namespace Fanex.CustomerSetting.Tasks
{
    using Models;
    using Models.Requests;
    using Models.Results;

    public interface IPostUpdateMultipleCustomersSettingTask
    {
        void Invoke(ISettingContext settingContext, MultipleUpdateCustomerSettingCommand command, IResult result);
    }
}