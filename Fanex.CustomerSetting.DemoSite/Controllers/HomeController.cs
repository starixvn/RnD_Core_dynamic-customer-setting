namespace Fanex.CustomerSetting.DemoSite.Controllers
{
    using System.Web.Mvc;
    using System.Web.Security;
    using Fanex.CustomerSetting.Client40.Configurations;
    using Fanex.CustomerSetting.DemoSite.App_Start;
    using Fanex.CustomerSetting.DemoSite.Constants;

    public class HomeController : Controller
    {
        private const byte AgencySystem = 2;

        [RestrictAccessFilter]
        public ActionResult Index()
        {
            FormsAuthentication.SetAuthCookie("DemoName", true);
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
            byte system = 1,
            string language = "en-US")
        {
            if (AgencySystem != CustomerSettingConfiguration.Settings.System)
            {
                subAccountId = 0;
            }

            Session[SessionKeys.ActorIdSessionKey] = actorId;
            Session[SessionKeys.ActorNameSessionKey] = actorName;
            Session[SessionKeys.ActorLevelSessionKey] = role;
            Session[SessionKeys.SubAccountIdSessionKey] = subAccountId;
            Session[SessionKeys.SystemIdSessionKey] = system;
            Session[SessionKeys.LanguageSessionKey] = language;

            return Json(new
                {
                    Code = 0,
                    Message = string.Empty,
                    IsSuccessful = true
                });
        }

        public ActionResult RedirectAction()
        {
            return this.RedirectToAction("DemoAction", "DemoCtrl");
        }

        public ActionResult Login(string securityCode)
        {
            if (securityCode == DemoSiteConfiguration.Settings.SecurityCode)
            {
                Session["IsAuthenticated"] = true;
                return RedirectToAction("Index");
            }

            return Content("You are not authorized!");
        }
    }
}