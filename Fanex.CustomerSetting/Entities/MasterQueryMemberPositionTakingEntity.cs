namespace Fanex.CustomerSetting.Entities
{
    using Fanex.CustomerSetting.Attributes;

    public class MasterQueryMemberPositionTakingEntity : PositionTakingEntity
    {
        public byte A1 { get; set; }

        [OutputElement("MasterPT")]
        public byte M1 { get; set; }

        [OutputElement("IsAutoMasterPT")]
        public bool IM1 { get; set; }

        [OutputElement("AutoMasterPT")]
        public byte UM1 { get; set; }

        public bool IM2 { get; set; }

        public byte M3 { get; set; }

        public byte N3 { get; set; }

        public override bool IsValid()
        {
            return base.IsValid()
                && IsPositionTakingValue(M1)
                && IsPositionTakingValue(UM1)
                && !(IM1 && M1 > 0)
                && !(!IM1 && UM1 > 0);
        }
    }
}