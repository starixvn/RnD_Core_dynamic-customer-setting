namespace Fanex.CustomerSetting.MatrixClient.AntiForgery
{
    using System;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Web.Helpers;
    using System.Web.Mvc;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public sealed class ValidateAntiForgery : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext != null)
            {
                NameValueCollection headers = filterContext.HttpContext.Request.Headers;
                string requestVerificationToken = headers.GetValues("__RequestVerificationToken").FirstOrDefault();
                string cookieToken = null;
                string formToken = null;

                if (requestVerificationToken != null)
                {
                    string[] tokens = requestVerificationToken.Split(':');

                    if (tokens.Length == 2)
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