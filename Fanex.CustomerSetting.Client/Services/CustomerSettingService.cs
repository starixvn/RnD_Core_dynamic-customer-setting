namespace Fanex.CustomerSetting.Client.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.CustomerSetting.Client.Configurations;
    using Fanex.CustomerSetting.Client.Requests;
    using RestSharp;

    public class CustomerSettingService : ICustomerSettingService
    {
        private static readonly RestClient Client =
            new RestClient(CustomerSettingConfiguration.Settings.ServiceUri);

        public RestResponse QueryCustomerSetting(QuerySettingRequest query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }

            var request = new RestRequest("/customersetting/", Method.GET);
            request.AddParameter("ProductId", query.ProductId);

            Client.Execute(request);
        }
    }
}
