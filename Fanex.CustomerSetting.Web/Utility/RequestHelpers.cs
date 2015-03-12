namespace Fanex.CustomerSetting.Web.Utility
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Net;
    using System.Web;

    /// <summary>
    /// RequestExtensions class.
    /// </summary>
    public static class RequestHelpers
    {
        /// <summary>
        /// Gets the client IP.
        /// </summary>
        /// <returns>Returns client IP.</returns>
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

        /// <summary>
        /// Gets the internal address.
        /// </summary>
        /// <param name="hostAddress">The host address.</param>
        /// <returns>Returns internal IP address.</returns>
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