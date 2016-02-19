namespace Fanex.CustomerSetting.SettingType6.Models
{
    internal class CurrentWinLossAmountRequest
    {
        public int CustomerId { get; set; }

        public short SportTypeId { get; set; }

        public short BetTypeId { get; set; }
    }
}