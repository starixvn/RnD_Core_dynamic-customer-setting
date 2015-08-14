namespace Fanex.CustomerSetting.Client.Services
{
    using Fanex.CustomerSetting.Client.Requests;
    using RestSharp;

    public interface ICustomerSettingService
    {
        IRestResponse QueryCustomerSetting(QuerySettingRequest query);

        IRestResponse UpdateCustomerSetting(UpdateSettingRequest update);
    }
}
