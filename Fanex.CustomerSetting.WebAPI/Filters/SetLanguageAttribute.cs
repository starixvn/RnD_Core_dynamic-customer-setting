namespace Fanex.CustomerSetting.WebAPI.Filters
{
    using System.Globalization;
    using System.Threading;
    using System.Web.Http.Controllers;
    using System.Web.Http.Filters;
    using Fanex.CustomerSetting.WebAPI.Models.Request;

    public class SetLanguageAttribute : ActionFilterAttribute
    {
        private const string ActionArgumentName = "request";
        private const string DefaultLanguage = "en-US";
        private static readonly CultureInfo DefaultCulture = CultureInfo.CreateSpecificCulture(DefaultLanguage);

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var currentLanguage = DefaultLanguage;

            if (actionContext.Request.Method.Method.ToUpper().Contains("GET")
                || actionContext.Request.Method.Method.ToUpper().Contains("POST"))
            {
                var request = actionContext.ActionArguments[ActionArgumentName] as SettingRequest;

                if (request != null)
                {
                    currentLanguage = request.Language;
                }
            }

            if (currentLanguage != DefaultLanguage)
            {
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(currentLanguage);
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(currentLanguage);
                Thread.CurrentThread.CurrentCulture.DateTimeFormat = DefaultCulture.DateTimeFormat;
                Thread.CurrentThread.CurrentCulture.NumberFormat = DefaultCulture.NumberFormat;
            }

            base.OnActionExecuting(actionContext);
        }
    }
}