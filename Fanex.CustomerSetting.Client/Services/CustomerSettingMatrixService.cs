namespace Fanex.CustomerSetting.Client.Services
{
    using System;
    using Fanex.CustomerSetting.Client.Requests;
    using RestSharp;

    public class CustomerSettingMatrixService : ICustomerSettingMatrixService
    {
        public IRestResponse QueryAllProductNames()
        {
            var request = new RestRequest("/product/", Method.GET);

            return ApiClient.Client.Execute(request);
        }

        public IRestResponse QueryCustomerSettingMatrix(QuerySettingMatrixRequest query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }

            var request = new RestRequest("/customersettingmaxtrix/", Method.GET);

            request.AddObject(query);

            return ApiClient.Client.Execute(request);
        }
    }
}