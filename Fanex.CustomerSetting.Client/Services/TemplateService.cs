namespace Fanex.CustomerSetting.Client.Services
{
    using System;
    using System.Net;
    using System.Runtime.Caching;
    using Fanex.CustomerSetting.Client.Requests;
    using RestSharp;

    public class TemplateService : ITemplateService
    {
        private const string SettingTemplateCachingKey = "CustomerSettingTemplate";
        private const string SettingTemplateIe8CachingKey = "CustomerSettingIE8Template";
        private const string SettingMatrixTemplateCachingKey = "CustomerSettingMatrixTemplate";
        private const byte CachingHours = 4;

        public string QueryCustomerSettingTemplate(QuerySettingTemplateRequest query)
        {
            var cachedKey = query.IsIe8 ? SettingTemplateIe8CachingKey : SettingTemplateCachingKey;
            var template = BuildTemplate("/Template/GetCustomerSettingTemplate", cachedKey, query);

            template = template.Replace("{{params}}", query.SettingFormQuery);
            template = template.Replace("{{productId}}", query.ProductId.ToString());

            return template;
        }

        public string QueryCustomerSettingMatrixTemplate(QuerySettingMatrixTemplateRequest query)
        {
            var template = BuildTemplate("/Template/GetCustomerSettingMatrixTemplate", SettingMatrixTemplateCachingKey, query);

            template = template.Replace("{{customerName}}", query.CustomerName);

            return template;
        }

        private IRestResponse QueryTemplate(string queryUrl, SettingTemplateRequest query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }

            var request = new RestRequest(queryUrl, Method.GET);

            request.AddObject(query);

            return ApiClient.Client.Execute(request);
        }

        private string BuildTemplate(string queryUrl, string templateCacheKey, SettingTemplateRequest templateRequest)
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