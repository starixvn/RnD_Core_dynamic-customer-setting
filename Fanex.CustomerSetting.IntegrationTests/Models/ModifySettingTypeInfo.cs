namespace Fanex.CustomerSetting.IntegrationTests.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.CustomerSetting.Models;

    public class ModifySettingTypeInfo
    {
        public ISettingItem SettingItem { get; set; }

        public byte SettingTypeId { get; set; }
    }
}
