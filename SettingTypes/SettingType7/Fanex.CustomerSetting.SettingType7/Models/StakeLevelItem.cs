namespace Fanex.CustomerSetting.SettingType7.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using CustomerSetting.Models;

    public class StakeLevelItem : SettingItemDefault
    {
        private const string StakeLevelXmlFormat = "<StakeLevel {0} StakeLevelList=\"{1}\"/>";

        public StakeLevelItem()
        {
            StakeRanges = new List<StakeRange>();
        }

        public List<StakeRange> StakeRanges { get; set; }

        public override string ToXml()
        {
            var checkedStakeLevelIds = StakeRanges
                .Where(stakeRange => stakeRange.Selected && stakeRange.StakeLevelId > 0)
                .Select(stakeRange => stakeRange.StakeLevelId);

            return string.Format(StakeLevelXmlFormat, base.ToXml(), string.Join(",", checkedStakeLevelIds));
        }

        public override bool Equals(object obj)
        {
            var otherObj = obj as StakeLevelItem;

            if (otherObj == null)
            {
                return false;
            }

            return Equals(SportTypeId, otherObj.SportTypeId)
                && Equals(BetTypeId, otherObj.BetTypeId)
                && StakeRanges.SequenceEqual(otherObj.StakeRanges);
        }

        public override int GetHashCode()
        {
            int stakeRangesHashCode = 0;

            foreach (StakeRange stakeRange in StakeRanges)
            {
                stakeRangesHashCode ^= stakeRange.GetHashCode();
            }

            return SportTypeId.GetHashCode() ^ BetTypeId.GetHashCode() ^ stakeRangesHashCode;
        }
    }
}