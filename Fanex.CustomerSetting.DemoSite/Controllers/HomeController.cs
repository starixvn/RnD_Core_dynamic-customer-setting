namespace Fanex.CustomerSetting.DemoSite.Controllers
{
    using System.Web.Mvc;
    using Fanex.CustomerSetting.Client40.Configurations;
    using Fanex.CustomerSetting.Client40.Context;

    public class HomeController : Controller
    {
        public const byte AgencySystem = 2;

        public ActionResult Index()
        {
            if (this.Request.Browser.Browser == "IE" && (this.Request.Browser.MajorVersion < 9))
            {
                return View("Index-IE8");
            }
            else
            {
                return View();
            }
        }

        public JsonResult SaveSession(
            int actorId,
            string actorName,
            byte role,
            int subAccountId = 0,
            string language = "en-US")
        {
            if (AgencySystem != CustomerSettingConfiguration.Settings.System)
            {
                subAccountId = 0;
            }

            SettingContextManager.Register(() =>
            {
                var settingContext = new SettingContext();

                settingContext.ActorId = actorId;
                settingContext.ActorName = actorName;
                settingContext.ActorLevel = role;
                settingContext.SubAccountId = subAccountId;

                return settingContext;
            });

            return Json(new
                {
                    Code = 0,
                    Message = string.Empty,
                    IsSuccessful = true
                });
        }
    }
}