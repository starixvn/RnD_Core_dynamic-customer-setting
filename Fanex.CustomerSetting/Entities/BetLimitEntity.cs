namespace Fanex.CustomerSetting.Entities
{
    /// <summary>
    /// BetLimit class.
    /// </summary>
    public abstract class BetLimitEntity
    {
        public short SportType { get; set; }

        public string SportTypeName { get; set; }

        public short BetType { get; set; }

        public string BetTypeName { get; set; }

        public decimal MinBet { get; set; }

        public decimal MaxBet { get; set; }

        public decimal MaxPerMatch { get; set; }
    }
}