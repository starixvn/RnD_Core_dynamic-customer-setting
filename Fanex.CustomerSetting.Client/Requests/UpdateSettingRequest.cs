namespace Fanex.CustomerSetting.Client.Requests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class UpdateSettingRequest : SettingRequest
    {
        public string JsonSetting { get; set; }
    }
}
