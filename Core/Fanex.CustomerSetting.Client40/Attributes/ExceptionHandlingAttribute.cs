namespace Fanex.CustomerSetting.Client40.Attributes
{
    using System;
    using System.Diagnostics.CodeAnalysis;
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
            if (filterContext != null)
            {
                var exception = filterContext.Exception;

                if (exception != null)
                {
                    Log(exception);
                }
            }

            base.OnException(filterContext);
        }

        [SuppressMessage("Microsoft.Globalization", 
            "CA1303:Do not pass literals as localized parameters", 
            MessageId = "Fanex.Logging.LogService.Error(System.String,System.String)", 
            Justification = "Reviewed!")]
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
                "BROWSER CAPABILITIES:" + LogInfoFilter.GetClientBrowserInformation(HttpContext.Current.Request));

            LogService.Error(message, CustomerSettingConfiguration.Settings.ClientLogCategory);
        }
    }
}