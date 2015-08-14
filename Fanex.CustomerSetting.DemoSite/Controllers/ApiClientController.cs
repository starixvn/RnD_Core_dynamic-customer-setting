namespace Fanex.CustomerSetting.DemoSite.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Fanex.CustomerSetting.Client.Configurations;
    using Fanex.CustomerSetting.Client.Requests;
    using Fanex.CustomerSetting.Client.Services;
    using Newtonsoft.Json;

    public class ApiClientController : Controller
    {
        private ICustomerSettingService _customerSettingService;

        public ApiClientController(ICustomerSettingService customerSettingService)
        {
            _customerSettingService = customerSettingService ?? new CustomerSettingService();
        }

        public ApiClientController()
            : this(null)
        {
        }

        //
        // GET: /CusomterSettingClient/
        public ActionResult Index(
            byte productId = 255,
            int customerId = 4174,
            byte customerLevel = 3,
            byte? uplineLevel = null)
        {
            var queryRequest = new QuerySettingRequest 
            {
                ProductId = productId,
                CustomerId = customerId,
                CustomerLevel = customerLevel,
                UplineLevel = uplineLevel,
                ActorId = 2,
                ActorLevel = 4,
                ActorName = "ActorName",
                SubAccountId = 3,
                Domain = HttpContext.Request.Url.Host,
                Language = "en-US"
            };

            var customerSetting = _customerSettingService.QueryCustomerSetting(queryRequest);

            //// change setting in debug mode
            UpdateSettingRequest updateRequest = new UpdateSettingRequest
                {
                    ProductId = queryRequest.ProductId,
                    CustomerId = queryRequest.CustomerId,
                    CustomerLevel = queryRequest.CustomerLevel,
                    UplineLevel = queryRequest.UplineLevel,
                    ActorId = queryRequest.ActorId,
                    ActorLevel = queryRequest.ActorLevel,
                    ActorName = queryRequest.ActorName,
                    SubAccountId = queryRequest.SubAccountId,
                    Domain = HttpContext.Request.Url.Host,
                    Language = queryRequest.Language,
                    JsonSetting = customerSetting.Content
                };

            var updateResult = _customerSettingService.UpdateCustomerSetting(updateRequest);

            return new EmptyResult();
        }

    }
}
