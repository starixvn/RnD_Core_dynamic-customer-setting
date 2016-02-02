namespace Fanex.CustomerSetting.WebApi.Filters
{
    using System;
    using System.Globalization;
    using System.Threading;
    using System.Web.Http.Controllers;
    using System.Web.Http.Filters;
    using Fanex.CustomerSetting.WebApi.Models.Request;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public sealed class SetLanguageFilter : ActionFilterAttribute
    {
        private const string ActionArgumentName = "request";
        private const string DefaultLanguage = "en-US";
        private static readonly CultureInfo DefaultCulture = CultureInfo.CreateSpecificCulture(DefaultLanguage);

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var currentLanguage = DefaultLanguage;

            if (actionContext != null
                && actionContext.ActionArguments.ContainsKey(ActionArgumentName)
                && (actionContext.Request.Method.Method.ToUpper().Contains("GET") || actionContext.Request.Method.Method.ToUpper().Contains("POST")))
            {
                var request = actionContext.ActionArguments[ActionArgumentName] as SettingRequest;

                if (request != null && !string.IsNullOrWhiteSpace(request.Language))
                {
                    currentLanguage = request.Language;
                }
            }

            if (currentLanguage != DefaultLanguage)
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(currentLanguage);
                Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

                Thread.CurrentThread.CurrentCulture.DateTimeFormat = DefaultCulture.DateTimeFormat;
                Thread.CurrentThread.CurrentCulture.NumberFormat = DefaultCulture.NumberFormat;
            }

            base.OnActionExecuting(actionContext);
        }
    }
}