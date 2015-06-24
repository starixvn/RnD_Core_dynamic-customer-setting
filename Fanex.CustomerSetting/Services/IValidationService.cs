namespace Fanex.CustomerSetting.Services
{
    using Fanex.CustomerSetting.Dtos.Params;
    using Fanex.CustomerSetting.Dtos.Results;

    public interface IValidationService
    {
        IResult ValidateQuerySetting(QuerySettingParams querySettingParams);

        IResult ValidateUpdatingSetting(UpdatingSettingParams updatingParams);

        IResult ValidateUpdatingMultipleSetting(UpdateMultipleSettingParams updatingMultipleParams);
    }
}