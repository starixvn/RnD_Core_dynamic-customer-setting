namespace Fanex.CustomerSetting.Entities
{
    public class AgentQueryMemberBetLimitEntity : BetLimitEntity
    {
        public decimal MinBetMin { get; set; }

        public decimal MaxBetMax { get; set; }

        public decimal MaxPerMatchMax { get; set; }
    }
}