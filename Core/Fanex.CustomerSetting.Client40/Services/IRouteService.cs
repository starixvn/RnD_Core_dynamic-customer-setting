namespace Fanex.CustomerSetting.Client40.Services
{
    using Models.Requests;
    using RestSharp;

    public interface IRouteService
    {
        IRestResponse Redirect(RedirectRequest command);
    }
}