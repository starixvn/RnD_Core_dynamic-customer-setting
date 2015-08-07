namespace Fanex.CustomerSetting.Web.ViewModels
{
    using System;

    internal class AppConfiguration
    {
        public AppConfiguration()
            : this(true, "/")
        {
        }

        public AppConfiguration(bool isDebug, string appPath)
        {
            this.IsDebug = isDebug;
            this.ScriptVersion = isDebug ? "0.0.1" : Guid.NewGuid().ToString("N");
            this.RootUrl = appPath;
        }

        public string RootUrl { get; set; }

        public bool IsDebug { get; set; }

        public string ScriptVersion { get; set; }
    }
}