namespace Fanex.CustomerSetting.SettingType7.Dtos
{
    internal class StakeLevelDto
    {
        public short SportTypeId { get; set; }

        public short BetTypeId { get; set; }

        public int StakeLevelId { get; set; }

        public string StakeLevelRange { get; set; }

        public bool Selected { get; set; }
    }
}