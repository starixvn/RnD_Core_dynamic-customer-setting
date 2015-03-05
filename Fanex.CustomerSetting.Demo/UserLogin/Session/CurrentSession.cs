// <history>
//   <change who="Kaden.Trinh" date="2015.03.04">Created and Implement current session user.</change>
// </history>
namespace Fanex.CustomerSetting.Demo.UserLogin.Session
{
    using System;
    using System.Web;

    /// <summary>
    /// Properties of an authenticated user.
    /// </summary>
    public class CurrentSession : ICurrentSession
    {
        #region Constant

        public const string ROLEID = "CustomerSettingRoleId";
        public const string CUSTID = "CustomerSettingCustId";
        public const string SUBACCOUNTID = "CustomerSettingSubAccountId";
        public const string LANGUAGECODE = "CustomerSettingLanguageCode";

        #endregion Constant

        /// <summary>
        /// Initializes a new instance of the <see cref="CurrentSession" /> class.
        /// </summary>
        public CurrentSession()
        {
        }

        public int RoleId
        {
            get
            {
                return (HttpContext.Current != null)
                ? Convert.ToInt32(HttpContext.Current.Session[ROLEID])
                : 0;
            }

            set
            {
                HttpContext.Current.Session[ROLEID] = value;
            }
        }

        public int CustId
        {
            get
            {
                return (HttpContext.Current != null)
                ? Convert.ToInt32(HttpContext.Current.Session[CUSTID])
                : 0;
            }

            set
            {
                HttpContext.Current.Session[CUSTID] = value;
            }
        }

        public int SubAccountId
        {
            get
            {
                return (HttpContext.Current != null)
                ? Convert.ToInt32(HttpContext.Current.Session[SUBACCOUNTID])
                : 0;
            }

            set
            {
                HttpContext.Current.Session[SUBACCOUNTID] = value;
            }
        }

        public string LanguageCode
        {
            get
            {
                return
                (HttpContext.Current != null)
                ? Convert.ToString(HttpContext.Current.Session[LANGUAGECODE])
                : "en-US";
            }

            set
            {
                HttpContext.Current.Session[LANGUAGECODE] = value;
            }
        }

        public void ClearAll()
        {
            if (!ReferenceEquals(HttpContext.Current, null))
            {
                HttpContext.Current.Session.Clear();
                HttpContext.Current.Session.Abandon();
            }
        }
    }
}