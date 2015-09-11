namespace Fanex.CustomerSetting.Client40.Controllers
{
    using System;
    using System.Net;
    using System.Runtime.Caching;
    using System.Web.Mvc;
    using Fanex.CustomerSetting.Client40.AntiForgery;
    using Fanex.CustomerSetting.Client40.Context;
    using Fanex.CustomerSetting.Client40.Models;
    using Fanex.CustomerSetting.Client40.Models.Requests;
    using Fanex.CustomerSetting.Client40.Services;
    using RestSharp;

    public class ViewCustomerSettingController : Controller
    {
        private const string SettingTemplateCachingKey = "CustomerSettingTemplate";
        private const string SettingTemplateIe8CachingKey = "CustomerSettingIE8Template";
        private const byte CachingHours = 4;

        private readonly ICustomerSettingService _customerSettingService;

        public ViewCustomerSettingController(
            ICustomerSettingService customerSettingService)
        {
            _customerSettingService = customerSettingService ?? new CustomerSettingService();
        }

        public ViewCustomerSettingController()
            : this(null)
        {
        }

        [HttpPost]
        public ContentResult Index(SettingFormQuery query)
        {
            bool isIe8 = this.Request.Browser.Browser == "IE" && this.Request.Browser.MajorVersion < 9;
            AppConfiguration appConfig = new AppConfiguration(HttpContext.IsDebuggingEnabled, Request.ApplicationPath);
            ObjectCache cache = MemoryCache.Default;
            string cachedKey = isIe8 ? SettingTemplateIe8CachingKey : SettingTemplateCachingKey;
            string template = cache.Get(cachedKey) as string;

            if (string.IsNullOrEmpty(template))
            {
                IRestResponse response = _customerSettingService.QueryCustomerSettingTemplate(isIe8);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return Content(response.Content, "text/HTML");
                }

                template = SimpleJson.DeserializeObject<string>(response.Content);
                template = template.Replace("{{cdnUri}}", appConfig.CdnUri.ToString());
                template = template.Replace("{{version}}", appConfig.ScriptVersion);
                template = template.Replace("{{min}}", appConfig.IsDebug ? string.Empty : ".min");

                cache.Set(cachedKey, template, DateTime.Now.AddHours(CachingHours));
            }

            if (!string.IsNullOrEmpty(template))
            {
                template = template.Replace("{{appConfig}}", appConfig.ToJson());
                template = template.Replace("{{params}}", query.ToJson());
                template = template.Replace("{{productId}}", query.ProductId.ToString());
            }

            return Content(template, "text/HTML");
        }

        [HttpPost]
        [ValidateAntiForgery]
        public ContentResult QuerySetting(QuerySettingRequest querySetting)
        {
            querySetting.ActorId = SettingContextManager.SettingContext.ActorId;
            querySetting.ActorLevel = SettingContextManager.SettingContext.ActorLevel;
            querySetting.ActorName = SettingContextManager.SettingContext.ActorName;
            querySetting.SubAccountId = SettingContextManager.SettingContext.SubAccountId;
            querySetting.Domain = SettingContextManager.SettingContext.Domain;
            querySetting.Language = SettingContextManager.SettingContext.Language;

            var customerSettingResponse = _customerSettingService.QueryCustomerSetting(querySetting);

            return Content(customerSettingResponse.Content);
        }
    }
}