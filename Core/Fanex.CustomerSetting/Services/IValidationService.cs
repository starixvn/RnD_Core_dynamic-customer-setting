namespace Fanex.CustomerSetting.Services
{
    using Models.Requests;
    using Models.Results;

    public interface IValidationService
    {
        IResult ValidateQuerySettingAction(CustomerSettingQuery query);

        IResult ValidateUpdateSettingAction(UpdateCustomerSettingCommand command);

        IResult ValidateUpdateMultipleSettingAction(MultipleUpdateCustomerSettingCommand command);
    }
}