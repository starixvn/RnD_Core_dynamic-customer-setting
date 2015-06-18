namespace Fanex.CustomerSetting.Web
{
    public interface ICurrentSession
    {
        int RoleId { get; set; }

        int CustId { get; set; }

        int SubAccountId { get; set; }

        string LanguageCode { get; set; }

        void ClearAll();
    }
}