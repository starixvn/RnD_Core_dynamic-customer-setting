namespace Fanex.CustomerSetting.Web.Controllers
{
    using System;
    using System.Web.Mvc;
    using System.Web.SessionState;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.FormTypes;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Services;
    using Fanex.CustomerSetting.Web.Models;
    using Fanex.CustomerSetting.Web.Mvc;
    using Fanex.CustomerSetting.Web.Services;

    [SessionState(SessionStateBehavior.ReadOnly)]
    public class CustomerSettingController : Controller
    {
        #region Variables

        private readonly IQueryService _queryService;
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
            _queryService = queryService ?? new QueryService("en-US");
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
        public ActionResult Query(QueryModel model)
        {
            if (_userContext == null)
            {
                return Json(Result.AccessDenied);
            }

            var queryInfo = model.CreateQueryInfo(_userContext);
            Result result = _validationService.Validate(queryInfo);
            if (!result.IsSuccess)
            {
                return Json(result);
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
            return new JsonNetResult(commandResult);
        }

        private ActionResult CommandOnMultipleCustomer(CommandModel commandModel)
        {
            var multipleCommandInfo = commandModel.CreateMultipleCommandInfo();
            var formTypeHandler = FormHandlerFactory.Create(multipleCommandInfo.FormType);
            ICommandOnMultipleCustomerService commandOnMultipleMemberService = new CommandOnMultipleMemberService();
            ICommandOnMultipleCustomerService commandOnMultipleNotMemberService = new CommandOnMultipleNotMemberService();
            ICommandOnMultipleCustomerService commandService = formTypeHandler.IsCommandOnMember
                ? commandOnMultipleMemberService
                : commandOnMultipleNotMemberService;
            var commandResults = commandService.Do(multipleCommandInfo);
            return new JsonNetResult(commandResults);
        }
    }
}