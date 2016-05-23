namespace Fanex.CustomerSetting.WebApi.Filters
{
    using System;
    using System.Data.SqlClient;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading;
    using System.Web.Http.Filters;
    using Data;
    using Helpers;
    using Logging;
    using Resource;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public sealed class GlobalExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext != null)
            {
                var exception = actionExecutedContext.Exception;
                var messageBuilder = new StringBuilder();

                messageBuilder.AppendFormat("Message: {0}", exception.Message).AppendLine();
                messageBuilder.AppendFormat("Type: {0}", exception.GetType().ToString()).AppendLine();
                messageBuilder.AppendFormat("Source: {0}", exception.Source).AppendLine();
                messageBuilder.AppendFormat("Client IP: {0}", actionExecutedContext.Request.GetClientIPAddress()).AppendLine();
                messageBuilder.AppendFormat("Culture: {0}", Thread.CurrentThread.CurrentUICulture).AppendLine();
                messageBuilder.AppendFormat("Url: {0}", actionExecutedContext.Request.RequestUri.OriginalString).AppendLine();
                messageBuilder.AppendFormat("Body: {0}", actionExecutedContext.Request.GetBodyForm()).AppendLine();

                LogDatabaseException(exception, messageBuilder);

                LogService.Error(messageBuilder.ToString(), actionExecutedContext.Exception);

                actionExecutedContext.Response = actionExecutedContext.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, Message.SystemError);
            }
        }

        private void LogDatabaseException(Exception exception, StringBuilder messageBuilder)
        {
            var objectDbException = exception as ObjectDbException;

            if (objectDbException != null)
            {
                messageBuilder.AppendFormat("Procedure: {0}", objectDbException.DbSetting.SpName).AppendLine();
                messageBuilder.AppendFormat("DB Name: {0}", objectDbException.DbSetting.DbName).AppendLine();

                if (objectDbException.InnerException is SqlException)
                {
                    var innerSqlException = objectDbException.InnerException as SqlException;
                    messageBuilder.AppendFormat("Server: {0}", innerSqlException.Server).AppendLine();
                    messageBuilder.AppendFormat("Line Number: {0}", innerSqlException.LineNumber).AppendLine();
                }
            }
        }
    }
}