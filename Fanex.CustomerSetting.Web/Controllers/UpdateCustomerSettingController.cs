namespace Fanex.CustomerSetting.Web.Controllers
{
    using System.Web.Mvc;
    using Fanex.CustomerSetting.Dtos.Params;
    using Fanex.CustomerSetting.Dtos.Results;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Services;
    using Fanex.CustomerSetting.Services.Impl;
    using Fanex.CustomerSetting.Web.Contexts;

    public class UpdateCustomerSettingController : Controller
    {
        private readonly IUpdateSettingService _customerSettingService;

        public UpdateCustomerSettingController(IUpdateSettingService customerSettingService)
        {
            _customerSettingService = customerSettingService ?? new UpdateSettingService(SettingContextManager.SettingContext);
        }

        public UpdateCustomerSettingController()
            : this(null)
        { 
        }

        [HttpPost]
        public JsonResult Index(UpdateSettingParam updateSetting)
        {
            var result = _customerSettingService.UpdateCustomerSetting(updateSetting);

            return Json(result);
        }

        [HttpPost]
        public JsonResult Index(UpdateMultipleSettingParam updateMulitpleSetting)
        {
            var result = _customerSettingService.UpdateCustomerSetting(updateMulitpleSetting);

            return Json(result);
        }
    }
}