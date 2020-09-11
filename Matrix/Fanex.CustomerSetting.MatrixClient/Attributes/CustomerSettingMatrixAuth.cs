namespace Fanex.CustomerSetting.MatrixClient.Attributes
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using Configurations;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public sealed class CustomerSettingMatrixAuthAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            var isAuthorized = SettingMatrixRegistration.CheckPermission();

            if (!isAuthorized)
            {
                UnAuthorizedAction(filterContext);
            }
        }

        private void UnAuthorizedAction(AuthorizationContext filterContext)
        {
            HttpResponseBase response = filterContext.HttpContext.Response;

            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                response.Clear();
                response.Write("You are not authorized on this function!");
                filterContext.Result = new HttpUnauthorizedResult();
                filterContext.Result.ExecuteResult(filterContext.Controller.ControllerContext);
                response.End();
            }
            else
            {
                filterContext.Result = new RedirectResult(SettingMatrixConfiguration.Settings.AccessDeniedAction);
            }
        }
    }
}