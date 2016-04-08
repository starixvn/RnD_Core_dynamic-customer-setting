namespace Fanex.CustomerSetting.SettingType8.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using CustomerSetting.Models;

    public class StakeLevel : SettingItemDefault
    {
        public IEnumerable<StakeRange> StakeRanges { get; set; }

        public override bool Equals(object obj)
        {
            var otherObj = obj as StakeLevel;

            if (otherObj == null)
            {
                return false;
            }
            else
            {
                return Equals(SportTypeId, otherObj.SportTypeId)
                    && Equals(BetTypeId, otherObj.BetTypeId)
                    && StakeRanges.SequenceEqual(otherObj.StakeRanges);
            }
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