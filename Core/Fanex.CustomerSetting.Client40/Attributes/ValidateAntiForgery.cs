namespace Fanex.CustomerSetting.Client40.Attributes
{
    using System;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Web.Helpers;
    using System.Web.Mvc;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    internal sealed class ValidateAntiForgeryAttribute : ActionFilterAttribute
    {
        private const string AntiForgeryKey = "__RequestVerificationToken";

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext != null)
            {
                NameValueCollection headers = filterContext.HttpContext.Request.Headers;
                string requestVerificationToken = headers.GetValues(AntiForgeryKey).FirstOrDefault();
                string cookieToken = null;
                string formToken = null;

                if (requestVerificationToken != null)
                {
                    string[] tokens = requestVerificationToken.Split(':');

#pragma warning disable S109 // Magic numbers should not be used
                    if (tokens.Length == 2)
#pragma warning restore S109 // Magic numbers should not be used
                    {
                        cookieToken = tokens[0].Trim();
                        formToken = tokens[1].Trim();
                    }
                }

                AntiForgery.Validate(cookieToken, formToken);
                base.OnActionExecuting(filterContext);
            }
        }
    }
}