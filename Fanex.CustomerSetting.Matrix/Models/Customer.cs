namespace Fanex.CustomerSetting.Matrix.Models
{
    using Fanex.CustomerSetting.Enumerations;

    public class Customer
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public byte Currency { get; set; }

        public UserLevel CustomerLevel { get; set; }
    }
}