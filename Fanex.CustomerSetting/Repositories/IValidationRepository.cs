namespace Fanex.CustomerSetting.Repositories
{
    using Fanex.CustomerSetting.Criteria;

    public interface IValidationRepository
    {
        void ValidatePermission(PermissionValidateCriteria criteria);

        void ValidatePermission(PermissionValidateMultipleCriteria criteria);
    }
}