namespace Fanex.CustomerSetting.Client.Services
{
    using Fanex.CustomerSetting.Client.Requests;
    using RestSharp;

    public interface ICustomerSettingMatrixService
    {
        IRestResponse QueryAllProducts();

        IRestResponse QueryCustomerSettingMatrix(QuerySettingMatrixRequest query);
    }
}