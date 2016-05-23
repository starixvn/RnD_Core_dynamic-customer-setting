namespace Fanex.CustomerSetting.Client40.Controllers
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Net;
    using System.Runtime.Caching;
    using System.Web.Mvc;
    using Attributes;
    using Configurations;
    using Exceptions;
    using Helpers;
    using Models;
    using Models.Requests;
    using RestSharp;
    using Services;

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
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

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
                SettingContext settingContext = CustomerSettingRegistration.SettingContext;

                query.System = settingContext.System;
                query.Language = settingContext.Language;
                template = ReplaceParameters(template, query);
            }

            return Content(template, "text/HTML");
        }

        [SuppressMessage(
            "Microsoft.Naming",
            "CA1719:ParameterNamesShouldNotMatchMemberNames",
            Justification = "Reviewed!"), HttpPost]
        [ValidateAntiForgery, CustomerSettingAuth(IsUpdate = false, IsMultipleUpdate = false), LogInfoFilter]
        public ContentResult QuerySetting(QuerySettingRequest querySetting)
        {
            if (querySetting == null)
            {
                throw new ArgumentNullException(nameof(querySetting), "Query Setting cannot be null");
            }

            IRestResponse customerSettingResponse;
            SettingContext settingContext = CustomerSettingRegistration.SettingContext;

            querySetting.ActorId = settingContext.ActorId;
            querySetting.ActorLevel = settingContext.ActorLevel;
            querySetting.ActorName = settingContext.ActorName;
            querySetting.SubAccountId = settingContext.SubAccountId;
            querySetting.Domain = settingContext.Domain;
            querySetting.Language = settingContext.Language;
            querySetting.ClientIP = settingContext.IP;
            querySetting.System = settingContext.System;

            customerSettingResponse = _customerSettingService.QueryCustomerSetting(querySetting);
            Response.StatusCode = (int)customerSettingResponse.StatusCode;
            Response.TrySkipIisCustomErrors = true;

            return Content(customerSettingResponse.Content);
        }

        private string BuildTemplate(bool isIe8)
        {
            IRestResponse response = _customerSettingService.QueryCustomerSettingTemplate(isIe8);
            bool isDebug = HttpContext.IsDebuggingEnabled;
            string cdn = RandomHelper.GetRandomUri(CustomerSettingConfiguration.Settings.CdnUris);

            if (!cdn.Contains("http"))
            {
                cdn = Url.Content(cdn);
            }

            string customCss = CustomerSettingConfiguration.Settings.CustomCssUrl != null
                ? Url.Content(CustomerSettingConfiguration.Settings.CustomCssUrl.ToString())
                : string.Empty;
            string template = string.Empty;

            if (response.StatusCode != HttpStatusCode.OK)
            {
                var errorMessage = response.StatusCode == 0 ? response.ErrorMessage : response.Content;

                throw new CustomerSettingException(errorMessage);
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

            appConfig.Editable = !query.IsReadOnly;

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