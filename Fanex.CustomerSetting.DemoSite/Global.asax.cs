namespace Fanex.CustomerSetting.DemoSite
{
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;
    using Client40;
    using MatrixClient;

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            CustomerSettingRegistration.Init();
            SettingMatrixRegistration.Init();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}