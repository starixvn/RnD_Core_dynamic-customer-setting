namespace Fanex.CustomerSetting.Services.Impl
{
    using Fanex.CustomerSetting.Models.Enumerations;

    public class RouteService : IRouteService
    {
        public object Redirect(short settingTypeId, byte actionId, ActionType actionType, dynamic parameters)
        {
            IAdditionalRequestHandler handler = AdditionalRequestHandlerFactory.GetInstance(settingTypeId);
            object result = handler.Handle(actionId, actionType, parameters);

            return result;
        }
    }
}