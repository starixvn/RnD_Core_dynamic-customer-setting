namespace Fanex.CustomerSetting.WebApi.Filters
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Filters;
    using Fanex.CustomerSetting.WebApi.Configurations;
    using Sunplus.UMControllerLibrary;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public sealed class MaintenanceFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            base.OnActionExecuting(actionContext);

            if (IsDowntime())
            {
                if (actionContext == null)
                {
                    throw new ArgumentNullException("actionContext", "Action Context cannot be null");
                }

                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Forbidden, "Service is under-maintenance.");
            }
        }

        /// <summary>
        /// Get down time information.
        /// </summary>
        /// <returns>Down time object.</returns>
        private bool IsDowntime()
        {
            if (!CustomerSettingAPIConfiguration.Settings.EnableCheckUM)
            {
                return false;
            }

            string timeFrom = string.Empty;
            string timeTo = string.Empty;
            var umctrl = new UMBusiness();

            // gets Agent Site UM Status.
            return umctrl.isUnderMaintainence(
                Sites.AccountSite,
                CustomerSettingAPIConfiguration.Settings.UMAddress,
                ref timeFrom,
                ref timeTo);
        }
    }
}