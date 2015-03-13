namespace Fanex.CustomerSetting.Models
{
    using Fanex.CustomerSetting.Criteria;
    using Fanex.CustomerSetting.Enumerations;

    /// <summary>
    /// MessageInfo class.
    /// </summary>
    public class MessageInfo
    {
        public MessageInfo(UpdateCriteria criteria)
        {
            this.ErrCode = criteria.ErrCode;
            this.ErrMsg = criteria.ErrMsg;
            this.MsgType = (MsgType)criteria.MsgType;
        }

        public int ErrCode { get; private set; }

        public string ErrMsg { get; private set; }

        public MsgType MsgType { get; private set; }

        public string UserName { get; set; }

        public bool IsMultiple { get; set; }

        public RoleType TargetRoleId { get; set; }

        public SettingLevel SettingLevel { get; set; }

        public SettingType SettingType { get; set; }
    }
}