namespace Fanex.CustomerSetting.SettingType11.Dtos
{
    public class ProductStatusDto
    {
        public short SportTypeId { get; set; }

        public short BetTypeId { get; set; }

        public bool IsDisabled { get; set; }

        public bool? IsDisabledDownline { get; set; }

        public bool? IsDisabledUpline { get; set; }

        public bool HasDownline { get; set; }
    }
}