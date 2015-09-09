namespace Fanex.CustomerSetting.WebAPI.Models.Request
{
    using System;
    using Fanex.CustomerSetting.WebAPI.Helpers;

    public class QueryTemplateRequest
    {
        public bool IsDebug { get; set; }

        public string ScriptVersion
        {
            get { return IsDebug ? "1.0.0" : Guid.NewGuid().ToString("N"); }
        }

        public bool IsIe8 { get; set; }
    }
}