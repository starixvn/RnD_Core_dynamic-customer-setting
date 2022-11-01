namespace Fanex.CustomerSetting.DemoSite
{
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;
    using Client40;
    using MatrixClient;
    using Fanex.Logging.Extensions.Database;
    using System.Net;

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Fanex.CustomerSetting.DemoSite.LoggingConfig.LoggingConfig.Init(new[] { "pass", "pwd", "security", "Password" }, getDbExceptionInfo: DatabaseInfoGetters.GetDbExceptionInfo);
            AreaRegistration.RegisterAllAreas();

            CustomerSettingRegistration.Init();
            SettingMatrixRegistration.Init();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

        }
    }
}