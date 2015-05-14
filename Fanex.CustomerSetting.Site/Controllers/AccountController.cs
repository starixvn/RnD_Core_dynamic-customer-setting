namespace Fanex.CustomerSetting.Site.Controllers
{
    using System.Web.Mvc;
    using System.Web.SessionState;
    using Fanex.CustomerSetting.Site.UserLogin;
    using Fanex.CustomerSetting.Web;

    [SessionState(SessionStateBehavior.Default)]
    public class AccountController : Controller
    {
        [HttpPost]
        public ActionResult Login(LoginViewModel viewModel)
        {
            new CurrentSession
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