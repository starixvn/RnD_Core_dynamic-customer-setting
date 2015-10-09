namespace Fanex.CustomerSetting.MatrixClient.Controllers
{
    using System;
    using System.Net;
    using System.Runtime.Caching;
    using System.Web.Mvc;
    using Fanex.CustomerSetting.MatrixClient.Attributes;
    using Fanex.CustomerSetting.MatrixClient.Configurations;
    using Fanex.CustomerSetting.MatrixClient.Helpers;
    using Fanex.CustomerSetting.MatrixClient.Models;
    using Fanex.CustomerSetting.MatrixClient.Models.Requests;
    using Fanex.CustomerSetting.MatrixClient.Services;
    using RestSharp;

    public class ViewCustomerSettingMatrixController : Controller
    {
        private const string SettingMatrixTemplateCachingKey = "CustomerSettingMatrixTemplate";
        private const byte CachingHours = 4;

        private readonly ICustomerSettingMatrixService _customerSettingMatrixService;

        public ViewCustomerSettingMatrixController(
            ICustomerSettingMatrixService customerSettingMatrixService)
        {
            _customerSettingMatrixService = customerSettingMatrixService ?? new CustomerSettingMatrixService();
        }

        public ViewCustomerSettingMatrixController()
            : this(null)
        {
        }

        [CustomerSettingMatrixAuth]
        public ContentResult Index(string customerName)
        {
            ObjectCache cache = MemoryCache.Default;
            string template = cache.Get(SettingMatrixTemplateCachingKey) as string;

            if (string.IsNullOrEmpty(template))
            {
                template = BuildTemplate();
                cache.Set(SettingMatrixTemplateCachingKey, template, DateTime.Now.AddHours(CachingHours));
            }

            if (!string.IsNullOrEmpty(template))
            {
                template = ReplaceParameters(template, customerName);
            }

            return Content(template, "text/HTML");
        }

        [HttpPost]
        [ValidateAntiForgery, CustomerSettingMatrixAuth]
        public ContentResult QuerySettingMatrix(QuerySettingMatrixRequest querySettingMatrix)
        {
            var customerSettingResponse = _customerSettingMatrixService.QueryCustomerSettingMatrix(querySettingMatrix);

            return Content(customerSettingResponse.Content);
        }

        [HttpPost]
        [ValidateAntiForgery, CustomerSettingMatrixAuth]
        public ContentResult QueryAllProducts()
        {
            var productNamesReponse = _customerSettingMatrixService.QueryAllProducts();

            return Content(productNamesReponse.Content);
        }

        private string BuildTemplate()
        {
            IRestResponse response = _customerSettingMatrixService.QueryCustomerSettingMatrixTemplate();
            bool isDebug = HttpContext.IsDebuggingEnabled;
            string cdn = RandomHelper.GetRandomUri(SettingMatrixConfiguration.Settings.CdnUris).ToString();
            string customCss = SettingMatrixConfiguration.Settings.CustomCssUrl != null
                ? SettingMatrixConfiguration.Settings.CustomCssUrl.ToString()
                : string.Empty;
            string template = string.Empty;

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new InvalidOperationException(response.Content);
            }

            template = SimpleJson.DeserializeObject<string>(response.Content);
            template = template.Replace("{{cdnUri}}", cdn);
            template = template.Replace("{{customCss}}", customCss);
            template = template.Replace("{{version}}", isDebug ? "0.0.1" : Guid.NewGuid().ToString("N"));
            template = template.Replace("{{min}}", isDebug ? string.Empty : ".min");

            return template;
        }

        private string ReplaceParameters(string template, string customerName)
        {
            string finalTemplate = template;
            AppConfiguration appConfig = new AppConfiguration(Response.ApplyAppPathModifier(Request.ApplicationPath));

            if (!string.IsNullOrEmpty(finalTemplate))
            {
                finalTemplate = finalTemplate.Replace("{{appConfig}}", appConfig.ToJson());
                finalTemplate = finalTemplate.Replace("{{customerName}}", customerName);
            }

            return finalTemplate;
        }
    }
}