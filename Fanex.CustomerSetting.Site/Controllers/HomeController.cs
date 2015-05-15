namespace Fanex.CustomerSetting.Site.Controllers
{
    using System.Web.Mvc;
    using System.Web.SessionState;
    using Fanex.CustomerSetting.Site.UserLogin;
    using Fanex.CustomerSetting.Web;

    [SessionState(SessionStateBehavior.ReadOnly)]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var currentSession = SessionManager.CurrentSession;
            if (currentSession == null)
            {
                currentSession = new CurrentSession();
            }

            var viewModel = new LoginViewModel
            {
                RoleId = currentSession.RoleId,
                CustId = currentSession.CustId,
                SubAccountId = currentSession.SubAccountId,
                LanguageCode = currentSession.LanguageCode,
                HideForm = currentSession.CustId > 0
            };

            return View(viewModel);
        }

        public ActionResult Welcome()
        {
            return View();
        }
    }
}