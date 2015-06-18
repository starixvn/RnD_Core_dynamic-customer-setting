namespace Fanex.CustomerSetting.Entities
{
    using Fanex.CustomerSetting.Attributes;

    public class SuperQueryMemberPositionTakingEntity : PositionTakingEntity
    {
        public byte A1 { get; set; }

        public byte M1 { get; set; }

        [OutputElement("SuperPT")]
        public byte S1 { get; set; }

        [OutputElement("IsAutoSuperPT")]
        public bool IS1 { get; set; }

        public bool IS3 { get; set; }

        [OutputElement("AutoSuperPT")]
        public byte US1 { get; set; }

        public byte S4 { get; set; }

        public byte N4 { get; set; }

        public byte M3 { get; set; }

        public override bool IsValid()
        {
            return base.IsValid()
                && IsPositionTakingValue(S1)
                && IsPositionTakingValue(US1)
                && !(IS1 && S1 > 0)
                && !(!IS1 && US1 > 0);
        }
    }
}