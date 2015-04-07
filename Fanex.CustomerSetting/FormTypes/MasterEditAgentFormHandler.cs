namespace Fanex.CustomerSetting.FormTypes
{
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Models;

    /// <summary>
    /// MasterEditAgentFormHandler class.
    /// </summary>
    public class MasterEditAgentFormHandler : IFormHandler
    {
        public bool HasPermission(QueryInfo model)
        {
            return !((model.RoleId != RoleType.Company && model.RoleId != RoleType.Master) || model.TargetRoleId != RoleType.Agent);
        }
    }
}