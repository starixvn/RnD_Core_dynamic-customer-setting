namespace Fanex.CustomerSetting.DemoSite
{
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Models.Enumerations;
    using Fanex.CustomerSetting.Web;
    using Fanex.Data.Configuration;

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            DbSettingProviderManager.Current.Start();
            ModelBinders.Binders.Add(typeof(ISetting), new SettingModelBinder());
            ModelBinders.Binders.Add(typeof(SettingType), new SettingItemModelBinder());
            ModelBinders.Binders.Add(typeof(ISettingItem), new SettingItemModelBinder());
            ModelBinders.Binders.Add(typeof(Level), new LevelModelBinder());
        }
    }
}