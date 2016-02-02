namespace Fanex.CustomerSetting.SettingType3.Dtos
{
    internal class CommissionItemDto
    {
        public short SportTypeId { get; set; }

        public short BetTypeId { get; set; }

        public decimal CommRate { get; set; }

        public decimal UplineCommRate { get; set; }

        public decimal? UplineOfUplineCommRate { get; set; }
    }
}