namespace Fanex.CustomerSetting.FormTypes
{
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Models;

    public class MasterEditMemberFormHandler : IFormHandler
    {
        public bool IsCommandOnMember
        {
            get
            {
                return true;
            }
        }

        public bool HasPermission(QueryInfo model)
        {
            return !((model.RoleId != RoleType.Company && model.RoleId != RoleType.Master) || model.TargetRoleId != RoleType.Member);
        }
    }
}