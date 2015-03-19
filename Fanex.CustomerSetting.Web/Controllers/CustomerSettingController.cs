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

        private readonly ICustomerSettingQueryService _queryService;
        private readonly ICustomerSettingCommandService _commandService;
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
            ICustomerSettingQueryService queryService,
            ICustomerSettingCommandService commandService,
            IValidationService validationService,
            IModelService modelService)
        {
            var userService = ServiceFactory.CreateUserService();
            _userContext = userService.CreateUserContext();
            _queryService = queryService ?? new CustomerSettingQueryService(_userContext.Language);
            _commandService = commandService ?? new CustomerSettingCommandService(_userContext.Language);
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
        public JsonResult Select(QueryModel model)
        {
            if (_userContext == null)
            {
                return Json(Result.AccessDenied);
            }

            var queryInfo = model.CreateQueryInfo(_userContext);
            var validationResult = _validationService.Validate(queryInfo);
            if (!validationResult.IsSuccess)
            {
                return Json(validationResult);
            }

            switch (model.FormType)
            {
                case FormType.CompanyEditSuper:
                    return Json(_queryService.CompanyQuerySuper(queryInfo));

                case FormType.SuperEditMaster:
                    return Json(_queryService.SuperQueryMaster(queryInfo));

                case FormType.MasterEditAgent:
                    return Json(_queryService.MasterQueryAgent(queryInfo));

                case FormType.SuperEditMember:
                    return Json(_queryService.SuperQueryMember(queryInfo));

                case FormType.MasterEditMember:
                    return Json(_queryService.MasterQueryMember(queryInfo));

                case FormType.AgentEditMember:
                    return Json(_queryService.AgentQueryMember(queryInfo));
            }

            return Json(Result.Error);
        }

        /// <summary>
        /// Update customer setting.
        /// </summary>
        /// <param name="model">The update model.</param>
        /// <returns>Returns update JSON result.</returns>
        [HttpPost]
        public JsonResult Update(CommandModel model)
        {
            if (_userContext == null)
            {
                return Json(Result.AccessDenied);
            }

            var commandInfo = model.CreateCommandInfo(_userContext, _modelService);
            var validationResult = _validationService.Validate(commandInfo);
            if (!validationResult.IsSuccess)
            {
                return Json(validationResult);
            }

            return Json(_commandService.Update(commandInfo));
        }
    }
}