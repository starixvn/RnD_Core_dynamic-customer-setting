namespace Fanex.CustomerSetting.Matrix.Models
{
    using Fanex.CustomerSetting.Enumerations;

    public class Customer
    {
        public int Id { get; set; }

        public int Name { get; set; }

        public byte Currency { get; set; }

        public CustomerLevel CustomerLevel { get; set; }

        public Customer Downline { get; set; }
    }
}