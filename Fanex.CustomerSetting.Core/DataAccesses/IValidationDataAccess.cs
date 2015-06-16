namespace Fanex.CustomerSetting.Core.DataAccesses
{
    using Fanex.CustomerSetting.Core.Criteria.Validation;
    using Fanex.CustomerSetting.Core.Dtos.Results;

    public interface IValidationDataAccess
    {
        IResult ValidatePermission(PermissionValidationCommand criteria);

        IResult ValidatePermissionMultiple(PermissionValidationMultipleCommand criteria);
    }
}