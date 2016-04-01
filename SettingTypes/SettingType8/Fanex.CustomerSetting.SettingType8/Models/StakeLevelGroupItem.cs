namespace Fanex.CustomerSetting.SettingType8.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using CustomerSetting.Models;

    public class StakeLevelGroupItem : SettingItemDefault
    {
        private const string StakeLevelGroupXmlFormat = "<StakeLevel {0} StakeLevelList=\"{1}\"/>";

        public StakeLevelGroupItem()
        {
            LevelGroups = new List<LevelGroup>();
            StakeLevelItems = new List<StakeLevelItem>();
        }

        public List<LevelGroup> LevelGroups { get; set; }

        public List<StakeLevelItem> StakeLevelItems { get; set; }

        public int NumberOfDetailedItems { get; set; }

        public override string ToXml()
        {
            var checkedGroupIds = LevelGroups
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
                    && LevelGroups.SequenceEqual(otherObj.LevelGroups);
            }
        }

        public override int GetHashCode()
        {
            int levelGroupsHashCode = 0;

            foreach (LevelGroup levelGroup in LevelGroups)
            {
                levelGroupsHashCode ^= levelGroup.GetHashCode();
            }

            return SportTypeId.GetHashCode() ^ BetTypeId.GetHashCode() ^ levelGroupsHashCode;
        }
    }
}