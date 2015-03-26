namespace Fanex.CustomerSetting.Entities
{
    using Fanex.CustomerSetting.Attributes;

    /// <summary>
    /// MasterCommandAgentPositionTakingEntity class.
    /// </summary>
    public class MasterCommandAgentPositionTakingEntity : MasterQueryAgentPositionTakingEntity
    {
        [OutputElement("UpdAllMems")]
        public bool IsUpdateAllMembers { get; set; }

        [OutputElement("NewMasterPT")]
        public byte MemberPositionTaking { get; set; }

        public override bool IsValid()
        {
            return base.IsValid()
                && IsPositionTakingValue(MemberPositionTaking)
                && !(IsUpdateAllMembers && IM2)
                && !(!IsUpdateAllMembers && MemberPositionTaking > 0);
        }
    }
}