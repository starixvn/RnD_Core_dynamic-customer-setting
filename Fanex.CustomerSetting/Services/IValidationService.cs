namespace Fanex.CustomerSetting.Services
{
    using Fanex.CustomerSetting.Params;
    using Fanex.CustomerSetting.Results;

    public interface IValidationService
    {
        IResult ValidateQuerySettingAction(QueryCustomerSettingParam queryParam);

        IResult ValidateUpdateSettingAction(UpdateCustomerSettingParam updateParam);

        IResult ValidateUpdateMultipleSettingAction(UpdateMultipleCustomerSettingParam updateMultipleParam);
    }
}