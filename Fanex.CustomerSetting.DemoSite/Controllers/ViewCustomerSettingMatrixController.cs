namespace Fanex.CustomerSetting.DemoSite.Controllers
{
    using System.Web;
    using System.Web.Mvc;
    using Fanex.CustomerSetting.Client.Requests;
    using Fanex.CustomerSetting.Client.Services;
    using Fanex.CustomerSetting.DemoSite.Helpers;
    using Fanex.CustomerSetting.DemoSite.ViewModels;

    public class ViewCustomerSettingMatrixController : Controller
    {
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
            var appConfig = new AppConfiguration(HttpContext.IsDebuggingEnabled, Request.ApplicationPath);
            var template = _templateService.QueryCustomerSettingMatrixTemplate(new QuerySettingMatrixTemplateRequest
            {
                IsDebug = appConfig.IsDebug,
                RootUrl = appConfig.RootUrl,
                CustomerName = customerName,
                AppConfig = JsonConverter.ToJson(appConfig)
            });

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