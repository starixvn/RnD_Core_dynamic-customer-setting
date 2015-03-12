// <history>
//   <change who="Kaden.Trinh" date="2015.03.04">Created and Implement current session user.</change>
// </history>

namespace Fanex.CustomerSetting.Web
{
    using System;
    using System.Web;

    /// <summary>
    /// Properties of an authenticated user.
    /// </summary>
    public class CurrentSession : ICurrentSession
    {
        #region Constant

        public const string RoleIdSessionKey = "CSR_ROLE_ID";
        public const string CustIdSessionKey = "CSR_CUST_ID";
        public const string SubAccountIdSessionKey = "CSR_SUB_ACCOUNT_ID";
        public const string LanguageCodeSessionKey = "CSR_LANGUAGE_CODE";

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
                ? Convert.ToInt32(HttpContext.Current.Session[RoleIdSessionKey])
                : 0;
            }

            set
            {
                HttpContext.Current.Session[RoleIdSessionKey] = value;
            }
        }

        public int CustId
        {
            get
            {
                return (HttpContext.Current != null)
                ? Convert.ToInt32(HttpContext.Current.Session[CustIdSessionKey])
                : 0;
            }

            set
            {
                HttpContext.Current.Session[CustIdSessionKey] = value;
            }
        }

        public int SubAccountId
        {
            get
            {
                return (HttpContext.Current != null)
                ? Convert.ToInt32(HttpContext.Current.Session[SubAccountIdSessionKey])
                : 0;
            }

            set
            {
                HttpContext.Current.Session[SubAccountIdSessionKey] = value;
            }
        }

        public string LanguageCode
        {
            get
            {
                var languageCode = "en-US";
                if (HttpContext.Current != null)
                {
                    var language = Convert.ToString(HttpContext.Current.Session[LanguageCodeSessionKey]);
                    if (!string.IsNullOrEmpty(language))
                    {
                        languageCode = language;
                    }
                }

                return languageCode;
            }

            set
            {
                HttpContext.Current.Session[LanguageCodeSessionKey] = !string.IsNullOrEmpty(value) ? value : "en-US";
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