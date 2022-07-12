namespace Fanex.CustomerSetting.SettingType21.Dtos
{
    public class ProductStatusDto
    {
        public short SportTypeId { get; set; }

        public short BetTypeId { get; set; }

        public short ParentSportType { get; set; }

        public short ParentBetType { get; set; }
        
        public bool IsDisabled { get; set; }

        public bool? IsDisabledUpline { get; set; }
    }
}