namespace Fanex.CustomerSetting.FormTypes
{
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Models;

    /// <summary>
    /// AgentEditMemberFormHandler class.
    /// </summary>
    public class AgentEditMemberFormHandler : IFormHandler
    {
        public bool HasPermission(QueryInfo model)
        {
            return !((model.RoleId != RoleType.Company && model.RoleId != RoleType.Agent) || model.TargetRoleId != RoleType.Member);
        }
    }
}