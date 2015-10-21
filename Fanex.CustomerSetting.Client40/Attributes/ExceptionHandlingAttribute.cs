namespace Fanex.CustomerSetting.Client40.Attributes
{
    using System;
    using System.Threading;
    using System.Web;
    using System.Web.Mvc;
    using Fanex.CustomerSetting.Client40.Configurations;
    using Fanex.Logging;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    internal sealed class ExceptionHandlingAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            var exception = filterContext.Exception;

            if (exception != null)
            {
                Log(exception);
            }

            base.OnException(filterContext);
        }

        private void Log(Exception ex)
        {
            string message = string.Join(
                "\r\n",
                ex.Message,
                "URL: " + HttpContext.Current.Request.Url.OriginalString,
                "REFERRER: " + HttpContext.Current.Request.UrlReferrer,
                "FORM: " + (HttpContext.Current.Request.Form.Count > 0 ? HttpContext.Current.Request.Form.ToString() : string.Empty),
                "SOURCE: " + ex.Source,
                "TARGETSITE: " + ex.TargetSite,
                "DETAIL: " + ex,
                "STACKTRACE: " + ex.StackTrace,
                "CULTURE: " + Thread.CurrentThread.CurrentUICulture,
                "BROWSER CAPABILITIES:" + ClientInformation(HttpContext.Current.Request));

            LogService.Error(message, CustomerSettingConfiguration.Settings.ClientLogCategory);
        }

        private string ClientInformation(HttpRequest request)
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
    }
}