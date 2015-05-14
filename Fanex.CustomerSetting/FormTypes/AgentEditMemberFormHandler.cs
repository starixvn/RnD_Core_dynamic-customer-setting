namespace Fanex.CustomerSetting.FormTypes
{
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Models;

    public class AgentEditMemberFormHandler : IFormHandler
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
            return !((model.RoleId != RoleType.Company && model.RoleId != RoleType.Agent) || model.TargetRoleId != RoleType.Member);
        }
    }
}