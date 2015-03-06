namespace Fanex.CustomerSetting.Site.UserLogin
{
    /// <summary>
    /// Model login class.
    /// </summary>
    public class LoginViewModel
    {
        public LoginViewModel()
        {
        }

        public int RoleId { get; set; }

        public int CustId { get; set; }

        public int SubAccountId { get; set; }

        public string LanguageCode { get; set; }

        public bool DisableLogin { get; set; }

        public bool HideForm { get; set; }
    }
}