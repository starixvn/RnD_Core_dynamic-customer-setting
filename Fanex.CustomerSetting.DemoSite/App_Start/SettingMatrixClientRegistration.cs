namespace Fanex.CustomerSetting.DemoSite.AppStart
{
    using System.Web.Mvc;
    using System.Web.Routing;
    using MatrixClient;

    public class SettingMatrixClientRegistration : SettingMatrixRegistration
    {
        public override bool VerifyPermission()
        {
            return true;
        }

        public override void RegisterRouteConfig(RouteCollection routes)
        {
            routes.MapRoute(
                name: "DCSMatrix_Routing",
                url: "Matrix/ViewCustomerSettingMatrix/{action}/{id}",
                defaults: new { controller = "ViewCustomerSettingMatrix", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "Fanex.CustomerSetting.MatrixClient.Controllers" });
        }
    }
}