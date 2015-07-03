namespace Fanex.CustomerSetting.DemoSite.Controllers
{
    using System.Web.Mvc;
    using Fanex.CustomerSetting.Dtos.Params;
    using Fanex.CustomerSetting.Enumerations;
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

        public ActionResult Index(
            int customerId = 2,
            CustomerLevel customerLevel = CustomerLevel.Super,
            byte producId = 255,
            CustomerLevel uplineLevel = CustomerLevel.Super)
        {
            var setting = _settingService.GetCustomerSetting(
                new QuerySettingParam
            {
                CustomerId = customerId,
                CustomerLevel = customerLevel,
                ProductId = producId,
                UplineLevel = uplineLevel
            });
            return View();
        }

        public ActionResult Login(
            int userLoginId = 8,
            string userLoginName = "LoginName",
            UserLevel userLevel = UserLevel.Company,
            int subAccountId = 0)
        {
            SettingContextManager.Init(userLoginId, userLoginName, userLevel, subAccountId);
            return RedirectToAction("Index");
        }
    }
}