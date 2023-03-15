namespace Fanex.CustomerSetting.SettingType23.Dtos
{
    internal class BetLimitItemDto
    {
        public short SportTypeId { get; set; }

        public short BetTypeId { get; set; }

        public short SubType { get; set; }

        public decimal MinBet { get; set; }

        public decimal MaxBet { get; set; }

        public decimal MaxPerMatch { get; set; }

        public decimal? MaxPerBall { get; set; }

        public decimal MinBetMin { get; set; }

        public decimal MaxBetMax { get; set; }

        public decimal MaxPerMatchMax { get; set; }

        public decimal? MaxPerBallMax { get; set; }
    }
}