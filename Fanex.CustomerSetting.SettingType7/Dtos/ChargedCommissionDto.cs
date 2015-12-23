namespace Fanex.CustomerSetting.SettingType7.Dtos
{
    internal class ChargedCommissionDto
    {
        public short SportTypeId { get; set; }

        public short BetTypeId { get; set; }

        public float ChargedCommission { get; set; }

        public float UplineChargedCommission { get; set; }
    }
}