namespace Fanex.CustomerSetting.Client.Services
{
    using System;
    using System.Net;
    using System.Runtime.Caching;
    using Fanex.CustomerSetting.Client.Configurations;
    using Fanex.CustomerSetting.Client.Requests;
    using RestSharp;

    public class CustomerSettingService : ICustomerSettingService
    {
        private const string SettingTemplateCachingKey = "CustomerSettingTemplate";
        private const string SettingTemplateIe8CachingKey = "CustomerSettingIE8Template";
        private const byte CachingHours = 4;

        public IRestResponse QueryCustomerSetting(QuerySettingRequest query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }

            var request = new RestRequest("/customersetting/", Method.GET);

            request.AddObject(query);

            return ApiClient.Client.Execute(request);
        }

        public IRestResponse UpdateCustomerSetting(UpdateSettingRequest update)
        {
            if (update == null)
            {
                throw new ArgumentNullException("update", "UpdateSettingRequest cannot be null");
            }

            var request = new RestRequest("/customersetting/post/", Method.POST);

            request.AddJsonBody(update);

            return ApiClient.Client.Execute(request);
        }

        public IRestResponse MultipleUpdateCustomerSetting(MultipleUpdateSettingRequest multipleUpdate)
        {
            if (multipleUpdate == null)
            {
                throw new ArgumentNullException("multipleUpdate", "MultipleUpdateSettingRequest cannot be null");
            }

            var request = new RestRequest("/customersetting/postmultiple/", Method.POST);

            request.AddJsonBody(multipleUpdate);

            return ApiClient.Client.Execute(request);
        }

        public string QueryCustomerSettingTemplate(QuerySettingTemplateRequest query)
        {
            var cachedKey = query.IsIe8 ? SettingTemplateIe8CachingKey : SettingTemplateCachingKey;
            var template = BuildTemplate("/Template/GetCustomerSettingTemplate", cachedKey, query);

            template = template.Replace("{{params}}", query.SettingFormQuery);
            template = template.Replace("{{productId}}", query.ProductId.ToString());

            return template;
        }

        private IRestResponse QueryTemplate(string queryUrl, QuerySettingTemplateRequest query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }

            var request = new RestRequest(queryUrl, Method.GET);

            request.AddObject(new
            {
                IsDebug = query.IsDebug,
                IsIe8 = query.IsIe8
            });

            return ApiClient.Client.Execute(request);
        }

        private string BuildTemplate(string queryUrl, string templateCacheKey, QuerySettingTemplateRequest templateRequest)
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