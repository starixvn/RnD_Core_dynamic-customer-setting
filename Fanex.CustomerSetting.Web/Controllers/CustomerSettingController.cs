namespace Fanex.CustomerSetting.Web.Controllers
{
    using System.Web.Mvc;

    public class CustomerSettingController : Controller
    {
        [HttpGet]
        [OutputCache(Duration = 14400)]
        public ActionResult Index()
        {
            return View();
        }
    }
}