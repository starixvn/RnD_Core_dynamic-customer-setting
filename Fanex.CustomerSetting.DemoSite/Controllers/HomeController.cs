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
    }
}