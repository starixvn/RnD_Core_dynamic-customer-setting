namespace Fanex.CustomerSetting.Client40.Controllers
{
    using System.Web.Mvc;
    using Fanex.CustomerSetting.Client40.AntiForgery;
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
                updateSetting.ActorId = CustomerSettingRegistration.SettingContext.ActorId;
                updateSetting.ActorLevel = CustomerSettingRegistration.SettingContext.ActorLevel;
                updateSetting.ActorName = CustomerSettingRegistration.SettingContext.ActorName;
                updateSetting.SubAccountId = CustomerSettingRegistration.SettingContext.SubAccountId;
                updateSetting.Domain = CustomerSettingRegistration.SettingContext.Domain;
                updateSetting.Language = CustomerSettingRegistration.SettingContext.Language;
                updateSetting.ClientIP = CustomerSettingRegistration.SettingContext.IP;
            }
        }
    }
}