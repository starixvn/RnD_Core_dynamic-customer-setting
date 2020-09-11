namespace Fanex.CustomerSetting.SettingType8.Models
{
    public class LevelGroup
    {
        public LevelGroup()
        {
        }

        public LevelGroup(int levelId, int levelGroupId, string levelGroupName, bool selected)
        {
            LevelId = levelId;
            LevelGroupId = levelGroupId;
            LevelGroupName = levelGroupName;
            Selected = selected;
        }

        public int LevelId { get; set; }

        public int LevelGroupId { get; set; }

        public string LevelGroupName { get; set; }

        public bool Selected { get; set; }

        public override bool Equals(object obj)
        {
            var otherObj = obj as LevelGroup;

            if (otherObj == null)
            {
                return false;
            }

            return Equals(LevelId, otherObj.LevelId)
                && Equals(LevelGroupId, otherObj.LevelGroupId)
                && Equals(Selected, otherObj.Selected);
        }

        public override int GetHashCode()
        {
            return LevelId.GetHashCode() ^ LevelGroupId.GetHashCode() ^ Selected.GetHashCode();
        }
    }
}