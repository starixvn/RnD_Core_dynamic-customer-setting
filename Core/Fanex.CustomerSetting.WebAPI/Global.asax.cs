namespace Fanex.CustomerSetting.WebApi
{
    using System;
    using System.Web;
    using System.Web.Http;
    using AppStart;
    using Data.Configuration;
    using Logging.Extensions.Database;
    using Logging.Extensions.Web;

    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            SettingServiceConfig.Run();
            DbSettingProviderManager.Current.Start();
            ExceptionHandler.UseGetDbExceptionInfo(DatabaseInfoGetters.GetDbExceptionInfo);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            ExceptionHandler.Handle(Server.GetLastError());
        }
    }
}