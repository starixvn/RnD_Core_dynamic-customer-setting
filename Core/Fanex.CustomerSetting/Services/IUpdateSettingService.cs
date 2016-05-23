namespace Fanex.CustomerSetting.Services
{
    using Models.Requests;
    using Models.Results;

    public interface IUpdateSettingService : ISettingService
    {
        IResult Update(UpdateCustomerSettingCommand command);

        IResult Update(MultipleUpdateCustomerSettingCommand command);
    }
}