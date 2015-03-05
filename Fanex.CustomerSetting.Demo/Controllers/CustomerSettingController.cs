namespace Fanex.CustomerSetting.Demo.Controllers
{
    using System.Web.Mvc;
    using Fanex.CustomerSetting.Demo.UserLogin.Session;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Services;

    /// <summary>
    /// CustomerSettingController class.
    /// </summary>
    public class CustomerSettingController : Controller
    {
        #region Variables

        private readonly IDataService _dataService;
        private readonly UserContext _userContext;
        private readonly IValidationService _validationService;
        private readonly ICurrentSession _currentSession;

        #endregion Variables

        #region Constructors

        public CustomerSettingController()
            : this(null, null, null)
        {
        }

        public CustomerSettingController(IDataService service, IValidationService validationService, ICurrentSession currentSession)
        {
            _currentSession = currentSession ?? new CurrentSession();

            _userContext = new UserContext
            {
                CustId = _currentSession.CustId,
                SubAccountId = _currentSession.SubAccountId,
                RoleId = (RoleType)_currentSession.RoleId,
                Language = _currentSession.LanguageCode
            };

            _dataService = service ?? new DataService(_userContext.Language);
            _validationService = validationService ?? new ValidationService();
        }

        #endregion Constructors

        [HttpGet]
        public ViewResult Index()
        {
            return View();
        }

        /// <summary>
        /// Select customer setting.
        /// </summary>
        /// <param name="model">The select model.</param>
        /// <returns>Returns select JSON result.</returns>
        [HttpPost]
        public JsonResult Select(SelectModel model)
        {
            model.CustId = _userContext.CustId;
            model.RoleId = _userContext.RoleId;
            model.Language = _userContext.Language;
            var validationResult = _validationService.Validate(model);
            if (!validationResult.IsSuccess)
            {
                return Json(validationResult);
            }

            var selectResult = _dataService.CompanySelectSuper(model);
            return Json(selectResult);
        }

        /// <summary>
        /// Update customer setting.
        /// </summary>
        /// <param name="model">The update model.</param>
        /// <returns>Returns update JSON result.</returns>
        [HttpPost]
        public JsonResult Update(UpdateModel model)
        {
            model.CustId = _userContext.CustId;
            model.RoleId = _userContext.RoleId;
            model.Language = _userContext.Language;
            var validationResult = _validationService.Validate(model);
            if (!validationResult.IsSuccess)
            {
                return Json(validationResult);
            }

            var updateResult = _dataService.Update(model);
            return Json(Result.AccessDenied);
        }
    }
}