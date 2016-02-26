namespace Fanex.CustomerSetting.Matrix.WebApi.Filters
{
    using System.Web.Http.Controllers;
    using System.Web.Http.Filters;
    using Fanex.CustomerSetting.Matrix.WebApi.Extensions;
    using Fanex.Logging;

    public class LogInfoFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            base.OnActionExecuting(actionContext);
            var controllerName = actionContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            var actionName = actionContext.ActionDescriptor.ActionName;
            var message = string.Join(
                "\r\n",
                string.Format("Controller: {0}, Action: {1} is executing.", controllerName, actionName),
                "Client IP: " + actionContext.Request.GetClientIPAddress(),
                "Url: " + actionContext.Request.RequestUri.OriginalString,
                "Body: " + actionContext.Request.GetBodyForm(),
                string.Empty);

            LogService.Info(message);
        }
    }
}