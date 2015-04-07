namespace Fanex.CustomerSetting.FormTypes
{
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Models;

    /// <summary>
    /// CompanyEditSuperFormHandler class.
    /// </summary>
    public class CompanyEditSuperFormHandler : IFormHandler
    {
        public bool HasPermission(QueryInfo model)
        {
            return !(model.RoleId != RoleType.Company || model.TargetRoleId != RoleType.Super);
        }
    }
}