namespace Fanex.CustomerSetting.DemoSite.App_Start
{
    using System.Web.Mvc;
    using System.Web.Routing;
    using Fanex.CustomerSetting.MatrixClient;

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
                url: "Matrix/{controller}/{action}/{id}",
                defaults: new { controller = "ViewCustomerSettingMatrix", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "Fanex.CustomerSetting.MatrixClient.Controllers" });
        }
    }
}