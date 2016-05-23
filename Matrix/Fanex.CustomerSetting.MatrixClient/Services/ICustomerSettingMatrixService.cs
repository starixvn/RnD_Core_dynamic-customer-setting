namespace Fanex.CustomerSetting.MatrixClient.Services
{
    using Models.Requests;
    using RestSharp;

    public interface ICustomerSettingMatrixService
    {
        IRestResponse QueryAllProducts();

        IRestResponse QueryCustomerSettingMatrix(QuerySettingMatrixRequest query);

        IRestResponse QueryCustomerSettingMatrixTemplate();
    }
}