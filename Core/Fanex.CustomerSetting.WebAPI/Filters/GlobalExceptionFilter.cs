namespace Fanex.CustomerSetting.WebApi.Filters
{
    using System;
    using System.Web.Http.Filters;
    using Logging.Extensions.Web;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public sealed class GlobalExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext != null)
            {
                ExceptionHandler.Handle(actionExecutedContext.Exception);
            }
        }
    }
}