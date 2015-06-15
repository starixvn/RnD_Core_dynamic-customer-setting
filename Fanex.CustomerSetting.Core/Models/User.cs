namespace Fanex.CustomerSetting.Core.Models
{
    using Fanex.CustomerSetting.Core.Enumerations;

    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public UserLevel Level { get; set; }
    }
}
