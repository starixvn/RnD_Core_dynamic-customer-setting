namespace Fanex.CustomerSetting.Core.DataAccesses.Impl
{
    using Fanex.CustomerSetting.Core.Dtos.Results;
    using Fanex.CustomerSetting.Core.StoredProcedureParams.Commands.Validation;

    public class ValidationDataAccess : BaseDataAccess, IValidationDataAccess
    {
        public IResult ValidatePermission(PermissionValidationCommand criteria)
        {
            return ExecuteNonQuery(criteria);
        }

        public IResult ValidatePermissionMultiple(PermissionValidationMultipleCommand criteria)
        {
            return ExecuteNonQuery(criteria);
        }
    }
}