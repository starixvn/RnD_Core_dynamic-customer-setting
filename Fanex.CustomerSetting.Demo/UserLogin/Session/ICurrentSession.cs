namespace Fanex.CustomerSetting.Demo.UserLogin.Session
{
    /// <summary>
    /// Properties of an authenticated user.
    /// </summary>
    public interface ICurrentSession
    {
        int RoleId { get; set; }

        int CustId { get; set; }

        int SubAccountId { get; set; }

        string LanguageCode { get; set; }

        void ClearAll();
    }
}