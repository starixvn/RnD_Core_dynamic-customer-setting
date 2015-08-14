namespace Fanex.CustomerSetting.Client.Services
{
    using Fanex.CustomerSetting.Client.Requests;
    using RestSharp;

    public interface ICustomerSettingService
    {
        RestResponse QueryCustomerSetting(QuerySettingRequest query);
    }
}
