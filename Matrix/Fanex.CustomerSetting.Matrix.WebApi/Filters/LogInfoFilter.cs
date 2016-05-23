namespace Fanex.CustomerSetting.Matrix.WebApi.Filters
{
    using System;
    using System.Text;
    using System.Web.Http.Controllers;
    using System.Web.Http.Filters;
    using Extensions;
    using Logging;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public sealed class LogInfoFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            base.OnActionExecuting(actionContext);
            var controllerName = actionContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            var actionName = actionContext.ActionDescriptor.ActionName;
            var messageBuilder = new StringBuilder();

            messageBuilder
                    .AppendLine($"Controller: {controllerName}, action: {actionName} is executing.")
                    .AppendLine($"Client IP: {actionContext.Request.GetClientIPAddress()}")
                    .AppendLine($"Url: {actionContext.Request.RequestUri.OriginalString}")
                    .AppendLine($"Body: {actionContext.Request.GetBodyForm()}")
                    .AppendLine();

            LogService.Info(messageBuilder.ToString());
        }
    }
}