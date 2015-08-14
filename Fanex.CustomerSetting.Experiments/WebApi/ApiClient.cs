namespace Fanex.CustomerSetting.Experiments.WebApi
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.CustomerSetting.Client.Requests;
    using Fanex.CustomerSetting.Client.Services;

    public static class ApiClient
    {
        public static void Test(
            byte productId = 255,
            int customerId = 4174,
            byte customerLevel = 3,
            byte? uplineLevel = null)
        {
            var customerSettingService = new CustomerSettingService();

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
                Domain = "Domain Name",
                Language = "en-US"
            };

            var customerSetting = customerSettingService.QueryCustomerSetting(queryRequest);

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
                Domain = queryRequest.Domain,
                Language = queryRequest.Language,
                JsonSetting = customerSetting.Content
            };

            var updateResult = customerSettingService.UpdateCustomerSetting(updateRequest);

        }
    }
}
