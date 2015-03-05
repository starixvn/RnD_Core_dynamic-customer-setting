namespace Fanex.CustomerSetting.Demo.Controllers
{
    using System.Web.Mvc;
    using Fanex.CustomerSetting.Demo.UserLogin;
    using Fanex.CustomerSetting.Demo.UserLogin.Session;

    /// <summary>
    /// HomeController class.
    /// </summary>
    public class HomeController : Controller
    {
        private readonly ICurrentSession _currentSession;

        public HomeController()
            : this(null)
        {
        }

        public HomeController(ICurrentSession currentSession)
        {
            _currentSession = currentSession ?? new CurrentSession();
        }

        /// <summary>
        /// Index page.
        /// </summary>
        /// <returns>Returns HTML index page.</returns>
        public ActionResult Index()
        {
            var viewModel = new LoginViewModel
            {
                RoleId = _currentSession.RoleId,
                CustId = _currentSession.CustId,
                SubAccountId = _currentSession.SubAccountId,
                LanguageCode = _currentSession.LanguageCode,
                HideForm = _currentSession.CustId > 0
            };
            return View(viewModel);
        }

        public ActionResult Welcome()
        {
            return View();
        }

        /// <summary>
        /// Login action.
        /// </summary>
        /// <param name="viewModel">Input value.</param>
        /// <returns>Return JSON value.</returns>
        [HttpPost]
        public ActionResult Login(LoginViewModel viewModel)
        {
            var currentSession = new CurrentSession
            {
                RoleId = viewModel.RoleId,
                CustId = viewModel.CustId,
                SubAccountId = viewModel.SubAccountId,
                LanguageCode = viewModel.LanguageCode
            };

            return Json(1);
        }
    }
}