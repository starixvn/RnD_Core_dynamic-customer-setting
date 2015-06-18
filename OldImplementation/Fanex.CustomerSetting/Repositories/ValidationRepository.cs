namespace Fanex.CustomerSetting.Repositories
{
    using Fanex.CustomerSetting.Criteria;

    public class ValidationRepository : ResultRepository, IValidationRepository
    {
        public void ValidatePermission(PermissionValidateCriteria criteria)
        {
            ExecuteNonQuery(criteria);
        }

        public void ValidatePermission(PermissionValidateMultipleCriteria criteria)
        {
            ExecuteNonQuery(criteria);
        }
    }
}