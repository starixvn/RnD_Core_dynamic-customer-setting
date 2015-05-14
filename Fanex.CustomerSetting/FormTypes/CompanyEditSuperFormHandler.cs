namespace Fanex.CustomerSetting.FormTypes
{
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Models;

    public class CompanyEditSuperFormHandler : IFormHandler
    {
        public bool IsCommandOnMember
        {
            get
            {
                return false;
            }
        }

        public bool HasPermission(QueryInfo model)
        {
            return !(model.RoleId != RoleType.Company || model.TargetRoleId != RoleType.Super);
        }
    }
}