namespace Fanex.CustomerSetting.Web.Controllers
{
    using System;
    using System.Web.Mvc;
    using System.Web.SessionState;
    using Fanex.CustomerSetting.Core;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Resources;
    using Fanex.CustomerSetting.Services;
    using Fanex.CustomerSetting.Web.Models;
    using Fanex.CustomerSetting.Web.Services;

    [SessionState(SessionStateBehavior.ReadOnly)]
    public class CustomerSettingController : Controller
    {
        private readonly IUserService _userService;
        private readonly IQueryService _queryService;
        private readonly ICommandOnCustomerService _commandOnCustomerService;
        private readonly ICommandOnMultipleCustomerService _commandOnMultipleCustomerService;

        public CustomerSettingController()
            : this(null, null, null, null)
        {
        }

        public CustomerSettingController(
            IUserService userService,
            IQueryService queryService,
            ICommandOnCustomerService commandOnCustomerService,
            ICommandOnMultipleCustomerService commandOnMultipleCustomerService)
        {
            _userService = userService ?? new UserService();
            var userContext = _userService.CreateUserContext();
            LanguageManager.CurrentLanguage = userContext != null ? userContext.Language : Configuration.DefaultLanguage;
            _queryService = queryService ?? new QueryService();
            _commandOnCustomerService = commandOnCustomerService ?? new CommandOnCustomerService();
            _commandOnMultipleCustomerService = commandOnMultipleCustomerService ?? new CommandOnMultipleCustomerService();
        }

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
            QueryInfo queryInfo;
            try
            {
                queryInfo = queryModel.CreateQueryInfo();
            }
            catch (UnauthorizedAccessException)
            {
                return Json(Result.AccessDenied);
            }

            var queryResult = _queryService.Execute(queryInfo);
            return Json(queryResult);
        }

        public ActionResult Command(CommandModel commandModel)
        {
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
            CommandInfo commandInfo;
            try
            {
                commandInfo = commandModel.CreateCommandInfo();
            }
            catch (UnauthorizedAccessException)
            {
                return Json(Result.AccessDenied);
            }

            CommandResult commandResult = _commandOnCustomerService.Execute(commandInfo);
            return Json(commandResult);
        }

        private ActionResult CommandOnMultipleCustomer(CommandModel commandModel)
        {
            MultipleCommandInfo multipleCommandInfo;
            try
            {
                multipleCommandInfo = commandModel.CreateMultipleCommandInfo();
            }
            catch (UnauthorizedAccessException)
            {
                return Json(Result.AccessDenied);
            }

            var commandResults = _commandOnMultipleCustomerService.Execute(multipleCommandInfo);
            return Json(commandResults);
        }
    }
}