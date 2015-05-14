namespace Fanex.CustomerSetting.Entities
{
    using Fanex.CustomerSetting.Attributes;

    public class AgentQueryMemberPositionTakingEntity : PositionTakingEntity
    {
        [OutputElement("AgentPT")]
        public byte A1 { get; set; }

        public byte A2 { get; set; }

        public byte N2 { get; set; }

        public override bool IsValid()
        {
            return base.IsValid() && IsPositionTakingValue(A1);
        }
    }
}