namespace Fanex.CustomerSetting.SettingType10.Models
{
    public class CommissionGroup
    {
        public int Id { get; set; }

        public string Value { get; set; }

        public string DisplayingName { get; set; }

        public decimal Rate { get; set; }

        public decimal MaxRate { get; set; }
    }
}