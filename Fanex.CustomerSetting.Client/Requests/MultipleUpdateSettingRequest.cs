namespace Fanex.CustomerSetting.Client.Requests
{
    public class MultipleUpdateSettingRequest : UpdateSettingRequest
    {
        public string CustomerIds { get; set; }

        public string CustomerNames { get; set; }
    }
}