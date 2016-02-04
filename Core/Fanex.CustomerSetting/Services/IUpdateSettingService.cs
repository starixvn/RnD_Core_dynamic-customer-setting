namespace Fanex.CustomerSetting.Services
{
    using Fanex.CustomerSetting.Models.Requests;
    using Fanex.CustomerSetting.Models.Results;

    public interface IUpdateSettingService : ISettingService
    {
        IResult Update(UpdateCustomerSettingCommand command);

        IResult Update(MultipleUpdateCustomerSettingCommand command);
    }
}