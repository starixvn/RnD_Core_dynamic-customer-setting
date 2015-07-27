namespace Fanex.CustomerSetting.Web.Controllers
{
    using System;
    using System.Web.Mvc;
    using Fanex.CustomerSetting.Models.Enumerations;
    using Fanex.CustomerSetting.Models.Requests;
    using Fanex.CustomerSetting.Services;
    using Fanex.CustomerSetting.Services.Impl;
    using Fanex.CustomerSetting.Web.Contexts;

    public class ViewCustomerSettingController : Controller
    {
        private readonly IQuerySettingService _querySettingService;

        public ViewCustomerSettingController(IQuerySettingService querySettingService)
        {
            _querySettingService = querySettingService ?? new QuerySettingService(SettingContextManager.SettingContext);
        }

        public ViewCustomerSettingController()
            : this(null)
        {
        }

        [HttpGet]
        //[OutputCache(Duration = 14400)]
        public ActionResult Index()
        {
            string filePath = Server.MapPath("~/App/CustomerSetting/customer-setting.htm");
            string htmlContent = System.IO.File.ReadAllText(filePath);
            string rootUrl = Request.ApplicationPath;
            string min = HttpContext.IsDebuggingEnabled ? string.Empty : ".min";
            string versionId = HttpContext.IsDebuggingEnabled ? "0.0.1" : Guid.NewGuid().ToString("N");

            htmlContent = htmlContent.Replace("{{rootUrl}}", rootUrl);
            htmlContent = htmlContent.Replace("{{min}}", min);
            htmlContent = htmlContent.Replace("{{version}}", versionId);

            return Content(htmlContent, "text/HTML");
        }

        [HttpGet]
        public JsonResult QuerySetting(
            int customerId,
            byte customerLevel,
            byte productId,
            byte? uplineLevel = null)
        {
            var querySetting = new CustomerSettingQuery(
                customerId,
                Level.FromValue<Level>(customerLevel),
                productId,
                uplineLevel != null ? Level.FromValue<Level>(uplineLevel.Value) : null);
            var customerSetting = _querySettingService.GetCustomerSetting(querySetting);

            return new JsonNetResult(customerSetting, JsonRequestBehavior.AllowGet);
        }
    }
}