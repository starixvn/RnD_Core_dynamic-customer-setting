namespace Fanex.CustomerSetting.Web.ViewModels
{
    using Fanex.CustomerSetting.Models.Enumerations;

    public class SettingFormQuery
    {
        public string Language { get; set; }

        public byte ProductId { get; set; }

        public Level CustomerLevel { get; set; }

        public Level UplineLevel { get; set; }

        public bool? IsMultiple { get; set; }

        public int? CustomerId { get; set; }

        public string CustomerIds { get; set; }

        public string CustomerNames { get; set; }
    }
}