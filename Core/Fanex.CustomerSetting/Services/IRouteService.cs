namespace Fanex.CustomerSetting.Services
{
    using Models.Requests;

    public interface IRouteService
    {
        object Redirect(RedirectCommand command);
    }
}