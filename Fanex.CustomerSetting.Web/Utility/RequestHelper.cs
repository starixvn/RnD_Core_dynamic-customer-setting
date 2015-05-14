namespace Fanex.CustomerSetting.Web.Utility
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Net;
    using System.Web;

    public static class RequestHelper
    {
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "This is a utlity function, not a property")]
        public static string GetClientIp()
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

        private static IPAddress GetInternalAddress(string hostAddress)
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