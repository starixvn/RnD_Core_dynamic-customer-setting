namespace Fanex.CustomerSetting.WebApi
{
    using System;
    using System.Data.SqlClient;
    using System.Diagnostics.CodeAnalysis;
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

        /// <summary>
        /// Application error event handler.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event arguments.</param>
        [SuppressMessage("Microsoft.Globalization",
            "CA1303:Do not pass literals as localized parameters",
            MessageId = "Fanex.Logging.LogService.Error(System.String)",
            Justification = "Reviewed!")]
        protected void Application_Error(object sender, EventArgs e)
        {
            // Logging
            if (Server != null)
            {
                Exception ex = Server.GetLastError().GetBaseException();

                // Get base exception data
                string message = ex.Message +
                    "\r\nType: " + ex.GetType().ToString() +
                    "\r\nSource: " + ex.Source +
                    "\r\nTarget Site: " + ex.TargetSite +
                    "\r\nStack Trace: " + ex.StackTrace;

                // Get additional exception data
                var sqlException = ex as SqlException;

                if (sqlException == null && ex.InnerException != null)
                {
                    sqlException = ex.InnerException as SqlException;
                }

                if (sqlException != null)
                {
                    message += "\r\nProcedure: " + sqlException.Procedure +
                        "\r\nLine Number: " + sqlException.LineNumber +
                        "\r\nServer: " + sqlException.Server;
                }

                LogService.Error(message);
            }
        }
    }
}