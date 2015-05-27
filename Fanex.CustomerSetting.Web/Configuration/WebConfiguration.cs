namespace Fanex.CustomerSetting.Web.Configuration
{
    public static class WebConfiguration
    {
        public static WebSettings Settings { get; private set; }

        public static void Start()
        {
            var webSettings = new WebSettings
            {
                IsMinifiedVersion = true
            };

            Start(webSettings);
        }

        public static void Start(WebSettings webSettings)
        {
            Settings = webSettings;
        }
    }
}