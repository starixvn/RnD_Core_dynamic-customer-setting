namespace Fanex.CustomerSetting.SettingType10.Dtos
{
    internal class CommissionDto
    {
        public short SportTypeId { get; set; }

        public short BetTypeId { get; set; }

        public int GroupId { get; set; }

        public string GroupValue { get; set; }

        public decimal Rate { get; set; }

        public decimal UplineRate { get; set; }

        public decimal? GrandUplineRate { get; set; }
    }
}