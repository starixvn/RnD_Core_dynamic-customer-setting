namespace Fanex.CustomerSetting.WebApi
{
    using System;
    using System.Web;
    using System.Web.Http;
    using AppStart;
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
            SettingServiceConfig.Run();
            DbSettingProviderManager.Current.Start();
            ExceptionHandler.UseGetDbExceptionInfo(DatabaseInfoGetters.GetDbExceptionInfo);

            RefDataTypeContainer
                .ConnectTo(new Uri(CustomerSettingApiConfiguration.Settings.FanexRemotingRefService))
                .FromApplication(CustomerSettingApiConfiguration.Settings.InstanceName)
                .Register(RefDataType.BetTypeName, new RefDataTypeHandlerOptions { SlidingTimeout = TimeSpan.MaxValue })
                .Register(RefDataType.SportName, new RefDataTypeHandlerOptions { SlidingTimeout = TimeSpan.MaxValue })
                .Register(RefDataType.RefResource, new RefDataTypeHandlerOptions { SlidingTimeout = TimeSpan.MaxValue });
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            ExceptionHandler.Handle(Server.GetLastError());
        }
    }
}