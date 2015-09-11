namespace Fanex.CustomerSetting.Client40.Controllers
{
    using System.Web.Mvc;
    using Fanex.CustomerSetting.Client40.AntiForgery;
    using Fanex.CustomerSetting.Client40.Context;
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
                updateSetting.ActorId = SettingContextManager.SettingContext.ActorId;
                updateSetting.ActorLevel = SettingContextManager.SettingContext.ActorLevel;
                updateSetting.ActorName = SettingContextManager.SettingContext.ActorName;
                updateSetting.SubAccountId = SettingContextManager.SettingContext.SubAccountId;
                updateSetting.Domain = SettingContextManager.SettingContext.Domain;
                updateSetting.Language = SettingContextManager.SettingContext.Language;
                updateSetting.ClientIP = SettingContextManager.SettingContext.IP;
            }
        }
    }
}