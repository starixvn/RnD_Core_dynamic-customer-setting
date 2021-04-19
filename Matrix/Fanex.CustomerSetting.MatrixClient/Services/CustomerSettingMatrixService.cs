namespace Fanex.CustomerSetting.MatrixClient.Services
{
    using System;
    using System.Net.Http;
    using Configurations;
    using Helpers;
    using Models.Requests;
    using RestSharp;

    public class CustomerSettingMatrixService : ICustomerSettingMatrixService
    {
        private readonly IRestClient _restClient;

        internal CustomerSettingMatrixService()
            : this(null)
        {
        }

        internal CustomerSettingMatrixService(IRestClient restClient)
        {
            _restClient = restClient ?? GetRestClient();
        }

        public IRestResponse QueryAllProducts()
        {
            for (int i = 0; i < 10; i++)
            {
                using (var client = new HttpClient())
                {
                   var response = client.GetAsync(new Uri("product/GetAll", UriKind.Relative));
                    
                }
            }

            var request = new RestRequest("/product/", Method.GET);
            return _restClient.Execute(request);
        }

        public IRestResponse QueryCustomerSettingMatrix(QuerySettingMatrixRequest query)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            var request = new RestRequest("/customersettingmatrix/post", Method.POST);

            request.AddJsonBody(query);

            return _restClient.Execute(request);
        }

        public IRestResponse QueryCustomerSettingMatrixTemplate()
        {
            IRestRequest request = new RestRequest("/customersettingmatrix/gettemplate", Method.GET);

            return _restClient.Execute(request);
        }

        private IRestClient GetRestClient()
        {
            var restClient = new RestClient(RandomHelper.GetRandomUri(SettingMatrixConfiguration.Settings.ServiceUris));
            restClient.Authenticator = new HmacAuthenticator();

            return restClient;
        }
    }
}