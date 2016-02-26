namespace Fanex.CustomerSetting.Client40.Attributes
{
    using System;
    using static System.Net.WebRequestMethods;
    using System.Web;
    using System.Web.Mvc;    
    using Fanex.CustomerSetting.Client40.Configurations;
    using Fanex.CustomerSetting.Client40.Models;
    using Fanex.CustomerSetting.Client40.Models.Constants;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    internal sealed class AdditionalRequestAuth : AuthorizeAttribute
    {
        private const string ActionTypeName = "actionType";

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentException(nameof(filterContext), "Filter Context must not be null");
            }

            base.OnAuthorization(filterContext);

            var isAuthorized = false;
            var actionType = filterContext.Controller.ValueProvider.GetValue(ActionTypeName);

            if (actionType != null)
            {
                byte actualActionType = Convert.ToByte(actionType.AttemptedValue);
                PermissionContext permissionContext = CustomerSettingRegistration.GetPermissionContext();

                permissionContext.AuthorizationContext = filterContext;
                permissionContext.AllowUpdate = (actualActionType == ActionTypes.Update) || (actualActionType == ActionTypes.MultipleUpdate);

                switch (actualActionType)
                {
                    case ActionTypes.Query:
                        isAuthorized = CustomerSettingRegistration.CheckQueryPermission(permissionContext);
                        break;

                    case ActionTypes.Update:
                        isAuthorized = CustomerSettingRegistration.CheckUpdatePermission(permissionContext);
                        break;

                    case ActionTypes.MultipleUpdate:
                        isAuthorized = CustomerSettingRegistration.CheckMultipleUpdatePermission(permissionContext);
                        break;
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

            if (filterContext.HttpContext.Request.IsAjaxRequest() || Http.Post == filterContext.HttpContext.Request.HttpMethod)
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