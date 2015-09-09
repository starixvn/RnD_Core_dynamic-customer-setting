namespace Fanex.CustomerSetting.DemoSite.Controllers
{
    using System.Web;
    using System.Web.Mvc;
    using Fanex.CustomerSetting.Client.Models;
    using Fanex.CustomerSetting.Client.Requests;
    using Fanex.CustomerSetting.Client.Services;
    using Fanex.CustomerSetting.DemoSite.Contexts;
    using Fanex.CustomerSetting.DemoSite.Helpers;
    using Fanex.CustomerSetting.DemoSite.ViewModels;

    public class ViewCustomerSettingController : Controller
    {
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
            var isIe8 = this.Request.Browser.Browser == "IE" && this.Request.Browser.MajorVersion < 9;
            var appConfig = new AppConfiguration(HttpContext.IsDebuggingEnabled, Request.ApplicationPath);

            var template = _customerSettingService.QueryCustomerSettingTemplate(new QuerySettingTemplateRequest
            {
                SettingFormQuery = JsonConverter.ToJson(query),
                ProductId = query.ProductId,
                AppConfig = JsonConverter.ToJson(appConfig),
                IsDebug = appConfig.IsDebug,
                RootUrl = appConfig.RootUrl,
                IsIe8 = isIe8
            });

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
    }
}