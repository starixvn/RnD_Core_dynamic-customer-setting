namespace Fanex.CustomerSetting.SettingType5.Dtos
{
    internal class ChargeCommissionDto
    {
        public short SportTypeId { get; set; }

        public short BetTypeId { get; set; }

        public decimal ChargeCommission { get; set; }

        public decimal UplineChargeCommission { get; set; }

        public decimal? UplineOfUplineChargeCommission { get; set; }
    }
}