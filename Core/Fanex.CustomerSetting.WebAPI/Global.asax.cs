namespace Fanex.CustomerSetting.WebApi
{
    using System;
    using System.Data.SqlClient;
    using System.Text;
    using System.Web;
    using System.Web.Http;
    using AppStart;
    using Data.Configuration;
    using Logging;

    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            SettingServiceConfig.Run();
            DbSettingProviderManager.Current.Start();
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            // Logging
            if (Server != null)
            {
                Exception ex = Server.GetLastError().GetBaseException();
                var messageBuilder = new StringBuilder();

                // Get base exception data
                messageBuilder
                    .AppendLine(ex.Message)
                    .AppendLine($"Type: {ex.GetType()}")
                    .AppendLine($"Source: {ex.Source}")
                    .AppendLine($"Target Site: {ex.TargetSite}")
                    .AppendLine($"Stack Trace: {ex.StackTrace}");

                // Get additional exception data
                var sqlException = ex as SqlException;

                if (sqlException == null && ex.InnerException != null)
                {
                    sqlException = ex.InnerException as SqlException;
                }

                if (sqlException != null)
                {
                    messageBuilder
                    .AppendLine($"Procedure: {sqlException.Procedure}")
                    .AppendLine($"Line Number: {sqlException.LineNumber}")
                    .AppendLine($"Server: {sqlException.Server}");
                }

                LogService.Error(messageBuilder.ToString());
            }
        }
    }
}