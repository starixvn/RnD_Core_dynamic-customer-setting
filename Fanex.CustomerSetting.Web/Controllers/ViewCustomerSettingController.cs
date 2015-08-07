namespace Fanex.CustomerSetting.Web.Controllers
{
    using System;
    using System.Runtime.Caching;
    using System.Web;
    using System.Web.Mvc;
    using Fanex.CustomerSetting.Models.Requests;
    using Fanex.CustomerSetting.Services;
    using Fanex.CustomerSetting.Services.Impl;
    using Fanex.CustomerSetting.Web.Contexts;
    using Fanex.CustomerSetting.Web.ViewModels;

    public class ViewCustomerSettingController : Controller
    {
        private const string TemplateCachingKey = "CustomerSettingTemplate";
        private const byte CachingHours = 4;
        private readonly IQuerySettingService _querySettingService;

        public ViewCustomerSettingController(IQuerySettingService querySettingService)
        {
            _querySettingService = querySettingService ?? new QuerySettingService(SettingContextManager.SettingContext);
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
        //[ValidateAntiForgeryToken]
        public JsonResult QuerySetting(CustomerSettingQuery querySetting)
        {
            var customerSetting = _querySettingService.GetCustomerSetting(querySetting);

            return new JsonNetResult(customerSetting, JsonRequestBehavior.AllowGet);
        }

        private string GetTemplate()
        {
            ObjectCache cache = MemoryCache.Default;
            string htmlContent = cache.Get(TemplateCachingKey) as string;

            if (string.IsNullOrEmpty(htmlContent))
            {
                string filePath = Server.MapPath("~/App/CustomerSetting/customer-setting.htm");
                var appConfig = new AppConfiguration(HttpContext.IsDebuggingEnabled, Request.ApplicationPath);

                htmlContent = System.IO.File.ReadAllText(filePath);
                htmlContent = htmlContent.Replace("{{rootUrl}}", appConfig.RootUrl);
                htmlContent = htmlContent.Replace("{{debug}}", appConfig.IsDebug.ToString().ToLowerInvariant());
                htmlContent = htmlContent.Replace("{{version}}", appConfig.ScriptVersion);
                htmlContent = htmlContent.Replace("{{min}}", appConfig.IsDebug ? string.Empty : ".min");
                htmlContent = htmlContent.Replace("{{appConfig}}", JsonConverter.ToJson(appConfig));

                cache.Set(TemplateCachingKey, htmlContent, DateTime.Now.AddHours(CachingHours));
            }

            return htmlContent;
        }
    }
}