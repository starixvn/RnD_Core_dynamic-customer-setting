namespace Fanex.CustomerSetting.DemoSite.Controllers
{
    using System.Web.Mvc;
    using Fanex.CustomerSetting.Dtos.Results;
    using Fanex.CustomerSetting.Enumerations;
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
            int subAccountId = 0,
            string language = "en-US")
        {
            SettingContextManager.Init(actorId, actorname, Level.FromValue<Level>(role));

            return Json(Result.Success);
        }
    }
}