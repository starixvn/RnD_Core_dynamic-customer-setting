namespace Fanex.CustomerSetting.DemoSite.Controllers
{
    using System;
    using System.Net;
    using System.Runtime.Caching;
    using System.Web;
    using System.Web.Mvc;
    using Fanex.CustomerSetting.Client.Requests;
    using Fanex.CustomerSetting.Client.Services;
    using Fanex.CustomerSetting.DemoSite.Helpers;
    using Fanex.CustomerSetting.DemoSite.ViewModels;

    public class ViewCustomerSettingMatrixController : Controller
    {
        private const string TemplateCachingKey = "CustomerSettingMatrixTemplate";
        private const byte CachingHours = 4;
        private readonly ICustomerSettingMatrixService _customerSettingMatrixService;
        private readonly ITemplateService _templateService;

        public ViewCustomerSettingMatrixController(
            ICustomerSettingMatrixService customerSettingMatrixService,
            ITemplateService templateService)
        {
            _customerSettingMatrixService = customerSettingMatrixService ?? new CustomerSettingMatrixService();
            _templateService = templateService ?? new TemplateService();
        }

        public ViewCustomerSettingMatrixController()
            : this(null, null)
        {
        }

        public ContentResult Index(string customerName = "428RM")
        {
            string template = GetTemplate();

            template = template.Replace("{{customerName}}", JsonConverter.ToJson(customerName));

            return Content(template);
        }

        [HttpPost]
        [ValidateAntiForgery]
        public ContentResult QuerySettingMatrix(QuerySettingMatrixRequest querySettingMatrix)
        {
            var customerSettingResponse = _customerSettingMatrixService.QueryCustomerSettingMatrix(querySettingMatrix);

            return Content(customerSettingResponse.Content);
        }

        [HttpPost]
        [ValidateAntiForgery]
        public ContentResult QueryAllProducts()
        {
            var productNamesReponse = _customerSettingMatrixService.QueryAllProducts();

            return Content(productNamesReponse.Content);
        }

        private string GetTemplate()
        {
            ObjectCache cache = MemoryCache.Default;
            var template = cache.Get(TemplateCachingKey) as string;

            if (string.IsNullOrEmpty(template))
            {
                var appConfig = new AppConfiguration(HttpContext.IsDebuggingEnabled, Request.ApplicationPath);

                var response = _templateService.QueryCustomerSettingMatrixTemplate(new QueryTemplateRequest
                {
                    IsDebug = appConfig.IsDebug
                });

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw new InvalidOperationException(response.ErrorMessage);
                }

                template = Newtonsoft.Json.JsonConvert.DeserializeObject<string>(response.Content);
                template = template.Replace("{{rootUrl}}", appConfig.RootUrl);
                template = template.Replace("{{appConfig}}", JsonConverter.ToJson(appConfig));

                cache.Set(TemplateCachingKey, template, DateTime.Now.AddHours(CachingHours));
            }

            return template;
        }
    }
}