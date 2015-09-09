namespace Fanex.CustomerSetting.MatrixClient.Services
{
    using System;
    using System.Net;
    using System.Runtime.Caching;
    using Fanex.CustomerSetting.MatrixClient.Requests;
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
                throw new ArgumentNullException("query");
            }

            var request = new RestRequest("/customersettingmatrix/", Method.GET);

            request.AddObject(query);

            return ApiClient.Client.Execute(request);
        }

        public string QueryCustomerSettingMatrixTemplate(QuerySettingMatrixTemplateRequest query)
        {
            var template = BuildTemplate("/Template/GetCustomerSettingMatrixTemplate", SettingMatrixTemplateCachingKey, query);

            template = template.Replace("{{customerName}}", query.CustomerName);

            return template;
        }

        private IRestResponse QueryTemplate(string queryUrl, QuerySettingMatrixTemplateRequest query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }

            var request = new RestRequest(queryUrl, Method.GET);

            request.AddObject(new
            {
                IsDebug = query.IsDebug
            });

            return ApiClient.Client.Execute(request);
        }

        private string BuildTemplate(string queryUrl, string templateCacheKey, QuerySettingMatrixTemplateRequest templateRequest)
        {
            ObjectCache cache = MemoryCache.Default;
            var template = cache.Get(templateCacheKey) as string;

            if (string.IsNullOrEmpty(template))
            {
                var response = QueryTemplate(queryUrl, templateRequest);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return response.ErrorMessage;
                }

                template = SimpleJson.DeserializeObject<string>(response.Content);
                template = template.Replace("{{rootUrl}}", templateRequest.RootUrl);
                template = template.Replace("{{appConfig}}", templateRequest.AppConfig);

                cache.Set(templateCacheKey, template, DateTime.Now.AddHours(CachingHours));
            }

            return template;
        }
    }
}