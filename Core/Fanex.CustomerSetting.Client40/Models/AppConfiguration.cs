namespace Fanex.CustomerSetting.Client40.Models
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

        public bool Editable { get; set; }

        public string ToJson()
        {

            AntiForgeryToken = AntiForgeryExtension.GetTokenHeaderValue();

#pragma warning disable S1449 // Culture should be specified for "string" operations
            return string.Format("{{\"applicationPath\": \"{0}\", \"antiForgeryToken\": \"{1}\", \"editable\": {2}}}", ApplicationPath, AntiForgeryToken, Editable.ToString().ToLower());
#pragma warning restore S1449 // Culture should be specified for "string" operations
        }
    }
}