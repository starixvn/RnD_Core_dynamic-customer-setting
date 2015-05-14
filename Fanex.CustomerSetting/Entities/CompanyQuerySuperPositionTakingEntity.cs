namespace Fanex.CustomerSetting.Entities
{
    using Fanex.CustomerSetting.Attributes;

    public class CompanyQuerySuperPositionTakingEntity : PositionTakingEntity
    {
        [OutputElement("SuperPT")]
        public byte S4 { get; set; }

        [OutputElement("MasterPT")]
        public byte M4 { get; set; }

        [OutputElement("AgentPT")]
        public byte A4 { get; set; }

        [OutputElement("SuperMinPT")]
        public byte N4 { get; set; }

        public byte DefaultMaxPT { get; set; }

        public override bool IsValid()
        {
            return base.IsValid()
                && IsPositionTakingValue(S4)
                && IsPositionTakingValue(M4)
                && IsPositionTakingValue(A4)
                && IsPositionTakingValue(N4)
                && S4 >= M4 && M4 >= A4 && S4 >= N4;
        }
    }
}