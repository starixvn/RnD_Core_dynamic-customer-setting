namespace Fanex.CustomerSetting.WebAPI.Filters
{
    using System;
    using System.Data.SqlClient;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Web.Http.Filters;
    using Fanex.CustomerSetting.Resource;
    using Fanex.Data;
    using Fanex.Logging;

    public class GlobalExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            var exception = context.Exception;
            var messageBuilder = new StringBuilder();

            messageBuilder.AppendFormat("Message: {0}", exception.Message).AppendLine();
            messageBuilder.AppendFormat("Type: {0}", exception.GetType().ToString()).AppendLine();
            messageBuilder.AppendFormat("Source: {0}", exception.Source).AppendLine();
            messageBuilder.AppendFormat("Domain: {0}", context.Request.Headers.Host).AppendLine();
            messageBuilder.AppendFormat("Query String:: {0}", context.Request.RequestUri.ToString()).AppendLine();

            LogDatabaseException(exception, messageBuilder);

            LogService.Error(messageBuilder.ToString(), context.Exception);

            context.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, Message.SystemError);
        }

        private void LogDatabaseException(Exception exception, StringBuilder messageBuilder)
        {
            if (exception is ObjectDbException)
            {
                var objectDbException = exception as ObjectDbException;
                messageBuilder.AppendFormat("Procedure: {0}", objectDbException.DbSetting.SpName).AppendLine();
                messageBuilder.AppendFormat("Connection String: {0}", objectDbException.DbSetting.ConnectionString).AppendLine();

                if (objectDbException.InnerException is SqlException)
                {
                    var innerSqlException = objectDbException.InnerException as SqlException;
                    messageBuilder.AppendFormat("Line Number: {0}", innerSqlException.LineNumber).AppendLine();
                }
            }
        }
    }
}