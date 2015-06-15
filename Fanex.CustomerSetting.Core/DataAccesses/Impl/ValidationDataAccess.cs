namespace Fanex.CustomerSetting.Core.DataAccesses.Impl
{
    using Fanex.CustomerSetting.Core.Criteria.Validation;
    using Fanex.CustomerSetting.Core.Results;

    public class ValidationDataAccess : BaseDataAccess, IValidationDataAccess
    {
        public IResult ValidatePermission(PermissionValidateCriteria criteria)
        {
            return ExecuteNonQuery(criteria);
        }

        public IResult ValidatePermissionMultiple(PermissionValidateMultipleCriteria criteria)
        {
            return ExecuteNonQuery(criteria);
        }
    }
}