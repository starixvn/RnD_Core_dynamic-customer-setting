namespace Fanex.CustomerSetting.Client40.Models
{
    using Fanex.CustomerSetting.Client40.Models.Requests;

    public class PermissionContext : SettingRequest
    {
        public bool AllowUpdate { get; set; }
    }
}