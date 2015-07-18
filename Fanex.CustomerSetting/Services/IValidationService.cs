namespace Fanex.CustomerSetting.Services
{
    using Fanex.CustomerSetting.Dtos.Params;
    using Fanex.CustomerSetting.Dtos.Results;

    public interface IValidationService
    {
        IResult ValidateQuerySettingAction(QuerySettingParam queryParam);

        IResult ValidateUpdateSettingAction(UpdateSettingParam updateParam);

        IResult ValidateUpdateMultipleSettingAction(UpdateMultipleSettingParam updateMultipleParam);
    }
}