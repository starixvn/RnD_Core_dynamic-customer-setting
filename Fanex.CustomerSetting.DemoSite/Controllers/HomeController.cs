namespace Fanex.CustomerSetting.DemoSite.Controllers
{
    using System.Web.Mvc;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Web.Contexts;

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        //public ActionResult Login(
        //    int userLoginId = 8,
        //    string userLoginName = "LoginName",
        //    int subAccountId = 0,
        //    UserLevel? userLevel)
        //{
        //    SettingContextManager.Init(userLoginId, userLoginName, userLevel ?? UserLevel.Company, subAccountId);
        //    return Redirect("~/BackendTest/Index");
        //}
    }
}