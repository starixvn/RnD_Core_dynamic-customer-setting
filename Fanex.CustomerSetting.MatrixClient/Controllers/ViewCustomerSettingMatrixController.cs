namespace Fanex.CustomerSetting.MatrixClient.Controllers
{
    using System;
    using System.Net;
    using System.Runtime.Caching;
    using System.Web.Mvc;
    using Fanex.CustomerSetting.MatrixClient.AntiForgery;
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

        public ContentResult Index(string customerName)
        {
            AppConfiguration appConfig = new AppConfiguration(HttpContext.IsDebuggingEnabled, Request.ApplicationPath);
            ObjectCache cache = MemoryCache.Default;
            string template = cache.Get(SettingMatrixTemplateCachingKey) as string;

            if (string.IsNullOrEmpty(template))
            {
                IRestResponse response = _customerSettingMatrixService.QueryCustomerSettingMatrixTemplate();

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return Content(response.Content, "text/HTML");
                }

                template = SimpleJson.DeserializeObject<string>(response.Content);
                template = template.Replace("{{cdnUri}}", appConfig.CdnUri.ToString());
                template = template.Replace("{{version}}", appConfig.ScriptVersion);
                template = template.Replace("{{min}}", appConfig.IsDebug ? string.Empty : ".min");

                cache.Set(SettingMatrixTemplateCachingKey, template, DateTime.Now.AddHours(CachingHours));
            }

            if (!string.IsNullOrEmpty(template))
            {
                template = template.Replace("{{appConfig}}", appConfig.ToJson());
                template = template.Replace("{{customerName}}", customerName);
            }

            return Content(template, "text/HTML");
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
    }
}