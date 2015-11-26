namespace Fanex.CustomerSetting.Client40.Models
{
    using System.Web.Mvc;
    using Fanex.CustomerSetting.Client40.Models.Requests;

    public class PermissionContext : SettingRequest
    {
        public bool AllowUpdate { get; set; }

        public AuthorizationContext AuthorizationContext { get; set; }
    }
}