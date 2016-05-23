namespace Fanex.CustomerSetting.WebApi.Filters
{
    using System.Web.Http.Controllers;
    using System.Web.Http.Filters;
    using Helpers;
    using Logging;

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