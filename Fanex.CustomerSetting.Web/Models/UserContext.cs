namespace Fanex.CustomerSetting.Web.Models
{
    using Fanex.CustomerSetting.Enumerations;

    /// <summary>
    /// UserContext class.
    /// </summary>
    public class UserContext
    {
        public bool IsInternal { get; set; }

        public int CustId { get; set; }

        public int SubAccountId { get; set; }

        public RoleType RoleId { get; set; }

        public string Language { get; set; }

        public bool HasCommandPermission
        {
            get
            {
                return !IsInternal;
            }
        }
    }
}