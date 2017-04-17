namespace Fanex.CustomerSetting.SettingType15.Models
{
    using CustomerSetting.Models;
    using Dtos;

    public class WinningShareItem : SettingItemDefault
    {
        public WinningShareItem()
        {
        }

        internal WinningShareItem(WinningShareDto dto)
        {
            SportTypeId = dto.SportTypeId;
            BetTypeId = dto.BetTypeId;
            WinningShare = dto.WinningShare;
        }

        public decimal WinningShare { get; set; }
    }
}