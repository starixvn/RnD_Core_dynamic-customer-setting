namespace Fanex.CustomerSetting.Site.Controllers
{
    using System.Web.Mvc;
    using System.Web.SessionState;
    using Fanex.CustomerSetting.Site.UserLogin;
    using Fanex.CustomerSetting.Web;

    [SessionState(SessionStateBehavior.ReadOnly)]
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
    }
}