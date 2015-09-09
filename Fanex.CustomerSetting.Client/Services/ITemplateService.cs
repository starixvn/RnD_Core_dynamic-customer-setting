namespace Fanex.CustomerSetting.Client.Services
{
    using Fanex.CustomerSetting.Client.Requests;
    using RestSharp;

    public interface ITemplateService
    {
        string QueryCustomerSettingTemplate(QuerySettingTemplateRequest query);

        string QueryCustomerSettingMatrixTemplate(QuerySettingMatrixTemplateRequest query);
    }
}