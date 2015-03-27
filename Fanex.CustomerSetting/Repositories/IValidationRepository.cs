namespace Fanex.CustomerSetting.Repositories
{
    using Fanex.CustomerSetting.Criteria;

    /// <summary>
    /// IValidationRepository interface.
    /// </summary>
    public interface IValidationRepository
    {
        void ValidatePermission(PermissionValidateCriteria criteria);
    }
}