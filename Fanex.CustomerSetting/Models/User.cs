namespace Fanex.CustomerSetting.Models
{
    using System;
    using Fanex.CustomerSetting.Enumerations;

    [Serializable]
    public class User : Entity<int>
    {
        public string Name { get; set; }

        public int SubAccountId { get; set; }

        public UserLevel Level { get; set; }
    }
}