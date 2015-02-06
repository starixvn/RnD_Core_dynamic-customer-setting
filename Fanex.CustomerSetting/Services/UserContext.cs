namespace Fanex.CustomerSetting.Services
{
    using Fanex.CustomerSetting.Enumerations;

    /// <summary>
    /// UserContext class.
    /// </summary>
    public class UserContext
    {
        public int CustId { get; set; }

        public int SubAccountId { get; set; }

        public RoleType RoleType { get; set; }

        public string Language { get; set; }
    }
}