namespace Fanex.CustomerSetting.WebApi.Helpers
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Web;

    public static class HttpRequestMessageExtensions
    {
        public static string GetClientIPAddress(this HttpRequestMessage request)
        {
            object httpContext = null;

            if (request?.Properties.TryGetValue("MS_HttpContext", out httpContext) == true)
            {
                return IPAddress.Parse(((HttpContextBase)httpContext).Request.UserHostAddress).ToString();
            }

            return "Unknown";
        }

        public static string GetBodyForm(this HttpRequestMessage request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "Request cannot be null");
            }

            string data;

            using (var stream = request.Content.ReadAsStreamAsync().Result)
            {
                if (stream.CanSeek)
                {
                    stream.Position = 0;
                }

                data = request.Content.ReadAsStringAsync().Result;
            }

            return data;
        }
    }
}