namespace Fanex.CustomerSetting.Entities
{
    public class SuperQueryMasterBetLimitEntity : BetLimitEntity
    {
        public decimal MinBetMin { get; set; }

        public decimal MaxBetMax { get; set; }

        public decimal MaxPerMatchMax { get; set; }
    }
}