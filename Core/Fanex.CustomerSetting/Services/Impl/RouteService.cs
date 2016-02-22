namespace Fanex.CustomerSetting.Services.Impl
{
    using Fanex.Common.Helper;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Models.Requests;

    public class RouteService : IRouteService
    {
        private ISettingContext _settingContext;

        public RouteService(ISettingContext settingContext)
        {
            _settingContext = settingContext;
        }

        public object Redirect(RedirectCommand command)
        {
            Check.Argument.IsNotNull(command, nameof(command));

            IAdditionalRequestHandler handler = AdditionalRequestHandlerFactory.GetInstance(command.SettingTypeId);
            object result = handler.Handle(_settingContext, command.ActionId, command.ActionType, command.JsonParameters);

            return result;
        }
    }
}