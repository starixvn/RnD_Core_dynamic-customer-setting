namespace Fanex.CustomerSetting.Client.Models
{
    public class SettingFormQuery
    {
        public string Language { get; set; }

        public byte ProductId { get; set; }

        public byte CustomerLevel { get; set; }

        public byte UplineLevel { get; set; }

        public bool? IsMultiple { get; set; }

        public int? CustomerId { get; set; }

        public string CustomerIds { get; set; }

        public string CustomerNames { get; set; }
    }
}