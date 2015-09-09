namespace Fanex.CustomerSetting.Client.Requests
{
    public class QuerySettingTemplateRequest : SettingTemplateRequest
    {
        public string SettingFormQuery { get; set; }

        public byte ProductId { get; set; }
    }
}