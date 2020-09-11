namespace Fanex.CustomerSetting.Client40.Attributes
{
    using System;
    using System.Text;
    using System.Threading;
    using System.Web;
    using System.Web.Mvc;
    using Configurations;
    using Logging;

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

        private static void Log(Exception ex)
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
                .AppendLine($"BROWSER CAPABILITIES: {LogInfoFilterAttribute.GetClientBrowserInformation(request)}");

            LogService.Error(messageBuilder.ToString(), CustomerSettingConfiguration.Settings.ClientLogCategory);
        }
    }
}