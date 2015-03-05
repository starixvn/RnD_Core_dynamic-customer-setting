namespace Fanex.CustomerSetting.Demo.Controllers
{
    using System.Web.Mvc;
    using Fanex.CustomerSetting.Demo.UserLogin;

    /// <summary>
    /// HomeController class.
    /// </summary>
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var viewModel = new LoginViewModels();
            return View(viewModel);
        }

        public ActionResult Welcome()
        {
            return View();
        }
    }
}