using System.Web.Mvc;

namespace Fanex.CustomerSetting.Site.Areas.CustomerSetting
{
    public class CustomerSettingAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "CustomerSetting";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "CustomerSetting_default",
                "CustomerSetting/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
