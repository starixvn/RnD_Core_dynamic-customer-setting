namespace Fanex.CustomerSetting.DemoSite.App_Start
{
    using System.Web.Mvc;

    public class RestrictAccessFilter : AuthorizeAttribute
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