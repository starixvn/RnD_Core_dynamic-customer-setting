namespace Fanex.CustomerSetting.SettingType8.Models
{
    public class StakeRange
    {
        public StakeRange()
        {
        }

        public StakeRange(int id, string value)
        {
            Id = id;
            Value = value;
        }

        public int Id { get; set; }

        public string Value { get; set; }

        public override bool Equals(object obj)
        {
            var otherObj = obj as StakeRange;

            if (otherObj == null)
            {
                return false;
            }

            return Equals(Id, otherObj.Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}