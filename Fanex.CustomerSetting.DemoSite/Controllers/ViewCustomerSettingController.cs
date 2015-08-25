namespace Fanex.CustomerSetting.DemoSite.Controllers
{
    using System;
    using System.Runtime.Caching;
    using System.Web;
    using System.Web.Mvc;
    using Fanex.CustomerSetting.Client.Requests;
    using Fanex.CustomerSetting.Client.Services;
    using Fanex.CustomerSetting.DemoSite.Contexts;
    using Fanex.CustomerSetting.Web;
    using Fanex.CustomerSetting.Web.ViewModels;

    public class ViewCustomerSettingController : Controller
    {
        private const string TemplateCachingKey = "CustomerSettingTemplate";
        private const string IE8TemplateCachingKey = "CustomerSettingIE8Template";
        private const byte CachingHours = 4;
        private readonly ICustomerSettingService _customerSettingService;

        public ViewCustomerSettingController(ICustomerSettingService customerSettingService)
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
            string htmlContent = GetTemplate();

            htmlContent = htmlContent.Replace("{{params}}", JsonConverter.ToJson(query));

            return Content(htmlContent, "text/HTML");
        }

        [HttpPost]
        [ValidateAntiForgery]
        public JsonResult QuerySetting(QuerySettingRequest querySetting)
        {
            var settingContext = SettingContextManager.SettingContext;
            querySetting.ActorId = settingContext.ActorId;
            querySetting.ActorLevel = settingContext.ActorLevel;
            querySetting.ActorName = settingContext.ActorName;
            querySetting.SubAccountId = settingContext.SubAccountId;
            querySetting.Domain = settingContext.Domain;
            querySetting.Language = settingContext.Language;

            var customerSettingResponse = _customerSettingService.QueryCustomerSetting(querySetting);

            return Json(customerSettingResponse.Content, JsonRequestBehavior.AllowGet);
        }

        private string GetTemplate()
        {
            ObjectCache cache = MemoryCache.Default;
            string htmlContent = cache.Get(TemplateCachingKey) as string;

            if (string.IsNullOrEmpty(htmlContent))
            {
                var filePath = string.Empty;
                var cachedKey = string.Empty;
                var appConfig = new AppConfiguration(HttpContext.IsDebuggingEnabled, Request.ApplicationPath);

                if (this.Request.Browser.Browser == "IE" && this.Request.Browser.MajorVersion < 9)
                {
                    filePath = Server.MapPath("~/App/CustomerSetting.IE8/customer-setting.htm");
                    cachedKey = IE8TemplateCachingKey;
                }
                else
                {
                    filePath = Server.MapPath("~/App/CustomerSetting/customer-setting.htm");
                    cachedKey = TemplateCachingKey;
                }

                htmlContent = System.IO.File.ReadAllText(filePath);
                htmlContent = htmlContent.Replace("{{rootUrl}}", appConfig.RootUrl);
                htmlContent = htmlContent.Replace("{{debug}}", appConfig.IsDebug.ToString().ToLowerInvariant());
                htmlContent = htmlContent.Replace("{{version}}", appConfig.ScriptVersion);
                htmlContent = htmlContent.Replace("{{min}}", appConfig.IsDebug ? string.Empty : ".min");
                htmlContent = htmlContent.Replace("{{appConfig}}", JsonConverter.ToJson(appConfig));

                cache.Set(cachedKey, htmlContent, DateTime.Now.AddHours(CachingHours));
            }

            return htmlContent;
        }
    }
}