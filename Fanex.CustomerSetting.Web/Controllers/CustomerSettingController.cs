namespace Fanex.CustomerSetting.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using System.Web.SessionState;
    using Fanex.CustomerSetting.CommandTypes;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.FormTypes;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Services;
    using Fanex.CustomerSetting.Web.Models;
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
            _queryService = queryService ?? new QueryService(_userContext.Language);
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
        public JsonResult Query(QueryModel model)
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

        public JsonResult Command(CommandModel model)
        {
            if (_userContext == null || !_userContext.HasCommandPermission)
            {
                return Json(Result.AccessDenied);
            }

            var commandInfo = model.CreateCommandInfo(_userContext, _modelService);
            var multipleCommandInfo = commandInfo as MultipleCommandInfo;
            if (multipleCommandInfo != null)
            {
                return CommandOnCustomers(multipleCommandInfo);
            }

            return CommandOnCustomer(commandInfo);
        }

        private JsonResult CommandOnCustomer(CommandInfo commandInfo)
        {
            CommandResult commandResult = null;
            Result validationResult = _validationService.Validate(commandInfo);
            if (validationResult.IsSuccess)
            {
                ICommand command = CommandFactory.Get(commandInfo.CommandType);
                commandResult = command.Do(commandInfo);
            }
            else
            {
                commandResult = new CommandResult(commandInfo.CommandType, validationResult);
            }

            return Json(commandResult);
        }

        private JsonResult CommandOnCustomers(MultipleCommandInfo multipleCommandInfo)
        {
            var formTypeHandler = FormHandlerFactory.Create(multipleCommandInfo.FormType);
            if (formTypeHandler.IsCommandOnMember)
            {
                return CommandOnMembers(multipleCommandInfo);
            }

            return CommandOnNotMembers(multipleCommandInfo);
        }

        private JsonResult CommandOnNotMembers(MultipleCommandInfo multipleCommandInfo)
        {
            var commandResults = new List<CommandResult>();
            while (true)
            {
                var commandInfo = multipleCommandInfo.Take();
                if (commandInfo == null)
                {
                    break;
                }

                CommandResult commandResult = null;
                Result validationResult = _validationService.Validate(commandInfo);
                if (validationResult.IsSuccess)
                {
                    ICommand command = CommandFactory.Get(commandInfo.CommandType);
                    commandResult = command.Do(commandInfo);
                }
                else
                {
                    commandResult = new CommandResult(commandInfo.CommandType, validationResult);
                }

                commandResults.Add(commandResult);
            }

            return Json(commandResults);
        }

        private JsonResult CommandOnMembers(MultipleCommandInfo multipleCommandInfo)
        {
            ICommandOnMultipleCustomerService commandOnMembersService = new CommandOnMultipleMemberService();
            var commandResults = commandOnMembersService.Do(multipleCommandInfo);
            return Json(commandResults);
        }
    }
}