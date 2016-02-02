namespace Fanex.CustomerSetting.MatrixClient.Models
{
    using Fanex.CustomerSetting.MatrixClient.Attributes;

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
            string objTpl = "{{\"applicationPath\": \"{0}\", \"antiForgeryToken\": \"{1}\"}}";

            this.AntiForgeryToken = AntiForgeryExtension.GetTokenHeaderValue();

            return string.Format(objTpl, this.ApplicationPath, this.AntiForgeryToken);
        }
    }
}