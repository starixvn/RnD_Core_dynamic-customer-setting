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
        private readonly ISettingService _settingService;

        public BackendTestController()
        {
            _settingService = new SettingService(SettingContextManager.SettingContext);
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
            var setting = _settingService.GetCustomerSetting(
                new QuerySettingParam
            {
                CustomerId = customerId,
                CustomerLevel = customerLevel,
                ProductId = productId,
                UplineLevel = actualUplineLevel
            });

            UpdateSetting(setting, actualUplineLevel);

            return new EmptyResult();
        }

        public ActionResult EditMultiple(
            string customerIds = "1277,1284,1290",
            CustomerLevel customerLevel = CustomerLevel.Member,
            byte productId = 255,
            CustomerLevel uplineLevel = CustomerLevel.Agent)
        {
            var actualUplineLevel = customerLevel == CustomerLevel.Member ? (CustomerLevel?)uplineLevel : null;
            IEnumerable<CustomerInfo> customers = customerIds.Split(',')
                .Select(customerId => new CustomerInfo
                {
                    CustomerId = Convert.ToInt32(customerId),
                    Level = customerLevel
                });

            var setting = _settingService.GetCustomerSetting(
                new QuerySettingParam
                {
                    CustomerId = customers.FirstOrDefault().CustomerId,
                    CustomerLevel = customerLevel,
                    ProductId = productId,
                    UplineLevel = actualUplineLevel
                });

            UpdateMultipleSetting(new UpdatingMultipleSettingParam 
            {
                CustomerIds = customers,
                Setting = setting,
                UplineLevel = actualUplineLevel
            });

            return new EmptyResult();
        }

        private void UpdateMultipleSetting(UpdatingMultipleSettingParam updateMultipleParam)
        {
            var updatedResult = _settingService.UpdateMultipleCustomerSetting(updateMultipleParam);
        }

        private void UpdateSetting(ISetting setting, CustomerLevel? uplineLevel)
        {
            var result = _settingService.UpdateCustomerSetting(new UpdatingSettingParam
                {
                    Setting = setting,
                    UplineLevel = uplineLevel
                });
        }
    }
}