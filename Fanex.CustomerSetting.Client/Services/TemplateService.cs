namespace Fanex.CustomerSetting.Client.Services
{
    using System;
    using Fanex.CustomerSetting.Client.Requests;
    using RestSharp;

    public class TemplateService : ITemplateService
    {
        public IRestResponse QueryCustomerSettingTemplate(QueryTemplateRequest query)
        {
            return QueryTemplate("/Template/GetCustomerSettingTemplate", query);
        }

        public IRestResponse QueryCustomerSettingMatrixTemplate(QueryTemplateRequest query)
        {
            return QueryTemplate("/Template/GetCustomerSettingMatrixTemplate", query);
        }

        private IRestResponse QueryTemplate(string queryUrl, QueryTemplateRequest query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }

            var request = new RestRequest(queryUrl, Method.GET);

            request.AddObject(query);

            return ApiClient.Client.Execute(request);
        }
    }
}