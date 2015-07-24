namespace Fanex.CustomerSetting.Services
{
    using Fanex.CustomerSetting.Requests;
    using Fanex.CustomerSetting.Results;

    public interface IUpdateSettingService
    {
        IResult Update(UpdateCustomerSettingCommand command);

        IResult Update(MultipleUpdateCustomerSettingCommand command);
    }
}