namespace Fanex.CustomerSetting.Web.Controllers
{
    using System;
    using System.Web.Mvc;
    using Fanex.CustomerSetting.Dtos.Params;
    using Fanex.CustomerSetting.Models;
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
            var versionId = new Guid();

            if (rootUrl.EndsWith("/", StringComparison.CurrentCulture))
            {
                rootUrl = rootUrl.Substring(0, rootUrl.Length - 1);
            }

            htmlContent = htmlContent.Replace("{{rootUrl}}", rootUrl);
            htmlContent = htmlContent.Replace("{{min}}", min);
            htmlContent = htmlContent.Replace("{{version}}", versionId.ToString());

            return Content(htmlContent, "text/HTML");
        }

        [HttpGet]
        public JsonResult GetSetting(QuerySettingParam querySetting)
        {
            var customerSetting = _querySettingService.GetCustomerSetting(querySetting);

            return Json(customerSetting, JsonRequestBehavior.AllowGet);
        }
    }
}