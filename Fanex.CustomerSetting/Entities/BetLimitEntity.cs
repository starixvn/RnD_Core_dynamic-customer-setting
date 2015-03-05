namespace Fanex.CustomerSetting.Entities
{
    /// <summary>
    /// BetLimit class.
    /// </summary>
    public abstract class BetLimitEntity : DataEntity
    {
        public decimal MinBet { get; set; }

        public decimal MaxBet { get; set; }

        public decimal MaxPerMatch { get; set; }
    }
}