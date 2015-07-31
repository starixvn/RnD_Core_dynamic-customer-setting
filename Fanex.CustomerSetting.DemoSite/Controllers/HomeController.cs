namespace Fanex.CustomerSetting.DemoSite.Controllers
{
    using System.Web.Mvc;
    using Fanex.CustomerSetting.Models.Enumerations;
    using Fanex.CustomerSetting.Models.Results;
    using Fanex.CustomerSetting.Web.Contexts;

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult SaveSession(
            int actorId,
            string actorname,
            byte role,
            byte system = (byte)SystemType.Accounting,
            int subAccountId = 0,
            string language = "en-US")
        {
            if (system != (byte)SystemType.Agency)
            {
                subAccountId = 0;
            }

            SettingContextManager.Init(actorId, actorname, role, system, subAccountId);
            SettingContextManager.SetLanguage(language);

            return Json(Result.Success);
        }
    }
}