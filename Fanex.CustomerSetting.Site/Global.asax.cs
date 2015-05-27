namespace Fanex.CustomerSetting.Site
{
    using System.Web;
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Fanex.CustomerSetting.Web.Configuration;
    using Fanex.Data.Configuration;

    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            DbSettingProviderManager.Current.Start();
            var isMinifiedVersion = false;
#if DEBUG
            isMinifiedVersion = true;
#endif
            var webSettings = new WebSettings
            {
                IsMinifiedVersion = isMinifiedVersion
            };
            WebConfiguration.Start(webSettings);
        }
    }
}