namespace Fanex.CustomerSetting.Matrix.WebApi.Filters
{
    using System.Net;
    using System.Net.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Filters;
    using Fanex.CustomerSetting.Matrix.WebApi.Configurations;
    using Sunplus.UMControllerLibrary;

    public class MaintenanceFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            base.OnActionExecuting(actionContext);

            if (IsDowntime())
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Service is under-maintenance.");
            }
        }

        /// <summary>
        /// Get down time information.
        /// </summary>
        /// <returns>Down time object.</returns>
        private bool IsDowntime()
        {
            if (!SettingMatrixAPIConfiguration.Settings.EnableCheckUM)
            {
                return false;
            }

            string timeFrom = string.Empty;
            string timeTo = string.Empty;
            var umctrl = new UMBusiness();

            // gets Agent Site UM Status.
            return umctrl.isUnderMaintainence(
                Sites.AccountSite,
                SettingMatrixAPIConfiguration.Settings.UMAddress,
                ref timeFrom,
                ref timeTo);
        }
    }
}