namespace Fanex.CustomerSetting.Web.Controllers
{
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
        public JsonResult Index(QuerySettingParam querySetting)
        {
            var customerSetting = _querySettingService.GetCustomerSetting(querySetting);

            return Json(customerSetting, JsonRequestBehavior.AllowGet);
        }
    }
}