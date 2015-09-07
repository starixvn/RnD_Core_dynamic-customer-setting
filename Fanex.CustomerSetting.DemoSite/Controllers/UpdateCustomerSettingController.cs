namespace Fanex.CustomerSetting.DemoSite.Controllers
{
    using System.Web.Mvc;
    using Fanex.CustomerSetting.Client.Requests;
    using Fanex.CustomerSetting.Client.Services;
    using Fanex.CustomerSetting.DemoSite.Contexts;
    using Fanex.CustomerSetting.DemoSite.Helpers;

    public class UpdateCustomerSettingController : Controller
    {
        private readonly ICustomerSettingService _customerSettingService;

        public UpdateCustomerSettingController(ICustomerSettingService customerSettingService)
        {
            _customerSettingService = customerSettingService ?? new CustomerSettingService();
        }

        public UpdateCustomerSettingController()
            : this(null)
        {
        }

        [HttpPost]
        [ValidateAntiForgery]
        public ContentResult UpdateSetting(UpdateSettingRequest updateSetting)
        {
            SetActorInformation(updateSetting);

            var updateResult = _customerSettingService.UpdateCustomerSetting(updateSetting);

            return Content(updateResult.Content);
        }

        [HttpPost]
        [ValidateAntiForgery]
        public ContentResult UpdateMultipleSetting(MultipleUpdateSettingRequest multipleUpdateSetting)
        {
            SetActorInformation(multipleUpdateSetting);

            var updateResult = _customerSettingService.MultipleUpdateCustomerSetting(multipleUpdateSetting);

            return Content(updateResult.Content);
        }

        private static void SetActorInformation(UpdateSettingRequest updateSetting)
        {
            var settingContext = SettingContextManager.SettingContext;

            updateSetting.ActorId = settingContext.ActorId;
            updateSetting.ActorLevel = settingContext.ActorLevel;
            updateSetting.ActorName = settingContext.ActorName;
            updateSetting.SubAccountId = settingContext.SubAccountId;
            updateSetting.Domain = settingContext.Domain;
            updateSetting.Language = settingContext.Language;
            updateSetting.ClientIP = settingContext.IP;
        }
    }
}