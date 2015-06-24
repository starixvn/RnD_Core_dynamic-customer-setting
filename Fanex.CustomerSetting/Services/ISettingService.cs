namespace Fanex.CustomerSetting.Services
{
    using Fanex.CustomerSetting.Dtos.Params;
    using Fanex.CustomerSetting.Dtos.Results;
    using Fanex.CustomerSetting.Models;

    public interface ISettingService
    {
        ISetting GetCustomerSetting(QuerySettingParams querySettingParams);

        IResult UpdateCustomerSetting(UpdatingSettingParams updatingParams);

        IResult UpdateMultipleCustomerSetting(UpdateMultipleSettingParams updatingMultipleSettingParams);
    }
}