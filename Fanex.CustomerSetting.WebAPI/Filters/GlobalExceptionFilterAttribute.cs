namespace Fanex.CustomerSetting.WebAPI.Filters
{
    using System.Net;
    using System.Net.Http;
    using System.Web.Http.Filters;
    using Fanex.CustomerSetting.Resource;
    using Fanex.Logging;

    public class GlobalExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            LogService.Error(context.Exception.Message, context.Exception);

            context.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, Message.SystemError);
        }
    }
}