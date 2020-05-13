using Fanex.CustomerSetting.SettingType17.Resources;

namespace Fanex.CustomerSetting.SettingType17.Dtos
{
    internal class StakeLevelDto
    {
        public short SportTypeId { get; set; }

        public short BetTypeId { get; set; }

        public int StakeLevelId { get; set; }

        public string StakeLevelRange { get; set; }

        public bool Selected { get; set; }

        public int MaxChoice { get; set; }
    }
}