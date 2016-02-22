namespace Fanex.CustomerSetting.Client40.Services
{
    using System.Net;
    using Fanex.CustomerSetting.Client40.Configurations;
    using Fanex.CustomerSetting.Client40.Exceptions;
    using Fanex.CustomerSetting.Client40.Helpers;
    using Fanex.CustomerSetting.Client40.Models.Requests;
    using RestSharp;

    public class RouteService : IRouteService
    {
        private readonly IRestClient _restClient;

        internal RouteService()
            : this(null)
        {
        }

        internal RouteService(IRestClient restClient)
        {
            _restClient = restClient ?? GetRestClient();
        }

        public IRestResponse Redirect(RedirectRequest command)
        {
            if (command == null)
            {
                throw new CustomerSettingException("Redirect Request cannot be null");
            }

            var request = new RestRequest("/route/redirect/", Method.POST);

            request.AddJsonBody(command);
            request.Timeout = CustomerSettingConfiguration.Settings.UpdateSettingTimeout;

            IRestResponse restReponse = _restClient.Execute(request);

            return UpdateTimeOutResponse(restReponse);
        }

        private IRestClient GetRestClient()
        {
            var restClient = new RestClient(RandomHelper.GetRandomUri(CustomerSettingConfiguration.Settings.ServiceUris));

            restClient.Authenticator = new HmacAuthenticator();

            return restClient;
        }

        private IRestResponse UpdateTimeOutResponse(IRestResponse restReponse)
        {
            if ((int)restReponse.StatusCode == 0)
            {
                restReponse.StatusCode = HttpStatusCode.RequestTimeout;
                restReponse.Content = "{ \"message\": \"Process is timeout.\" }";
            }

            return restReponse;
        }
    }
}