namespace Fanex.CustomerSetting.SettingType20.Dtos
{
    internal class BetLimitDto
    {
        public short SportTypeId { get; set; }

        public short BetTypeId { get; set; }

        public decimal MinBet { get; set; }

        public decimal MaxBet { get; set; }

        public decimal MinBetMin { get; set; }

        public decimal MaxBetMax { get; set; }
    }
}