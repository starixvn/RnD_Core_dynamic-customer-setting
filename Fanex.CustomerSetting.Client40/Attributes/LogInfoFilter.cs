namespace Fanex.CustomerSetting.Client40.Attributes
{
    using System;
    using System.IO;
    using System.Threading;
    using System.Web;
    using System.Web.Mvc;
    using Fanex.CustomerSetting.Client40.Configurations;
    using Fanex.Logging;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    internal sealed class LogInfoFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var request = HttpContext.Current.Request;

            string message = string.Join(
                "\r\n",
                "URL: " + request.Url.OriginalString,
                "REFERRER: " + request.UrlReferrer,
                "FORM: " + (request.Form.Count > 0 ? request.Form.ToString() : string.Empty),
                "BODY: " + GetRequestBody(request),
                "CULTURE: " + Thread.CurrentThread.CurrentUICulture,
                "BROWSER CAPABILITIES:" + GetClientBrowserInformation(request));

            LogService.Info(message, CustomerSettingConfiguration.Settings.ClientLogCategory);
        }

        internal static string GetClientBrowserInformation(HttpRequest request)
        {
            if (request.Browser == null)
            {
                return string.Empty;
            }

            var browser = request.Browser;
            var browserInformation = string.Join(
                "\r\n",
                "Type: " + browser.Type,
                "Name: " + browser.Browser,
                "Version: " + browser.Version,
                "Major Version: " + browser.MajorVersion,
                "Minor Version: " + browser.MinorVersion,
                "Platform: " + browser.Platform,
                "Is Beta: " + browser.Beta,
                "Is Crawler: " + browser.Crawler,
                "Is AOL: " + browser.AOL,
                "Is Win16: " + browser.Win16,
                "Is Win32: " + browser.Win32,
                "Supports Frames: " + browser.Frames,
                "Supports Tables: " + browser.Tables,
                "Supports Cookies: " + browser.Cookies);

            return browserInformation;
        }

        internal static string GetRequestBody(HttpRequest request)
        {
            Stream req = request.InputStream;
            req.Seek(0, SeekOrigin.Begin);
            var streamReader = new StreamReader(req);

            return streamReader.ReadToEnd();
        }
    }
}