namespace Fanex.CustomerSetting.Client40.Services
{
    using System;
    using Fanex.CustomerSetting.Client40.Exceptions;
    using Fanex.CustomerSetting.Client40.Models.Requests;
    using RestSharp;

    internal class CustomerSettingService : ICustomerSettingService
    {
        public IRestResponse QueryCustomerSetting(QuerySettingRequest query)
        {
            if (query == null)
            {
                throw new CustomerSettingException("Query Setting Request cannot be null");
            }

            var request = new RestRequest("/customersetting/", Method.GET);

            request.AddObject(query);

            return ApiClient.Client.Execute(request);
        }

        public IRestResponse UpdateCustomerSetting(UpdateSettingRequest update)
        {
            if (update == null)
            {
                throw new CustomerSettingException("Update Setting Request cannot be null");
            }

            var request = new RestRequest("/customersetting/post/", Method.POST);

            request.AddJsonBody(update);

            return ApiClient.Client.Execute(request);
        }

        public IRestResponse MultipleUpdateCustomerSetting(MultipleUpdateSettingRequest multipleUpdate)
        {
            if (multipleUpdate == null)
            {
                throw new CustomerSettingException("Multiple Update Setting Request cannot be null");
            }

            var request = new RestRequest("/customersetting/postmultiple/", Method.POST);

            request.AddJsonBody(multipleUpdate);

            return ApiClient.Client.Execute(request);
        }

        public IRestResponse QueryCustomerSettingTemplate(bool isIe8)
        {
            IRestRequest request = new RestRequest("/customersetting/gettemplate", Method.GET);

            request.AddParameter("isIe8", isIe8);

            return ApiClient.Client.Execute(request);
        }
    }
}