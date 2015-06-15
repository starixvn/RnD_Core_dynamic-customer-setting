namespace Fanex.CustomerSetting.Core.DataAccesses
{
    using Fanex.CustomerSetting.Core.Criteria.Validation;
    using Fanex.CustomerSetting.Core.Results;

    public interface IValidationDataAccess
    {
        IResult ValidatePermission(PermissionValidateCriteria criteria);

        IResult ValidatePermissionMultiple(PermissionValidateMultipleCriteria criteria);
    }
}