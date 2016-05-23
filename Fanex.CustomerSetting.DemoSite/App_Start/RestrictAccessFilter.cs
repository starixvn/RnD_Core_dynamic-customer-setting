namespace Fanex.CustomerSetting.DemoSite.AppStart
{
    using System;
    using System.Web.Mvc;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public sealed class RestrictAccessFilter : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (DemoSiteConfiguration.Settings.EnableLogin)
            {
                if (filterContext.HttpContext.Session["IsAuthenticated"] == null)
                {
                    filterContext.HttpContext.Response.Clear();
                    filterContext.HttpContext.Response.Write("You are not authorized!");
                    filterContext.Result = new HttpUnauthorizedResult();
                    filterContext.Result.ExecuteResult(filterContext.Controller.ControllerContext);
                    filterContext.HttpContext.Response.End();
                }
            }
        }
    }
}