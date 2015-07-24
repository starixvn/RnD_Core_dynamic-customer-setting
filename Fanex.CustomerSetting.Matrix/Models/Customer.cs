namespace Fanex.CustomerSetting.Matrix.Models
{
    using Fanex.CustomerSetting.Models.Enumerations;

    public class Customer
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public byte Currency { get; set; }

        public Level CustomerLevel { get; set; }
    }
}