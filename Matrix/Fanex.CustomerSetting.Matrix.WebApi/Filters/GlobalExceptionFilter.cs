namespace Fanex.CustomerSetting.Matrix.WebApi.Filters
{
    using System;
    using System.Data.SqlClient;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading;
    using System.Web.Http.Filters;
    using Data;
    using Extensions;
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

                messageBuilder.AppendLine($"Message: {exception.Message}");
                messageBuilder.AppendLine($"Type: {exception.GetType()}");
                messageBuilder.AppendLine($"Source: {exception.Source}");
                messageBuilder.AppendLine($"Client IP: {actionExecutedContext.Request.GetClientIPAddress()}");
                messageBuilder.AppendLine($"Culture: {Thread.CurrentThread.CurrentUICulture}");
                messageBuilder.AppendLine($"Url: {actionExecutedContext.Request.RequestUri.OriginalString}");
                messageBuilder.AppendLine($"Body: {actionExecutedContext.Request.GetBodyForm()}");

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
                var innerSqlException = objectDbException.InnerException as SqlException;

                messageBuilder.AppendLine($"Procedure: {objectDbException.DbSetting.SpName}");
                messageBuilder.AppendLine($"DB Name: {objectDbException.DbSetting.DbName}");

                if (innerSqlException != null)
                {
                    messageBuilder.AppendLine($"Server: {innerSqlException.Server}");
                    messageBuilder.AppendLine($"Line Number: {innerSqlException.LineNumber}");
                }
            }
        }
    }
}