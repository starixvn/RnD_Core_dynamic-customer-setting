namespace Fanex.CustomerSetting.SettingType8.Dtos
{
    internal class StakeLevelGroupDto
    {
        public short SportTypeId { get; set; }

        public short BetTypeId { get; set; }

        public int StakeLevelId { get; set; }

        public int StakeLevelGroupId { get; set; }

        public bool Selected { get; set; }

        public bool IsBuyBonus { get; set; }
    }
}