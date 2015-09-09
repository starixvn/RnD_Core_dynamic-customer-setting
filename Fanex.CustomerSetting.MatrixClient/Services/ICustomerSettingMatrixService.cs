namespace Fanex.CustomerSetting.MatrixClient.Services
{
    using Fanex.CustomerSetting.MatrixClient.Requests;
    using RestSharp;

    public interface ICustomerSettingMatrixService
    {
        IRestResponse QueryAllProducts();

        IRestResponse QueryCustomerSettingMatrix(QuerySettingMatrixRequest query);

        string QueryCustomerSettingMatrixTemplate(QuerySettingMatrixTemplateRequest query);
    }
}