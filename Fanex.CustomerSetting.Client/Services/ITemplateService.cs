namespace Fanex.CustomerSetting.Client.Services
{
    using Fanex.CustomerSetting.Client.Requests;
    using RestSharp;

    public interface ITemplateService
    {
        IRestResponse QueryCustomerSettingTemplate(QueryTemplateRequest query);

        IRestResponse QueryCustomerSettingMatrixTemplate(QueryTemplateRequest query);
    }
}