namespace Fanex.CustomerSetting.WebApi.Filters
{
    using System;
    using System.Web.Http.Filters;
    using Fanex.CustomerSetting.Exceptions;
    using Logging.Extensions.Web;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public sealed class GlobalExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext != null)
            {
                if (actionExecutedContext.Exception.GetType() == typeof(InvalidInputException))
                {
                    ExceptionHandler.Handle(actionExecutedContext.Exception, System.Diagnostics.TraceEventType.Warning);
                }

                ExceptionHandler.Handle(actionExecutedContext.Exception);
            }
        }
    }
}