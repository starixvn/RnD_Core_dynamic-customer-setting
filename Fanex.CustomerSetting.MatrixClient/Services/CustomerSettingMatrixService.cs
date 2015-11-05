namespace Fanex.CustomerSetting.MatrixClient.Services
{
    using Fanex.CustomerSetting.MatrixClient.Configurations;
    using Fanex.CustomerSetting.MatrixClient.Exceptions;
    using Fanex.CustomerSetting.MatrixClient.Helpers;
    using Fanex.CustomerSetting.MatrixClient.Models.Requests;
    using RestSharp;

    public class CustomerSettingMatrixService : ICustomerSettingMatrixService
    {
        private const string SettingMatrixTemplateCachingKey = "CustomerSettingMatrixTemplate";
        private const byte CachingHours = 4;
        private readonly IRestClient _restClient;

        internal CustomerSettingMatrixService()
            : this(null)
        {
        }

        /// <summary>
        /// https://groups.google.com/forum/#!topic/restsharp/X8JPtcfnSlo
        /// http://pawel.sawicz.eu/restsharp/
        /// </summary>
        /// <param name="?"></param>
        internal CustomerSettingMatrixService(IRestClient restClient)
        {
            _restClient = restClient ?? GetRestClient();
        }

        public IRestResponse QueryAllProducts()
        {
            var request = new RestRequest("/product/", Method.GET);

            return _restClient.Execute(request);
        }

        public IRestResponse QueryCustomerSettingMatrix(QuerySettingMatrixRequest query)
        {
            if (query == null)
            {
                throw new SettingMatrixException("QuerySettingMatrixRequest cannot be null");
            }

            var request = new RestRequest("/customersettingmatrix/get", Method.GET);

            request.AddObject(query);

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