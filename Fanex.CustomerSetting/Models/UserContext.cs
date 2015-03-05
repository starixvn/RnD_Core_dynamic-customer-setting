namespace Fanex.CustomerSetting.Models
{
    using Fanex.CustomerSetting.Enumerations;

    /// <summary>
    /// UserContext class.
    /// </summary>
    public class UserContext
    {
        public int CustId { get; set; }

        public int SubAccountId { get; set; }

        public RoleType RoleId { get; set; }

        public string Language { get; set; }
    }
}