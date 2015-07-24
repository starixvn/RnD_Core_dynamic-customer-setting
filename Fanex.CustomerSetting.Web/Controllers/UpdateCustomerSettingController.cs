namespace Fanex.CustomerSetting.Web.Controllers
{
    using System.Web.Mvc;
    using Fanex.CustomerSetting.Models.Requests;
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
        public JsonResult Index(UpdateCustomerSettingCommand updateSetting)
        {
            var result = _customerSettingService.Update(updateSetting);

            return Json(result);
        }

        [HttpPost]
        public JsonResult Index(MultipleUpdateCustomerSettingCommand updateMulitpleSetting)
        {
            var result = _customerSettingService.Update(updateMulitpleSetting);

            return Json(result);
        }
    }
}