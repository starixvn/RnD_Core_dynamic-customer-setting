namespace Fanex.CustomerSetting.Matrix.Models
{
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Models.Enumerations;

    public class Customer : Entity<int>
    {
        public string Name { get; set; }

        public byte Currency { get; set; }

        public Level Level { get; set; }
    }
}