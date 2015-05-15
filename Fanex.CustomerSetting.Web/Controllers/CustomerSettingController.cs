namespace Fanex.CustomerSetting.Web.Controllers
{
    using System;
    using System.Web.Mvc;
    using System.Web.SessionState;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Services;
    using Fanex.CustomerSetting.Web.Models;
    using Fanex.CustomerSetting.Web.Services;

    [SessionState(SessionStateBehavior.ReadOnly)]
    public class CustomerSettingController : Controller
    {
        #region Variables

        private readonly UserContext _userContext;
        private readonly IValidationService _validationService;
        private readonly IModelService _modelService;

        #endregion Variables

        #region Constructors

        public CustomerSettingController()
            : this(null, null, null)
        {
        }

        public CustomerSettingController(
            IQueryService queryService,
            IValidationService validationService,
            IModelService modelService)
        {
            var userService = ServiceFactory.CreateUserService();
            _userContext = userService.CreateUserContext();
            _validationService = validationService ?? new ValidationService();
            _modelService = modelService ?? new ModelService();
        }

        #endregion Constructors

        [HttpGet]
        [OutputCache(Duration = 14400)]
        public ActionResult Index()
        {
            var rootUrl = Request.ApplicationPath;
            if (rootUrl.EndsWith("/", StringComparison.CurrentCulture))
            {
                rootUrl = rootUrl.Substring(0, rootUrl.Length - 1);
            }

            var filePath = Server.MapPath("~/app/customerSetting/customer-setting-page.htm");
            var htmlContent = System.IO.File.ReadAllText(filePath);
            htmlContent = htmlContent.Replace("{{rootUrl}}", rootUrl);
            var min = ".min";
#if DEBUG
            min = string.Empty;
#endif
            htmlContent = htmlContent.Replace("{{min}}", min);
            return Content(htmlContent, "text/HTML");
        }

        [HttpPost]
        public ActionResult Query(QueryModel queryModel)
        {
            if (_userContext == null)
            {
                return Json(Result.AccessDenied);
            }

            var queryInfo = queryModel.CreateQueryInfo(_userContext);
            IQueryService queryService = new QueryService();
            var queryResult = queryService.Do(queryInfo);
            return Json(queryResult);
        }

        public ActionResult Command(CommandModel commandModel)
        {
            if (_userContext == null || !_userContext.HasCommandPermission)
            {
                return Json(Result.AccessDenied);
            }

            if (commandModel.IsMultiple)
            {
                return CommandOnMultipleCustomer(commandModel);
            }
            else
            {
                return CommandOnCustomer(commandModel);
            }
        }

        private ActionResult CommandOnCustomer(CommandModel commandModel)
        {
            var commandInfo = commandModel.CreateCommandInfo();
            ICommandOnCustomerService commandService = new CommandOnCustomerService();
            CommandResult commandResult = commandService.Do(commandInfo);
            return Json(commandResult);
        }

        private ActionResult CommandOnMultipleCustomer(CommandModel commandModel)
        {
            var multipleCommandInfo = commandModel.CreateMultipleCommandInfo();
            ICommandOnMultipleCustomerService commandService = new CommandOnMultipleCustomerService();
            var commandResults = commandService.Do(multipleCommandInfo);
            return Json(commandResults);
        }
    }
}