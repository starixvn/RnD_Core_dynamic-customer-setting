namespace Fanex.CustomerSetting.Core.Models
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Net;
    using System.Web;

    public class SettingContext : ISettingContext
    {
        #region Constant

        private const string ActorSessionKey = "Fanex.CustomerSetting.Core.Actor";
        private const string SubAccountSessionKey = "Fanex.CustomerSetting.Core.SubAccount";
        private const string LanguageSessionKey = "Fanex.CustomerSetting.Core.Language";
        private const string DefaultLanguage = "en-US";

        #endregion Constant

        public SettingContext()
        {
        }

        public User Actor
        {
            get
            {
                return (User)HttpContext.Current.Session[ActorSessionKey];
            }

            set
            {
                HttpContext.Current.Session[ActorSessionKey] = value;
            }
        }

        public User SubAccount
        {
            get
            {
                return (User)HttpContext.Current.Session[SubAccountSessionKey];
            }

            set
            {
                HttpContext.Current.Session[SubAccountSessionKey] = value;
            }
        }

        public string Language
        {
            get
            {
                if (HttpContext.Current != null)
                {
                    var languageFromSession = Convert.ToString(HttpContext.Current.Session[LanguageSessionKey]);
                    if (!string.IsNullOrEmpty(languageFromSession))
                    {
                        return languageFromSession;
                    }
                }

                return DefaultLanguage;
            }

            set
            {
                HttpContext.Current.Session[LanguageSessionKey] = !string.IsNullOrEmpty(value) ? value : DefaultLanguage;
            }
        }

        public byte System { get; set; }

        public string IP
        {
            get
            {
                return GetClientIp();
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "This is a utlity function, not a property")]
        private string GetClientIp()
        {
            var httpRequest = HttpContext.Current.Request;
            var forwardHeader = httpRequest.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (forwardHeader != null)
            {
                return forwardHeader.Split(',').First().Trim();
            }

            string userHostAddress = httpRequest.UserHostAddress;
            var internalAddress = GetInternalAddress(userHostAddress);
            if (internalAddress == null)
            {
                var hostName = Dns.GetHostName();
                internalAddress = GetInternalAddress(hostName);
            }

            return internalAddress != null ? internalAddress.ToString() : string.Empty;
        }

        private IPAddress GetInternalAddress(string hostAddress)
        {
            foreach (var itemIPA in Dns.GetHostAddresses(hostAddress))
            {
                if (itemIPA.AddressFamily.ToString().Equals("InterNetwork", StringComparison.OrdinalIgnoreCase))
                {
                    return itemIPA;
                }
            }

            return null;
        }
    }
}