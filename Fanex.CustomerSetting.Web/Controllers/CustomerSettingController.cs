namespace Fanex.CustomerSetting.Web.Controllers
{
    using System.Web.Mvc;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Services;
    using Fanex.CustomerSetting.Web.Models;
    using Fanex.CustomerSetting.Web.Services;

    /// <summary>
    /// CustomerSettingController class.
    /// </summary>
    public class CustomerSettingController : Controller
    {
        #region Variables

        private readonly IDataService _dataService;
        private readonly UserContext _userContext;
        private readonly IValidationService _validationService;
        private readonly IModelService _modelService;

        #endregion Variables

        #region Constructors

        public CustomerSettingController()
            : this(null, null, null)
        {
        }

        public CustomerSettingController(IDataService service, IValidationService validationService, IModelService modelService)
        {
            var userService = ServiceFactory.CreateUserService();
            _userContext = userService.CreateUserContext();
            _dataService = service ?? new DataService(_userContext.Language);
            _validationService = validationService ?? new ValidationService();
            _modelService = modelService ?? new ModelService();
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
            var selectInfo = model.CreateSelectInfo(_userContext);
            var validationResult = _validationService.Validate(selectInfo);
            if (!validationResult.IsSuccess)
            {
                return Json(validationResult);
            }

            var selectResult = _dataService.CompanySelectSuper(selectInfo);
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
            var updateInfo = model.CreateUpdateInfo(_userContext, _modelService);
            var validationResult = _validationService.Validate(updateInfo);
            if (!validationResult.IsSuccess)
            {
                return Json(validationResult);
            }

            var updateResult = _dataService.Update(updateInfo);
            return Json(Result.AccessDenied);
        }
    }
}