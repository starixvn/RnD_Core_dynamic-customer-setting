namespace Fanex.CustomerSetting.Core.DataAccesses.Impl
{
    using Fanex.CustomerSetting.Core.Dtos.Results;
    using Fanex.CustomerSetting.Core.SPObjects.Commands.Validation;

    public class ValidationDataAccess : BaseDataAccess, IValidationDataAccess
    {
        public IResult ValidatePermission(PermissionValidationCommand command)
        {
            return ExecuteNonQuery(command);
        }

        public IResult ValidatePermissionMultiple(PermissionValidationMultipleCommand command)
        {
            return ExecuteNonQuery(command);
        }
    }
}