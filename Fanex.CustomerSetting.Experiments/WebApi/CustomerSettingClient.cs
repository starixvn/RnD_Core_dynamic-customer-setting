namespace Fanex.CustomerSetting.Experiments.WebApi
{
    using System;
    using Fanex.CustomerSetting.Client40.Models.Requests;
    using Fanex.CustomerSetting.Client40.Services;

    public static class CustomerSettingClient
    {
        ////public static void TestUpdate(
        ////    byte productId = 255,
        ////    int customerId = 4174,
        ////    byte customerLevel = 3,
        ////    byte? uplineLevel = null)
        ////{
        ////    var customerSettingService = new CustomerSettingService();

        ////    var queryRequest = new QuerySettingRequest
        ////    {
        ////        ProductId = productId,
        ////        CustomerId = customerId,
        ////        CustomerLevel = customerLevel,
        ////        UplineLevel = uplineLevel,

        ////        ActorId = 2,
        ////        ActorLevel = 4,
        ////        ActorName = "ActorName",
        ////        SubAccountId = 3,
        ////        Domain = "Domain Name",
        ////        Language = "en-US"
        ////    };

        ////    var customerSetting = customerSettingService.QueryCustomerSetting(queryRequest);

        ////    //// Change setting in debug mode
        ////    var updateRequest = new UpdateSettingRequest
        ////    {
        ////        UplineLevel = queryRequest.UplineLevel,
        ////        Setting = customerSetting.Content,

        ////        ActorId = queryRequest.ActorId,
        ////        ActorLevel = queryRequest.ActorLevel,
        ////        ActorName = queryRequest.ActorName,
        ////        SubAccountId = queryRequest.SubAccountId,
        ////        Domain = queryRequest.Domain,
        ////        Language = queryRequest.Language,
        ////        ClientIP = "192.168.1.1"
        ////    };

        ////    var updateResult = customerSettingService.UpdateCustomerSetting(updateRequest);
        ////}

        ////public static void TestMultipleUpdate(
        ////    byte productId = 255,
        ////    string customerIds = "4174,4300,4356",
        ////    string customerNames = "mrm,mbrm,MYRM",
        ////    byte customerLevel = 3,
        ////    byte? uplineLevel = null)
        ////{
        ////    var customerSettingService = new CustomerSettingService();

        ////    var queryRequest = new QuerySettingRequest
        ////    {
        ////        ProductId = productId,
        ////        CustomerId = Convert.ToInt32(customerIds.Split(',')[0]),
        ////        CustomerLevel = customerLevel,
        ////        UplineLevel = uplineLevel,

        ////        ActorId = 2,
        ////        ActorLevel = 4,
        ////        ActorName = "ActorName",
        ////        SubAccountId = 3,
        ////        Domain = "Domain Name",
        ////        Language = "en-US"
        ////    };

        ////    var customerSetting = customerSettingService.QueryCustomerSetting(queryRequest);

        ////    //// Change setting in debug mode
        ////    var multipleUpdateRequest = new MultipleUpdateSettingRequest
        ////    {
        ////        CustomerIds = customerIds,
        ////        CustomerNames = customerNames,
        ////        UplineLevel = queryRequest.UplineLevel,
        ////        Setting = customerSetting.Content,

        ////        ActorId = queryRequest.ActorId,
        ////        ActorLevel = queryRequest.ActorLevel,
        ////        ActorName = queryRequest.ActorName,
        ////        SubAccountId = queryRequest.SubAccountId,
        ////        Domain = queryRequest.Domain,
        ////        Language = queryRequest.Language,
        ////        ClientIP = "192.168.1.1"
        ////    };

        ////    var updateResult = customerSettingService.MultipleUpdateCustomerSetting(multipleUpdateRequest);
        ////}
    }
}