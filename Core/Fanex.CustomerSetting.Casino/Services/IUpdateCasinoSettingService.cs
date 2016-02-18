namespace Fanex.CustomerSetting.Casino.Services
{
    using CustomerSetting.Models.Requests;
    using Fanex.CustomerSetting.Models.Results;
    using Fanex.CustomerSetting.Services;

    public interface IUpdateCasinoSettingService : IUpdateSettingService
    {
        IResult UpdateUnsyncedCustomerSetting(UpdateCustomerSettingCommand command);
    }
}