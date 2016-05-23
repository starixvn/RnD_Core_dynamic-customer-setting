namespace Fanex.CustomerSetting.Casino.Services
{
    using CustomerSetting.Models.Requests;
    using CustomerSetting.Models.Results;
    using CustomerSetting.Services;

    public interface IUpdateCasinoSettingService : IUpdateSettingService
    {
        IResult UpdateUnsyncedCustomerSetting(UpdateCustomerSettingCommand command);
    }
}