namespace Fanex.CustomerSetting.Client.Requests
{
    public class QuerySettingTemplateRequest
    {
        public bool IsDebug { get; set; }

        public bool IsIe8 { get; set; }

        public string RootUrl { get; set; }

        public string AppConfig { get; set; }

        public string SettingFormQuery { get; set; }

        public byte ProductId { get; set; }
    }
}