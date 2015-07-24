namespace Fanex.CustomerSetting.Services
{
    using Fanex.CustomerSetting.Models.Requests;
    using Fanex.CustomerSetting.Models.Results;

    public interface IValidationService
    {
        IResult ValidateQuerySettingAction(CustomerSettingQuery query);

        IResult ValidateUpdateSettingAction(UpdateCustomerSettingCommand command);

        IResult ValidateUpdateMultipleSettingAction(MultipleUpdateCustomerSettingCommand command);
    }
}