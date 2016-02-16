namespace Fanex.CustomerSetting.Services
{
    using Fanex.CustomerSetting.Models.Requests;

    public interface IRouteService
    {
        object Redirect(RouteCommand command);
    }
}