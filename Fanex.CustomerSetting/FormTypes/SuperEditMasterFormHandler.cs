namespace Fanex.CustomerSetting.FormTypes
{
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Models;

    /// <summary>
    /// SuperEditMasterFormHandler class.
    /// </summary>
    public class SuperEditMasterFormHandler : IFormHandler
    {
        public bool HasPermission(QueryInfo model)
        {
            return !((model.RoleId != RoleType.Company && model.RoleId != RoleType.Super) || model.TargetRoleId != RoleType.Master);
        }
    }
}