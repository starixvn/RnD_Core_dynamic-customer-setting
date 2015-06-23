namespace Fanex.CustomerSetting.Dtos.Params
{
    using Fanex.CustomerSetting.Enumerations;

    public class SettingQuery
    {
        public int CustomerId { get; set; }

        public CustomerLevel CustomerLevel { get; set; }

        public byte ProductId { get; set; }

        public CustomerLevel? UplineLevel { get; set; }
    }
}