namespace Fanex.CustomerSetting.Client40.Models.Requests
{
    public class MultipleUpdateSettingRequest : UpdateSettingRequest
    {
        public string CustomerIds { get; set; }

        public string CustomerNames { get; set; }
    }
}