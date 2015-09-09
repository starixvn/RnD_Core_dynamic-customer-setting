namespace Fanex.CustomerSetting.MatrixClient.Requests
{
    public class QuerySettingMatrixTemplateRequest
    {
        public bool IsDebug { get; set; }

        public string RootUrl { get; set; }

        public string AppConfig { get; set; }

        public string CustomerName { get; set; }
    }
}
