namespace Fanex.CustomerSetting.SettingType4.Template1.Dtos
{
    internal class ProductStatusItemDto
    {
        public short SportTypeId { get; set; }

        public short BetTypeId { get; set; }

        public bool IsDisabled { get; set; }

        public bool? IsDisabledDownline { get; set; }

        public bool? IsDisabledUpline { get; set; }
    }
}
