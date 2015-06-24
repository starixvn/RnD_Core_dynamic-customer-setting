namespace Fanex.CustomerSetting.Models
{
    using Fanex.CustomerSetting.Enumerations;

    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int SubAccountId { get; set; }

        public UserLevel Level { get; set; }
    }
}
