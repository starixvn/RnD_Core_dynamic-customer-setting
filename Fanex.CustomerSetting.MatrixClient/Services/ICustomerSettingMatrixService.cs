namespace Fanex.CustomerSetting.MatrixClient.Services
{
    using Fanex.CustomerSetting.MatrixClient.Models.Requests;
    using RestSharp;

    public interface ICustomerSettingMatrixService
    {
        IRestResponse QueryAllProducts();

        IRestResponse QueryCustomerSettingMatrix(QuerySettingMatrixRequest query);

        IRestResponse QueryCustomerSettingMatrixTemplate();
    }
}