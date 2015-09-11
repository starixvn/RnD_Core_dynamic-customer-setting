namespace Fanex.CustomerSetting.Client40.Models
{
    using System;
    using Fanex.CustomerSetting.Client40.AntiForgery;
    using Fanex.CustomerSetting.Client40.Configurations;

    public class AppConfiguration
    {
        public AppConfiguration()
            : this(true, "/")
        {
        }

        public AppConfiguration(bool isDebug, string appPath)
        {
            this.IsDebug = isDebug;
            this.ScriptVersion = isDebug ? "0.0.1" : Guid.NewGuid().ToString("N");
            this.ApplicationPath = appPath;
            this.CdnUri = CustomerSettingConfiguration.Settings.CdnUri ?? new Uri(appPath);
        }

        public string ApplicationPath { get; set; }

        public bool IsDebug { get; set; }

        public string ScriptVersion { get; set; }

        public string AntiForgeryToken { get; set; }

        public Uri CdnUri { get; set; }

        public string ToJson()
        {
            string objTpl = "{{\"applicationPath\": \"{0}\", \"antiForgeryToken\": \"{1}\"}}";

            this.AntiForgeryToken = AntiForgeryExtension.GetTokenHeaderValue();

            return string.Format(objTpl, this.ApplicationPath, this.AntiForgeryToken);
        }
    }
}