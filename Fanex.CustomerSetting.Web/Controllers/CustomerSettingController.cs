namespace Fanex.CustomerSetting.Web.Controllers
{
    using System.Web.Mvc;
    using Fanex.CustomerSetting.Enumerations;
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

        private readonly ICustomerSettingSelectService _selectService;
        private readonly ICustomerSettingUpdateService _updateService;
        private readonly UserContext _userContext;
        private readonly IValidationService _validationService;
        private readonly IModelService _modelService;

        #endregion Variables

        #region Constructors

        public CustomerSettingController()
            : this(null, null, null, null)
        {
        }

        public CustomerSettingController(
            ICustomerSettingSelectService selectService,
            ICustomerSettingUpdateService updateService,
            IValidationService validationService,
            IModelService modelService)
        {
            var userService = ServiceFactory.CreateUserService();
            _userContext = userService.CreateUserContext();
            _selectService = selectService ?? new CustomerSettingSelectService(_userContext.Language);
            _updateService = updateService ?? new CustomerSettingUpdateService(_userContext.Language);
            _validationService = validationService ?? new ValidationService();
            _modelService = modelService ?? new ModelService();
        }

        #endregion Constructors

        [HttpGet]
        public FileResult Index()
        {
#if (DEBUG)
            return File("/app/customerSetting/customer-setting-page.debug.htm", "text/HTML");
#else
            return File("/app/customerSetting/customer-setting-page.htm", "text/HTML");
#endif
        }

        /// <summary>
        /// Select customer setting.
        /// </summary>
        /// <param name="model">The select model.</param>
        /// <returns>Returns select JSON result.</returns>
        [HttpPost]
        public JsonResult Select(SelectModel model)
        {
            if (_userContext == null)
            {
                return Json(Result.AccessDenied);
            }

            var selectInfo = model.CreateSelectInfo(_userContext);
            var validationResult = _validationService.Validate(selectInfo);
            if (!validationResult.IsSuccess)
            {
                return Json(validationResult);
            }

            switch (model.FormType)
            {
                case FormType.CompanyEditSuper:
                    return Json(_selectService.CompanySelectSuper(selectInfo));

                case FormType.SuperEditMaster:
                    return Json(_selectService.SuperSelectMaster(selectInfo));

                case FormType.MasterEditAgent:
                    return Json(_selectService.MasterSelectAgent(selectInfo));

                case FormType.SuperEditMember:
                    return Json(_selectService.SuperSelectMember(selectInfo));

                case FormType.MasterEditMember:
                    return Json(_selectService.MasterSelectMember(selectInfo));

                case FormType.AgentEditMember:
                    return Json(_selectService.AgentSelectMember(selectInfo));
            }

            return Json(Result.Error);
        }

        /// <summary>
        /// Update customer setting.
        /// </summary>
        /// <param name="model">The update model.</param>
        /// <returns>Returns update JSON result.</returns>
        [HttpPost]
        public JsonResult Update(UpdateModel model)
        {
            if (_userContext == null)
            {
                return Json(Result.AccessDenied);
            }

            var updateInfo = model.CreateUpdateInfo(_userContext, _modelService);
            var validationResult = _validationService.Validate(updateInfo);
            if (!validationResult.IsSuccess)
            {
                return Json(validationResult);
            }

            return Json(_updateService.Update(updateInfo));
        }
    }
}