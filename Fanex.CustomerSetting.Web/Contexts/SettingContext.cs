namespace Fanex.CustomerSetting.Web.Contexts
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Net;
    using System.Web;
    using Fanex.CustomerSetting.Models;

    internal class SettingContext : ISettingContext
    {
        #region Constant

        private const string ActorSessionKey = "Fanex.CustomerSetting.Actor";
        private const string SubAccountSessionKey = "Fanex.CustomerSetting.SubAccount";
        private const string LanguageSessionKey = "Fanex.CustomerSetting.Language";
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

        public string Domain
        {
            get
            {
                return HttpContext.Current.Request.Url.Host;
            }
        }

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
            var forwardHeader = GetForwardHeader();

            if (!string.IsNullOrEmpty(forwardHeader))
            {
                return forwardHeader;
            }

            return GetInternalAddress();
        }

        private string GetForwardHeader()
        {
            var httpRequest = HttpContext.Current.Request;
            var forwardHeader = httpRequest.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (forwardHeader != null)
            {
                return forwardHeader.Split(',').First().Trim();
            }

            return string.Empty;
        }

        private string GetInternalAddress()
        {
            var httpRequest = HttpContext.Current.Request;
            string userHostAddress = httpRequest.UserHostAddress;
            var internalIPAddress = GetInternalIPAddress(userHostAddress);

            if (internalIPAddress == null)
            {
                var hostName = Dns.GetHostName();

                internalIPAddress = GetInternalIPAddress(hostName);
            }

            return internalIPAddress != null ? internalIPAddress.ToString() : string.Empty;
        }

        private IPAddress GetInternalIPAddress(string hostAddress)
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