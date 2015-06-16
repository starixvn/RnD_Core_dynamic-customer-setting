namespace Fanex.CustomerSetting.Core.DataAccesses
{
    using Fanex.CustomerSetting.Core.Dtos.Results;
    using Fanex.CustomerSetting.Core.StoredProcedureParams.Commands.Validation;

    public interface IValidationDataAccess
    {
        IResult ValidatePermission(PermissionValidationCommand criteria);

        IResult ValidatePermissionMultiple(PermissionValidationMultipleCommand criteria);
    }
}