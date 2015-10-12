namespace Fanex.CustomerSetting.MatrixClient.Services
{
    using System;
    using Fanex.CustomerSetting.MatrixClient.Exceptions;
    using Fanex.CustomerSetting.MatrixClient.Models.Requests;
    using RestSharp;

    public class CustomerSettingMatrixService : ICustomerSettingMatrixService
    {
        private const string SettingMatrixTemplateCachingKey = "CustomerSettingMatrixTemplate";
        private const byte CachingHours = 4;

        public IRestResponse QueryAllProducts()
        {
            var request = new RestRequest("/product/", Method.GET);

            return ApiClient.Client.Execute(request);
        }

        public IRestResponse QueryCustomerSettingMatrix(QuerySettingMatrixRequest query)
        {
            if (query == null)
            {
                throw new SettingMatrixException("QuerySettingMatrixRequest cannot be null");
            }

            var request = new RestRequest("/customersettingmatrix/get", Method.GET);

            request.AddObject(query);

            return ApiClient.Client.Execute(request);
        }

        public IRestResponse QueryCustomerSettingMatrixTemplate()
        {
            IRestRequest request = new RestRequest("/customersettingmatrix/gettemplate", Method.GET);

            return ApiClient.Client.Execute(request);
        }
    }
}