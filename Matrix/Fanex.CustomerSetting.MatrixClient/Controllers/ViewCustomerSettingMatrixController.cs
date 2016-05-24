namespace Fanex.CustomerSetting.MatrixClient.Controllers
{
    using System;
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
        public ContentResult Index(QuerySettingMatrixRequest querySettingMatrix)
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
                template = ReplaceParameters(template, querySettingMatrix);
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
            string cdn = RandomHelper.GetRandomUri(SettingMatrixConfiguration.Settings.CdnUris);
            string customCss = SettingMatrixConfiguration.Settings.CustomCssUrl != null
                ? Url.Content(SettingMatrixConfiguration.Settings.CustomCssUrl.ToString())
                : string.Empty;
            string template = string.Empty;

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new SettingMatrixException(response.Content);
            }

            if (!cdn.Contains("http"))
            {
                cdn = Url.Content(cdn);
            }

            template = SimpleJson.DeserializeObject<string>(response.Content);
            template = template.Replace("{{cdnUri}}", cdn);
            template = template.Replace("{{customCss}}", customCss);
            template = template.Replace("{{version}}", isDebug ? "0.0.1" : Guid.NewGuid().ToString("N"));
            template = template.Replace("{{min}}", isDebug ? string.Empty : ".min");

            return template;
        }

        private string ReplaceParameters(string template, QuerySettingMatrixRequest querySettingMatrix)
        {
            string finalTemplate = template;
            var appConfig = new AppConfiguration(Response.ApplyAppPathModifier(Request.ApplicationPath));

            if (!string.IsNullOrEmpty(finalTemplate))
            {
                finalTemplate = finalTemplate.Replace("{{appConfig}}", appConfig.ToJson());
                finalTemplate = finalTemplate.Replace("{{querySettingMatrix}}", querySettingMatrix.ToJson());
            }

            return finalTemplate;
        }
    }
}