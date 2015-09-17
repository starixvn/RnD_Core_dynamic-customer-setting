namespace Fanex.CustomerSetting.Client40.Controllers
{
    using System;
    using System.Net;
    using System.Runtime.Caching;
    using System.Web.Mvc;
    using Fanex.CustomerSetting.Client40.AntiForgery;
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
            AppConfiguration appConfig = new AppConfiguration(HttpContext.IsDebuggingEnabled, Request.ApplicationPath);
            ObjectCache cache = MemoryCache.Default;
            bool isIe8 = this.Request.Browser.Browser == "IE" && this.Request.Browser.MajorVersion < 9;
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
            IRestResponse customerSettingResponse;
            SettingContext settingContext = CustomerSettingRegistration.SettingContext;

            querySetting.ActorId = settingContext.ActorId;
            querySetting.ActorLevel = settingContext.ActorLevel;
            querySetting.ActorName = settingContext.ActorName;
            querySetting.SubAccountId = settingContext.SubAccountId;
            querySetting.Domain = settingContext.Domain;
            querySetting.Language = settingContext.Language;

            customerSettingResponse = _customerSettingService.QueryCustomerSetting(querySetting);

            return Content(customerSettingResponse.Content);
        }
    }
}