namespace Fanex.CustomerSetting.Site.Controllers
{
    using System.Web.Mvc;
    using System.Web.SessionState;
    using Fanex.CustomerSetting.Site.UserLogin;
    using Fanex.CustomerSetting.Web;

    /// <summary>
    /// AccountController class.
    /// </summary>
    [SessionState(SessionStateBehavior.Default)]
    public class AccountController : Controller
    {
        /// <summary>
        /// Login action.
        /// </summary>
        /// <param name="viewModel">Input value.</param>
        /// <returns>Return JSON value.</returns>
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