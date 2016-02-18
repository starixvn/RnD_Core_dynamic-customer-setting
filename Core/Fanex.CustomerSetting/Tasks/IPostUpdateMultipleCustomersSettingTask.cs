namespace Fanex.CustomerSetting.Tasks
{
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Models.Requests;
    using Fanex.CustomerSetting.Models.Results;

    public interface IPostUpdateMultipleCustomersSettingTask
    {
        void Invoke(ISettingContext settingContext, MultipleUpdateCustomerSettingCommand command, IResult result);
    }
}