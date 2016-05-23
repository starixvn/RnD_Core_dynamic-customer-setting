namespace Fanex.CustomerSetting.Matrix.Models
{
    using CustomerSetting.Models;
    using CustomerSetting.Models.Enumerations;

    public class Customer : Entity<int>
    {
        public string Name { get; set; }

        public byte Currency { get; set; }

        public Level Level { get; set; }
    }
}