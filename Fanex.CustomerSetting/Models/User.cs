namespace Fanex.CustomerSetting.Models
{
    using System;
    using Fanex.CustomerSetting.Enumerations;

    [Serializable]
    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int SubAccountId { get; set; }

        public UserLevel Level { get; set; }
    }
}