namespace Fanex.CustomerSetting.SettingType2.Template1.Dtos
{
    public class BetLimitItemDto
    {
        public short SportTypeId { get; set; }

        public short BetTypeId { get; set; }

        public decimal MinBet { get; set; }

        public decimal MaxBet { get; set; }

        public decimal MaxPerMatch { get; set; }

        public decimal MinBetMin { get; set; }

        public decimal MaxBetMax { get; set; }

        public decimal MaxPerMatchMax { get; set; }
    }
}
