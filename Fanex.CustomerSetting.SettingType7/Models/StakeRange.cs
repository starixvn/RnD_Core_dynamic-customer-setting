namespace Fanex.CustomerSetting.SettingType7.Models
{
    public class StakeRange
    {
        public StakeRange()
        {
        }

        public StakeRange(int stakeLevelId, string stakeLevelRange, bool selected)
        {
            StakeLevelId = stakeLevelId;
            StakeLevelRange = stakeLevelRange;
            Selected = selected;
        }

        public int StakeLevelId { get; set; }

        public string StakeLevelRange { get; set; }

        public bool Selected { get; set; }

        public override bool Equals(object obj)
        {
            var otherObj = obj as StakeRange;

            if (otherObj == null)
            {
                return false;
            }
            else
            {
                return Equals(StakeLevelId, otherObj.StakeLevelId)
                    && Equals(Selected, otherObj.Selected);
            }
        }

        public override int GetHashCode()
        {
            return StakeLevelId.GetHashCode() ^ Selected.GetHashCode();
        }
    }
}