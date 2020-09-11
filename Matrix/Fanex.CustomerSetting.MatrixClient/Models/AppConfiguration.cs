namespace Fanex.CustomerSetting.MatrixClient.Models
{
    using Attributes;

    internal class AppConfiguration
    {
        public AppConfiguration(string appPath)
        {
            this.ApplicationPath = appPath;
        }

        public string ApplicationPath { get; set; }

        public string AntiForgeryToken { get; set; }

        public string ToJson()
        {

            AntiForgeryToken = AntiForgeryExtension.GetTokenHeaderValue();

            return string.Format("{{\"applicationPath\": \"{0}\", \"antiForgeryToken\": \"{1}\"}}", ApplicationPath, AntiForgeryToken);
        }
    }
}