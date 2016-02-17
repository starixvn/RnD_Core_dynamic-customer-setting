namespace Fanex.CustomerSetting.Services.Impl
{
    using Fanex.CustomerSetting.Models.Requests;

    public class RouteService : IRouteService
    {
        public object Redirect(RedirectCommand command)
        {
            IAdditionalRequestHandler handler = AdditionalRequestHandlerFactory.GetInstance(command.SettingTypeId);
            object result = handler.Handle(command.ActionId, command.ActionType, command.Parameters);

            return result;
        }
    }
}