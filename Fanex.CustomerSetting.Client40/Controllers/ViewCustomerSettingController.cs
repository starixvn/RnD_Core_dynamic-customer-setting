namespace Fanex.CustomerSetting.Client40.Controllers
{
    using System;
    using System.Net;
    using System.Runtime.Caching;
    using System.Web.Mvc;
    using Fanex.CustomerSetting.Client40.Attributes;
    using Fanex.CustomerSetting.Client40.Configurations;
    using Fanex.CustomerSetting.Client40.Exceptions;
    using Fanex.CustomerSetting.Client40.Helpers;
    using Fanex.CustomerSetting.Client40.Models;
    using Fanex.CustomerSetting.Client40.Models.Requests;
    using Fanex.CustomerSetting.Client40.Services;
    using RestSharp;

    [ExceptionHandlingAttribute]
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

        [CustomerSettingAuth(IsUpdate = false, IsMultipleUpdate = false)]
        public ContentResult Index(SettingFormQuery query)
        {
            ObjectCache cache = MemoryCache.Default;
            bool isIe8 = this.Request.Browser.Browser == "IE" && this.Request.Browser.MajorVersion < 9;
            string cachedKey = isIe8 ? SettingTemplateIe8CachingKey : SettingTemplateCachingKey;
            string template = cache.Get(cachedKey) as string;

            if (string.IsNullOrEmpty(template))
            {
                template = BuildTemplate(isIe8);
                cache.Set(cachedKey, template, DateTime.Now.AddHours(CachingHours));
            }

            if (!string.IsNullOrEmpty(template))
            {
                template = ReplaceParameters(template, query);
            }

            return Content(template, "text/HTML");
        }

        [HttpPost]
        [ValidateAntiForgery, CustomerSettingAuth(IsUpdate = false, IsMultipleUpdate = false)]
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
            Response.StatusCode = (int)customerSettingResponse.StatusCode;

            return Content(customerSettingResponse.Content);
        }

        private string BuildTemplate(bool isIe8)
        {
            IRestResponse response = _customerSettingService.QueryCustomerSettingTemplate(isIe8);
            bool isDebug = HttpContext.IsDebuggingEnabled;
            string cdn = RandomHelper.GetRandomUri(CustomerSettingConfiguration.Settings.CdnUris).ToString();
            string customCss = CustomerSettingConfiguration.Settings.CustomCssUrl != null
                ? Url.Content(CustomerSettingConfiguration.Settings.CustomCssUrl.ToString())
                : string.Empty;
            string template = string.Empty;

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new CustomerSettingException(response.Content);
            }

            template = SimpleJson.DeserializeObject<string>(response.Content);
            template = template.Replace("{{cdnUri}}", cdn);
            template = template.Replace("{{customCss}}", customCss);
            template = template.Replace("{{version}}", isDebug ? "0.0.1" : Guid.NewGuid().ToString("N"));
            template = template.Replace("{{min}}", isDebug ? string.Empty : ".min");

            return template;
        }

        private string ReplaceParameters(string template, SettingFormQuery query)
        {
            string finalTemplate = template;
            AppConfiguration appConfig = new AppConfiguration(Response.ApplyAppPathModifier(Request.ApplicationPath));

            appConfig.Editable = !query.IsReadOnly && CustomerSettingRegistration.CheckUpdatePermission(CustomerSettingRegistration.GetPermissionContext()); 

            if (!string.IsNullOrEmpty(finalTemplate))
            {
                finalTemplate = finalTemplate.Replace("{{appConfig}}", appConfig.ToJson());
                finalTemplate = finalTemplate.Replace("{{params}}", query.ToJson());
                finalTemplate = finalTemplate.Replace("{{productId}}", query.ProductId.ToString());
            }

            return finalTemplate;
        }
    }
}