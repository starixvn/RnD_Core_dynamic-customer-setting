namespace Fanex.CustomerSetting.Services.Impl
{
    using System;
    using Common.Helper;
    using Models;
    using Models.Requests;

    public class RouteService : IRouteService
    {
        private readonly IValidationService _validationService;
        private readonly ITaskService _taskService;
        private readonly ISettingContext _settingContext;

        public RouteService(ISettingContext settingContext)
        {
            _settingContext = settingContext;
            _validationService = new ValidationService(_settingContext);
            _taskService = new TaskService(_settingContext);
        }

        internal RouteService(ISettingContext settingContext, ITaskService taskService, IValidationService validationService)
        {
            _settingContext = settingContext;
            _validationService = validationService;
            _taskService = taskService;
        }

        public object Redirect(RedirectCommand command)
        {
            Check.Argument.IsNotNull(command, nameof(command));

            var validationResult = _validationService.ValidateQuerySettingAction(command);

            if (validationResult.IsNotOk)
            {
                throw new AccessViolationException(validationResult.Message);
            }

            _taskService.Run(command.ProductId, command.SettingTypeId, command);

            IAdditionalRequestHandler handler = AdditionalRequestHandlerFactory.GetInstance(command.SettingTypeId);
            object result = handler.Handle(_settingContext, command);

            _taskService.Run(command.ProductId, command.SettingTypeId, command, result);

            return result;
        }
    }
}