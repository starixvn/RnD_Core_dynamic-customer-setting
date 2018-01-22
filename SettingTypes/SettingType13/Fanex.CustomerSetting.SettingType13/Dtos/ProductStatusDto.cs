namespace Fanex.CustomerSetting.SettingType13.Dtos
{
    public class ProductStatusDto
    {
        public short SportTypeId { get; set; }

        public short BetTypeId { get; set; }

        public bool IsDisabled { get; set; }

        public bool? IsDisabledUpline { get; set; }
    }
}