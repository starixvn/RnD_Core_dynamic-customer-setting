namespace Fanex.CustomerSetting.Client.Services
{
    using Fanex.CustomerSetting.Client.Requests;
    using RestSharp;

    public interface ICustomerSettingMatrixService
    {
        IRestResponse QueryAllProductNames();

        IRestResponse QueryCustomerSettingMatrix(QuerySettingMatrixRequest query);
    }
}