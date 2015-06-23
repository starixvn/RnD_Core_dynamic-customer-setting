namespace Fanex.CustomerSetting.Models
{
    using Fanex.CustomerSetting.Enumerations;

    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public UserLevel Level { get; set; }
    }
}
