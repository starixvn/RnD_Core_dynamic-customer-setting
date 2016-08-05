namespace Fanex.CustomerSetting.SettingType9.Dtos
{
    internal class BetLimitDto
    {
        public short SportTypeId { get; set; }

        public short BetTypeId { get; set; }

        public decimal MinBet { get; set; }

        public decimal MaxBet { get; set; }

        public decimal MaxPerNumber { get; set; }

        public decimal MinBetMin { get; set; }

        public decimal MaxBetMax { get; set; }

        public decimal MaxPerNumberMax { get; set; }
    }
}