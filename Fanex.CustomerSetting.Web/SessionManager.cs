namespace Fanex.CustomerSetting.Web
{
    using System.Web;

    public static class SessionManager
    {
        private const string AppSessionKey = "CSR_Current_Session";

        public static ICurrentSession CurrentSession
        {
            get
            {
                return (ICurrentSession)HttpContext.Current.Session[AppSessionKey];
            }

            set
            {
                HttpContext.Current.Session[AppSessionKey] = value;
            }
        }
    }
}