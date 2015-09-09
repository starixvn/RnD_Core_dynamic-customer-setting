namespace Fanex.CustomerSetting.DemoSite.Controllers
{
    using System;
    using System.Net;
    using System.Runtime.Caching;
    using System.Web;
    using System.Web.Mvc;
    using Fanex.CustomerSetting.Client.Requests;
    using Fanex.CustomerSetting.Client.Services;
    using Fanex.CustomerSetting.DemoSite.Contexts;
    using Fanex.CustomerSetting.DemoSite.Helpers;
    using Fanex.CustomerSetting.DemoSite.ViewModels;

    public class ViewCustomerSettingController : Controller
    {
        private const string TemplateCachingKey = "CustomerSettingTemplate";
        private const string Ie8TemplateCachingKey = "CustomerSettingIE8Template";
        private const byte CachingHours = 4;
        private readonly ICustomerSettingService _customerSettingService;
        private readonly ITemplateService _templateService;

        public ViewCustomerSettingController(
            ICustomerSettingService customerSettingService,
            ITemplateService templateService)
        {
            _customerSettingService = customerSettingService ?? new CustomerSettingService();
            _templateService = templateService ?? new TemplateService();
        }

        public ViewCustomerSettingController()
            : this(null, null)
        {
        }

        [HttpPost]
        public ContentResult Index(SettingFormQuery query)
        {
            ObjectCache cache = MemoryCache.Default;

            var isIe8 = this.Request.Browser.Browser == "IE" && this.Request.Browser.MajorVersion < 9;
            var cachedKey = isIe8 ? Ie8TemplateCachingKey : TemplateCachingKey;
            var template = cache.Get(cachedKey) as string;

            if (string.IsNullOrEmpty(template))
            {
                template = GetTemplate(cache, isIe8, cachedKey);
            }

            template = template.Replace("{{params}}", JsonConverter.ToJson(query));
            template = template.Replace("{{productId}}", query.ProductId.ToString());

            return Content(template, "text/HTML");
        }

        [HttpPost]
        [ValidateAntiForgery]
        public ContentResult QuerySetting(QuerySettingRequest querySetting)
        {
            var settingContext = SettingContextManager.SettingContext;
            querySetting.ActorId = settingContext.ActorId;
            querySetting.ActorLevel = settingContext.ActorLevel;
            querySetting.ActorName = settingContext.ActorName;
            querySetting.SubAccountId = settingContext.SubAccountId;
            querySetting.Domain = settingContext.Domain;
            querySetting.Language = settingContext.Language;

            var customerSettingResponse = _customerSettingService.QueryCustomerSetting(querySetting);

            return Content(customerSettingResponse.Content);
        }

        private string GetTemplate(ObjectCache cache, bool isIe8, string cachedKey)
        {
            var appConfig = new AppConfiguration(HttpContext.IsDebuggingEnabled, Request.ApplicationPath);
            var response = _templateService.QueryCustomerSettingTemplate(new QueryTemplateRequest
            {
                IsIe8 = isIe8,
                IsDebug = appConfig.IsDebug
            });

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new InvalidOperationException(response.ErrorMessage);
            }

            var template = Newtonsoft.Json.JsonConvert.DeserializeObject<string>(response.Content);
            template = template.Replace("{{rootUrl}}", appConfig.RootUrl);
            template = template.Replace("{{appConfig}}", JsonConverter.ToJson(appConfig));

            cache.Set(cachedKey, template, DateTime.Now.AddHours(CachingHours));

            return template;
        }
    }
}