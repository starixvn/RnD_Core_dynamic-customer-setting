namespace Fanex.CustomerSetting.DemoSite.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Caching;
    using System.Web;
    using System.Web.Mvc;
    using Fanex.CustomerSetting.Client.Requests;
    using Fanex.CustomerSetting.Client.Services;
    using Fanex.CustomerSetting.Web;
    using Fanex.CustomerSetting.Web.ViewModels;

    public class ViewCustomerSettingMatrixController : Controller
    {
        private const string TemplateCachingKey = "CustomerSettingMatrixTemplate";
        private const byte CachingHours = 4;
        private readonly ICustomerSettingMatrixService _customerSettingMatrixService;

        public ViewCustomerSettingMatrixController(ICustomerSettingMatrixService customerSettingMatrixService)
        {
            _customerSettingMatrixService = customerSettingMatrixService ?? new CustomerSettingMatrixService();
        }

        public ViewCustomerSettingMatrixController()
            : this(null)
        {
        }

        public ContentResult Index(string customerName = "428RM")
        {
            string htmlContent = GetTemplate();

            htmlContent = htmlContent.Replace("{{customerName}}", JsonConverter.ToJson(customerName));

            return Content(htmlContent, "text/HTML");
        }

        [HttpPost]
        [ValidateAntiForgery]
        public JsonResult QuerySettingMatrix(QuerySettingMatrixRequest querySettingMatrix)
        {
            var customerSettingResponse = _customerSettingMatrixService.QueryCustomerSettingMatrix(querySettingMatrix);

            return Json(customerSettingResponse.Content, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgery]
        public JsonResult QueryProductNames()
        {
            var productNamesReponse = _customerSettingMatrixService.QueryAllProductNames();

            return Json(productNamesReponse.Content, JsonRequestBehavior.AllowGet);
        }

        private string GetTemplate()
        {
            ObjectCache cache = MemoryCache.Default;
            string htmlContent = cache.Get(TemplateCachingKey) as string;

            if (string.IsNullOrEmpty(htmlContent))
            {
                string filePath = Server.MapPath("~/App/CustomerSettingMatrix/customer-setting-matrix.htm");
                var appConfig = new AppConfiguration(HttpContext.IsDebuggingEnabled, Request.ApplicationPath);

                htmlContent = System.IO.File.ReadAllText(filePath);
                htmlContent = htmlContent.Replace("{{rootUrl}}", appConfig.RootUrl);
                htmlContent = htmlContent.Replace("{{debug}}", appConfig.IsDebug.ToString().ToLowerInvariant());
                htmlContent = htmlContent.Replace("{{version}}", appConfig.ScriptVersion);
                htmlContent = htmlContent.Replace("{{min}}", appConfig.IsDebug ? string.Empty : ".min");
                htmlContent = htmlContent.Replace("{{appConfig}}", JsonConverter.ToJson(appConfig));

                //cache.Set(TemplateCachingKey, htmlContent, DateTime.Now.AddHours(CachingHours));
            }

            return htmlContent;
        }
    }
}
