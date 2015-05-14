namespace Fanex.CustomerSetting.Models
{
    using Fanex.CustomerSetting.Enumerations;

    public class MessageInfo
    {
        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public MsgType MsgType { get; set; }

        public RoleType TargetRoleId { get; set; }

        public SettingLevel SettingLevel { get; set; }

        public SettingType SettingType { get; set; }
    }
}