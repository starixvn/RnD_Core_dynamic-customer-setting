namespace Fanex.CustomerSetting.Client40.Controllers
{
    using System.Web.Mvc;
    using Fanex.CustomerSetting.Client40.AntiForgery;
    using Fanex.CustomerSetting.Client40.Models;
    using Fanex.CustomerSetting.Client40.Models.Requests;
    using Fanex.CustomerSetting.Client40.Services;
    using RestSharp;

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
            IRestResponse updateResult;

            SetActorInformation(updateSetting);
            updateResult = _customerSettingService.UpdateCustomerSetting(updateSetting);

            return Content(updateResult.Content);
        }

        [HttpPost]
        [ValidateAntiForgery]
        public ContentResult UpdateMultipleSetting(MultipleUpdateSettingRequest multipleUpdateSetting)
        {
            IRestResponse updateResult;

            SetActorInformation(multipleUpdateSetting);
            updateResult = _customerSettingService.MultipleUpdateCustomerSetting(multipleUpdateSetting);

            return Content(updateResult.Content);
        }

        private static void SetActorInformation(UpdateSettingRequest updateSetting)
        {
            if (updateSetting != null)
            {
                SettingContext settingContext = CustomerSettingRegistration.SettingContext;

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
}