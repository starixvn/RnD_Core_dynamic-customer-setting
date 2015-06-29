namespace Fanex.CustomerSetting.Services
{
    using Fanex.CustomerSetting.Dtos.Params;
    using Fanex.CustomerSetting.Dtos.Results;
    using Fanex.CustomerSetting.Models;

    public interface ISettingService
    {
        ISetting GetCustomerSetting(QuerySettingParam queryParam);

        IResult UpdateCustomerSetting(UpdatingSettingParam updatingParam);

        IResult UpdateMultipleCustomerSetting(UpdatingMultipleSettingParam updatingMultipleParam);
    }
}