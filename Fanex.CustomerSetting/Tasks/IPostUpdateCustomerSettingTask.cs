namespace Fanex.CustomerSetting.Tasks
{
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Models.Requests;
    using Fanex.CustomerSetting.Models.Results;

    public interface IPostUpdateCustomerSettingTask
    {
        void Invoke(ISettingContext settingContext, UpdateCustomerSettingCommand command, IResult result);
    }
}