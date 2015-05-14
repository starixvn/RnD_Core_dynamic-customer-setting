namespace Fanex.CustomerSetting.Entities
{
    using Fanex.CustomerSetting.Attributes;

    public class SuperQueryMasterPositionTakingEntity : PositionTakingEntity
    {
        [OutputElement("SuperPT")]
        public byte S3 { get; set; }

        [OutputElement("AutoSuperPT")]
        public byte US3 { get; set; }

        [OutputElement("IsAutoSuperPT")]
        public bool IS3 { get; set; }

        [OutputElement("MasterPT")]
        public byte M3 { get; set; }

        [OutputElement("MasterMinPT")]
        public byte N3 { get; set; }

        public byte S4 { get; set; }

        public byte M4 { get; set; }

        public byte N4 { get; set; }

        public override bool IsValid()
        {
            return base.IsValid()
                && IsPositionTakingValue(S3)
                && IsPositionTakingValue(US3)
                && IsPositionTakingValue(M3)
                && IsPositionTakingValue(N3)
                && M3 >= N3
                && !(IS3 && S3 > 0)
                && !(!IS3 && US3 > 0);
        }
    }
}