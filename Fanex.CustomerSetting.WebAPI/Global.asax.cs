namespace Fanex.CustomerSetting.WebApi
{
    using System.Web;
    using System.Web.Http;
    using Fanex.Data.Configuration;

    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            DbSettingProviderManager.Current.Start();
        }
    }
}