namespace Fanex.CustomerSetting.Services
{
    using Fanex.CustomerSetting.Dtos.Params;
    using Fanex.CustomerSetting.Dtos.Results;

    public interface IValidationService
    {
        IResult ValidateQuerySetting(QuerySettingParam queryParam);

        IResult ValidateUpdatingSetting(UpdatingSettingParam updatingParam);

        IResult ValidateUpdatingMultipleSetting(UpdatingMultipleSettingParam updatingMultipleParam);
    }
}