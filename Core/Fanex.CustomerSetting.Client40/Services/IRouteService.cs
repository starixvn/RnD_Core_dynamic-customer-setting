namespace Fanex.CustomerSetting.Client40.Services
{
    using Fanex.CustomerSetting.Client40.Models.Requests;
    using RestSharp;

    public interface IRouteService
    {
        IRestResponse Redirect(RedirectRequest command);
    }
}