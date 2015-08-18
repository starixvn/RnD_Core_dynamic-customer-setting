namespace Fanex.CustomerSetting.WebAPI
{
    using System.Web.Http;

    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            Fanex.Data.Configuration.DbSettingProviderManager.Current.Start();
        }
    }
}
