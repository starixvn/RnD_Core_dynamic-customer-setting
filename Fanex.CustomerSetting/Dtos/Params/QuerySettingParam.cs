namespace Fanex.CustomerSetting.Dtos.Params
{
    using Fanex.CustomerSetting.Enumerations;

    public class QuerySettingParam
    {
        public int CustomerId { get; set; }

        public CustomerLevel CustomerLevel { get; set; }

        public byte ProductId { get; set; }

        public CustomerLevel? UplineLevel { get; set; }
    }
}