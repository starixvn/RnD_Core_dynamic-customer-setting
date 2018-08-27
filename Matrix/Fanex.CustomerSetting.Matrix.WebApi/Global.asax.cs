namespace Fanex.CustomerSetting.Matrix.WebApi
{
    using System;
    using System.Web;
    using System.Web.Http;
    using Configurations;
    using Data.Configuration;
    using Logging.Extensions.Database;
    using Logging.Extensions.Web;
    using RemotingRef;

    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            DbSettingProviderManager.Current.Start();
            ExceptionHandler.UseGetDbExceptionInfo(DatabaseInfoGetters.GetDbExceptionInfo);

            RefDataTypeContainer
                .ConnectTo(new Uri(SettingMatrixApiConfiguration.Settings.FanexRemotingRefService))
                .FromApplication(SettingMatrixApiConfiguration.Settings.InstanceName);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            ExceptionHandler.Handle(Server.GetLastError());
        }
    }
}