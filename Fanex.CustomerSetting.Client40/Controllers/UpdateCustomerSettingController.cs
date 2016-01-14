namespace Fanex.CustomerSetting.Client40.Controllers
{
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Mvc;
    using Fanex.CustomerSetting.Client40.Attributes;
    using Fanex.CustomerSetting.Client40.Models;
    using Fanex.CustomerSetting.Client40.Models.Requests;
    using Fanex.CustomerSetting.Client40.Services;
    using RestSharp;

    [ExceptionHandlingAttribute, ValidateAntiForgery, LogInfoFilter]
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

        [SuppressMessage("Microsoft.Naming", "CA1719:ParameterNamesShouldNotMatchMemberNames", MessageId = "0#", Justification = "Reviewed!"), HttpPost]
        [CustomerSettingAuth(IsUpdate = true, IsMultipleUpdate = false)]
        public ContentResult UpdateSetting(UpdateSettingRequest updateSetting)
        {
            SetActorInformation(updateSetting);
            IRestResponse updateResult = _customerSettingService.UpdateCustomerSetting(updateSetting);
            Response.StatusCode = (int)updateResult.StatusCode;
            Response.TrySkipIisCustomErrors = true;

            return Content(updateResult.Content);
        }

        [HttpPost]
        [CustomerSettingAuth(IsUpdate = true, IsMultipleUpdate = true)]
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