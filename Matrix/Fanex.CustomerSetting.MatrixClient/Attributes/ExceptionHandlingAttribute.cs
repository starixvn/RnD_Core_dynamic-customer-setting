namespace Fanex.CustomerSetting.MatrixClient.Attributes
{
    using System;
    using System.Text;
    using System.Threading;
    using System.Web;
    using System.Web.Mvc;
    using Configurations;
    using Logging;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class ExceptionHandlingAttribute : HandleErrorAttribute
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
            var request = HttpContext.Current.Request;
            var messageBuilder = new StringBuilder();

            messageBuilder
                .AppendLine(ex.Message)
                .AppendLine($"URL: {request.Url.OriginalString}")
                .AppendLine($"REFERRER: {request.UrlReferrer}")
                .AppendLine($"FORM: {(request.Form.Count > 0 ? request.Form.ToString() : string.Empty)}")
                .AppendLine($"SOURCE: {ex.Source}")
                .AppendLine($"TARGET SITE: {ex.TargetSite}")
                .AppendLine($"DETAIL: {ex}")
                .AppendLine($"STACK TRACE: {ex.StackTrace}")
                .AppendLine($"CULTURE: {Thread.CurrentThread.CurrentUICulture}")
                .AppendLine($"BROWSER CAPABILITIES: {ClientInformation(request)}");

            LogService.Error(messageBuilder.ToString(), SettingMatrixConfiguration.Settings.ClientLogCategory);
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