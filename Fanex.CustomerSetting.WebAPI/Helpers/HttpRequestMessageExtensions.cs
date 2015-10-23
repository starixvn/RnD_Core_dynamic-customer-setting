namespace Fanex.CustomerSetting.WebApi.Helpers
{
    using System.Net;
    using System.Net.Http;
    using System.Web;

    public static class HttpRequestMessageExtensions
    {
        public static string GetClientIpAddress(this HttpRequestMessage request)
        {
            if (request.Properties.ContainsKey("MS_HttpContext"))
            {
                return IPAddress.Parse(((HttpContextBase)request.Properties["MS_HttpContext"]).Request.UserHostAddress).ToString();
            }

            return "Unknown";
        }

        public static string GetBodyForm(this HttpRequestMessage request)
        {
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