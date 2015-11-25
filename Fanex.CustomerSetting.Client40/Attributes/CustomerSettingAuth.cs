namespace Fanex.CustomerSetting.Client40.Attributes
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using Fanex.CustomerSetting.Client40.Configurations;
    using Fanex.CustomerSetting.Client40.Models;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    internal sealed class CustomerSettingAuth : AuthorizeAttribute
    {
        public bool IsUpdate { get; set; }

        public bool IsMultipleUpdate { get; set; }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            PermissionContext permissionContext = CustomerSettingRegistration.GetPermissionContext();

            permissionContext.AllowUpdate = IsUpdate;
            var isAuthorized = true;

            if (IsMultipleUpdate)
            {
                isAuthorized = CustomerSettingRegistration.CheckMultipleUpdatePermission(permissionContext);
            }
            else
            {
                if (IsUpdate)
                {
                    isAuthorized = CustomerSettingRegistration.CheckUpdatePermission(permissionContext);
                }
                else
                {
                    isAuthorized = CustomerSettingRegistration.CheckQueryPermission(permissionContext);
                }
            }

            if (!isAuthorized)
            {
                UnAuthorizedAction(filterContext);
            }
        }

        private void UnAuthorizedAction(AuthorizationContext filterContext)
        {
            HttpResponseBase response = filterContext.HttpContext.Response;

            if (filterContext.HttpContext.Request.IsAjaxRequest()
                || filterContext.HttpContext.Request.HttpMethod.ToUpper() == "POST")
            {
                response.Clear();
                response.Write("{ \"message\": \"You are not authorized on this function!\" }");
                filterContext.Result = new HttpUnauthorizedResult();
                filterContext.Result.ExecuteResult(filterContext.Controller.ControllerContext);
                response.TrySkipIisCustomErrors = true;
                response.End();
            }
            else
            {
                filterContext.Result = new RedirectResult(CustomerSettingConfiguration.Settings.AccessDeniedAction);
            }
        }
    }
}