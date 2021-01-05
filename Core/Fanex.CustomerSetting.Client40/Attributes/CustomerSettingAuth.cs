namespace Fanex.CustomerSetting.Client40.Attributes
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using Configurations;
    using Models;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    internal sealed class CustomerSettingAuthAttribute : AuthorizeAttribute
    {
        public bool IsUpdate { get; set; }

        public bool IsMultipleUpdate { get; set; }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            PermissionContext permissionContext = CustomerSettingRegistration.GetPermissionContext();

            permissionContext.AuthorizationContext = filterContext;
            permissionContext.AllowUpdate = IsUpdate;

            var isAuthorized = true;

#pragma warning disable S103 // Lines should not be too long
#pragma warning disable S3358 // Ternary operators should not be nested
            isAuthorized = IsMultipleUpdate ? CustomerSettingRegistration.CheckMultipleUpdatePermission(permissionContext) : IsUpdate ? CustomerSettingRegistration.CheckUpdatePermission(permissionContext) : CustomerSettingRegistration.CheckQueryPermission(permissionContext);
#pragma warning restore S3358 // Ternary operators should not be nested
#pragma warning restore S103 // Lines should not be too long

            if (!isAuthorized)
            {
                UnAuthorizedAction(filterContext);
            }
        }

        private static void UnAuthorizedAction(AuthorizationContext filterContext)
        {
            HttpResponseBase response = filterContext.HttpContext.Response;

            if (filterContext.HttpContext.Request.IsAjaxRequest()
                || filterContext.HttpContext.Request.HttpMethod.Equals("POST", StringComparison.CurrentCultureIgnoreCase))
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