namespace Fanex.CustomerSetting.Matrix.WebApi
{
    using System.Web.Http;
    using Fanex.Data.Configuration;

    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            DbSettingProviderManager.Current.Start();
        }
    }
}