namespace Fanex.CustomerSetting.DemoSite.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.CustomerSetting.Dtos.Params;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Services;
    using Fanex.CustomerSetting.Services.Impl;
    using Fanex.CustomerSetting.Web.Contexts;

    public class BackendTestController : Controller
    {
        private readonly IUpdateService _settingService;

        public BackendTestController()
        {
            _settingService = new UpdateService(SettingContextManager.SettingContext);
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Edit(
            int customerId = 1277,
            CustomerLevel customerLevel = CustomerLevel.Member,
            byte productId = 255,
            CustomerLevel uplineLevel = CustomerLevel.Agent)
        {
            var actualUplineLevel = customerLevel == CustomerLevel.Member ? (CustomerLevel?)uplineLevel : null;
            var querySettingParam = new QuerySettingParam(customerId, customerLevel, productId, uplineLevel);
            var setting = _settingService.GetCustomerSetting(querySettingParam);

            UpdateSetting(setting, actualUplineLevel);

            return new EmptyResult();
        }

        public ActionResult EditMultiple(
            string customerIds = "1277,1284,1290",
            string customerNames = "1277,1284,1290",
            CustomerLevel customerLevel = CustomerLevel.Member,
            byte productId = 255,
            CustomerLevel uplineLevel = CustomerLevel.Agent)
        {
            var actualUplineLevel = customerLevel == CustomerLevel.Member ? (CustomerLevel?)uplineLevel : null;
            IEnumerable<int> customerIdList = customerIds.Split(',').Select(customerId => Convert.ToInt32(customerId));
            IEnumerable<string> customerNameList = customerNames.Split(',').ToList();

            var querySettingParam = new QuerySettingParam(customerIdList.FirstOrDefault(), customerLevel, productId, uplineLevel);
            var setting = _settingService.GetCustomerSetting(querySettingParam);

            var updateMultipleParams = new UpdatingMultipleSettingParam(setting, customerIdList, customerNameList, actualUplineLevel);

            UpdateMultipleSetting(updateMultipleParams);

            return new EmptyResult();
        }

        private void UpdateMultipleSetting(UpdatingMultipleSettingParam updateMultipleParam)
        {
            var updatedResult = _settingService.UpdateMultipleCustomerSetting(updateMultipleParam);
        }

        private void UpdateSetting(ISetting setting, CustomerLevel? uplineLevel)
        {
            var result = _settingService.UpdateCustomerSetting(new UpdatingSettingParam(setting, uplineLevel));
        }
    }
}