namespace Fanex.CustomerSetting.Entities
{
    using Fanex.CustomerSetting.Attributes;

    public class MasterQueryAgentPositionTakingEntity : PositionTakingEntity
    {
        [OutputElement("MasterPT")]
        public byte M2 { get; set; }

        [OutputElement("AutoMasterPT")]
        public byte UM2 { get; set; }

        [OutputElement("IsAutoMasterPT")]
        public bool IM2 { get; set; }

        [OutputElement("AgentPT")]
        public byte A2 { get; set; }

        [OutputElement("AgentMinPT")]
        public byte N2 { get; set; }

        public byte M3 { get; set; }

        public byte N3 { get; set; }

        public byte A4 { get; set; }

        public override bool IsValid()
        {
            return base.IsValid()
                && IsPositionTakingValue(M2)
                && IsPositionTakingValue(UM2)
                && IsPositionTakingValue(A2)
                && IsPositionTakingValue(N2)
                && A2 >= N2
                && !(IM2 && M2 > 0)
                && !(!IM2 && UM2 > 0);
        }
    }
}