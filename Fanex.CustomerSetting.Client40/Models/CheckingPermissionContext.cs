namespace Fanex.CustomerSetting.Client40.Models
{
    public class CheckingPermissionContext : SettingContext
    {
        public string CustomerIds { get; set; }

        public byte CustomerLevel { get; set; }
    }
}