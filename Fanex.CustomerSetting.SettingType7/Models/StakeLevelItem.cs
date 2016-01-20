namespace Fanex.CustomerSetting.SettingType7.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.CustomerSetting.Models;

    public class StakeLevelItem : SettingItemDefault
    {
        private const string StakeLevelXmlFormat = "<StakeLevel {0} StakeLevelList=\"{1}\"/>";

        public StakeLevelItem()
        {
            StakeLevelValues = new List<StakeLevelValue>();
        }

        public List<StakeLevelValue> StakeLevelValues { get; set; }

        public override string ToXml()
        {
            var checkedStakeLevelIds = StakeLevelValues
                .Where(stakeLevelValue => stakeLevelValue.Selected)
                .Select(stakeLevelValue => stakeLevelValue.StakeLevelId);

            return string.Format(StakeLevelXmlFormat, base.ToXml(), string.Join(",", checkedStakeLevelIds));
        }

        public override bool Equals(object obj)
        {
            var otherObj = obj as StakeLevelItem;

            if (otherObj == null)
            {
                return false;
            }
            else
            {
                return Equals(SportTypeId, otherObj.SportTypeId)
                    && Equals(BetTypeId, otherObj.BetTypeId)
                    && StakeLevelValues.SequenceEqual(otherObj.StakeLevelValues);
            }
        }

        public override int GetHashCode()
        {
            int stakeLevelValuesHashCode = 0;

            foreach (StakeLevelValue stakeLevelValue in StakeLevelValues)
            {
                stakeLevelValuesHashCode ^= stakeLevelValue.GetHashCode();
            }

            return SportTypeId.GetHashCode() ^ BetTypeId.GetHashCode() ^ stakeLevelValuesHashCode;
        }
    }
}