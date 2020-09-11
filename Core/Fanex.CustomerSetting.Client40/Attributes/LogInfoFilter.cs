namespace Fanex.CustomerSetting.Client40.Attributes
{
    using System;
    using System.IO;
    using System.Text;
    using System.Threading;
    using System.Web;
    using System.Web.Mvc;
    using Configurations;
    using Helpers;
    using Logging;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    internal sealed class LogInfoFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!CustomerSettingConfiguration.Settings.EnableRequestLogging)
            {
                return;
            }

            var request = HttpContext.Current.Request;
            var messageBuilder = new StringBuilder();

            messageBuilder
                .AppendLine($"URL: {request.Url.OriginalString}")
                .AppendLine($"REFERRER: {request.UrlReferrer}")
                .AppendLine($"FORM: {(request.Form.Count > 0 ? request.Form.ToString() : string.Empty)}")
                .AppendLine($"BODY: {GetRequestBody(request)}")
                .AppendLine($"CLIENT IP: {IPHelper.GetClientIPAddress(request)}")
                .AppendLine($"CULTURE: {Thread.CurrentThread.CurrentUICulture}")
                .AppendLine($"BROWSER CAPABILITIES: {GetClientBrowserInformation(request)}");

            LogService.Info(messageBuilder.ToString(), CustomerSettingConfiguration.Settings.ClientLogCategory);
        }

        internal static string GetClientBrowserInformation(HttpRequest request)
        {
            if (request.Browser == null)
            {
                return string.Empty;
            }

            var browser = request.Browser;
            var browserInformationBuilder = new StringBuilder();

            browserInformationBuilder
                .AppendLine($"Type: {browser.Type}")
                .AppendLine($"Name: {browser.Browser}")
                .AppendLine($"Version: {browser.Version}")
                .AppendLine($"Major Version: {browser.MajorVersion}")
                .AppendLine($"Minor Version: {browser.MinorVersion}")
                .AppendLine($"Platform: {browser.Platform}")
                .AppendLine($"Is Beta: {browser.Beta}")
                .AppendLine($"Is Crawler: {browser.Crawler}")
                .AppendLine($"Is AOL: {browser.AOL}")
                .AppendLine($"Is Win16: {browser.Win16}")
                .AppendLine($"Is Win32: {browser.Win32}")
                .AppendLine($"Supports Frames: {browser.Frames}")
                .AppendLine($"Supports Tables: {browser.Tables}")
                .AppendLine($"Supports Cookies: {browser.Cookies}");

            return browserInformationBuilder.ToString();
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