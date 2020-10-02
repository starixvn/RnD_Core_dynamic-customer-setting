namespace Fanex.CustomerSetting.Client40.Attributes
{
    using System;
    using System.Web.Helpers;

    internal static class AntiForgeryExtension
    {
        internal delegate void GetTokens(string oldCookieToken, out string newCookieToken, out string formToken);

        internal static GetTokens GetTokensMethod { get; set; }

        public static string GetTokenHeaderValue()
        {
            var cookieToken = string.Empty;
            var formToken = string.Empty;
            if (GetTokensMethod == null)
            {
                AntiForgery.GetTokens(null, out cookieToken, out formToken);
            }
            else
            {
                GetTokensMethod.Invoke(null, out cookieToken, out formToken);
            }

            return string.Format("{0}:{1}", cookieToken, formToken);
        }
    }
}