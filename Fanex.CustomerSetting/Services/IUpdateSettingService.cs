namespace Fanex.CustomerSetting.Services
{
    using Fanex.CustomerSetting.Dtos.Params;
    using Fanex.CustomerSetting.Dtos.Results;
    using Fanex.CustomerSetting.Models;

    public interface IUpdateSettingService
    {
        IResult UpdateCustomerSetting(UpdateSettingParam updateParam);

        IResult UpdateMultipleCustomerSetting(UpdateMultipleSettingParam updateMultipleParam);
    }
}