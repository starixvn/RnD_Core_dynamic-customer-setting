namespace Fanex.CustomerSetting.SettingType8.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using CustomerSetting.Models;

    public class StakeLevelGroupItem : SettingItemDefault
    {
        private const string StakeLevelGroupXmlFormat = "<StakeLevel {0} StakeLevelList=\"{1}\"/>";

        public IEnumerable<LevelGroup> Groups { get; set; }

        public IEnumerable<StakeLevel> Details { get; set; }

        public bool ShowMore { get; set; }

        public override string ToXml()
        {
            var checkedGroupIds = Groups
                .Where(group => group.Selected)
                .Select(group => group.LevelId);

            return string.Format(StakeLevelGroupXmlFormat, base.ToXml(), string.Join(",", checkedGroupIds));
        }

        public override bool Equals(object obj)
        {
            var otherObj = obj as StakeLevelGroupItem;

            if (otherObj == null)
            {
                return false;
            }
            else
            {
                return Equals(SportTypeId, otherObj.SportTypeId)
                    && Equals(BetTypeId, otherObj.BetTypeId)
                    && Groups.SequenceEqual(otherObj.Groups)
                    && Equals(ShowMore, otherObj.ShowMore);
            }
        }

        public override int GetHashCode()
        {
            int levelGroupsHashCode = 0;

            foreach (LevelGroup levelGroup in Groups)
            {
                levelGroupsHashCode ^= levelGroup.GetHashCode();
            }

            return SportTypeId.GetHashCode()
                ^ BetTypeId.GetHashCode()
                ^ levelGroupsHashCode
                ^ ShowMore.GetHashCode();
        }
    }
}