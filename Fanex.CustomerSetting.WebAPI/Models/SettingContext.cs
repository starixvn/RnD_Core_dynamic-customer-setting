
namespace Fanex.CustomerSetting.WebAPI.Models
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Net;
    using System.Web;
    using Fanex.CustomerSetting.Models;

    public class SettingContext : ISettingContext
    {
        public User Actor
        {
            get;
            set;
        }

        public string Language
        {
            get;
            set;
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


        public byte System
        {
            get;
            set;
        }

        public string Domain
        {
            get
            {
                return HttpContext.Current.Request.Url.Host;
            }
        }
    }
}