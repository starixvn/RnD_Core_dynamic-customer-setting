namespace Fanex.CustomerSetting.Matrix.WebApi
{
    using System;
    using System.Web;
    using System.Web.Http;
    using Data.Configuration;
    using Logging.Extensions.Database;
    using Logging.Extensions.Web;

    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            DbSettingProviderManager.Current.Start();
            ExceptionHandler.UseGetDbExceptionInfo(DatabaseInfoGetters.GetDbExceptionInfo);
        }

        /// <summary>
        /// Application error event handler.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event arguments.</param>
        protected void Application_Error(object sender, EventArgs e)
        {
            ExceptionHandler.Handle(Server.GetLastError());
        }
    }
}