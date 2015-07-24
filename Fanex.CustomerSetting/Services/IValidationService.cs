namespace Fanex.CustomerSetting.Services
{
    using Fanex.CustomerSetting.Requests;
    using Fanex.CustomerSetting.Results;

    public interface IValidationService
    {
        IResult ValidateQuerySettingAction(CustomerSettingQuery query);

        IResult ValidateUpdateSettingAction(UpdateCustomerSettingCommand command);

        IResult ValidateUpdateMultipleSettingAction(MultipleUpdateCustomerSettingCommand command);
    }
}