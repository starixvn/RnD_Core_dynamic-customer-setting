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
        [ValidateAntiForgery]
        public JsonResult UpdateSetting(UpdateCustomerSettingCommand updateSetting)
        {
            var result = _customerSettingService.Update(updateSetting);

            return new JsonNetResult(result);
        }

        [HttpPost]
        [ValidateAntiForgery]
        public JsonResult UpdateMultipleSetting(MultipleUpdateCustomerSettingCommand updateMulitpleSetting)
        {
            var result = _customerSettingService.Update(updateMulitpleSetting);

            return new JsonNetResult(result);
        }
    }
}