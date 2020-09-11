namespace Fanex.CustomerSetting.Client40.Controllers
{
    using System.Web.Mvc;
    using Attributes;
    using Models;
    using Models.Requests;
    using RestSharp;
    using Services;

    [ExceptionHandling, ValidateAntiForgeryAttribute, LogInfoFilterAttribute]
    public class UpdateCustomerSettingController : Controller
    {
        private readonly ICustomerSettingService _customerSettingService;

        public UpdateCustomerSettingController()
            : this(null)
        {
        }

        internal UpdateCustomerSettingController(ICustomerSettingService customerSettingService)
        {
            _customerSettingService = customerSettingService ?? new CustomerSettingService();
        }

        [HttpPost]
        [CustomerSettingAuthAttribute(IsUpdate = true, IsMultipleUpdate = false)]
        public ContentResult UpdateSetting(UpdateSettingRequest updateSettingRequest)
        {
            SetActorInformation(updateSettingRequest);
            IRestResponse updateResult = _customerSettingService.UpdateCustomerSetting(updateSettingRequest);
            Response.StatusCode = (int)updateResult.StatusCode;
            Response.TrySkipIisCustomErrors = true;

            return Content(updateResult.Content);
        }

        [HttpPost]
        [CustomerSettingAuthAttribute(IsUpdate = true, IsMultipleUpdate = true)]
        public ContentResult UpdateMultipleSetting(MultipleUpdateSettingRequest multipleUpdateSetting)
        {
            IRestResponse updateResult;

            SetActorInformation(multipleUpdateSetting);
            updateResult = _customerSettingService.MultipleUpdateCustomerSetting(multipleUpdateSetting);
            Response.StatusCode = (int)updateResult.StatusCode;
            Response.TrySkipIisCustomErrors = true;

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
                updateSetting.System = settingContext.System;
            }
        }
    }
}