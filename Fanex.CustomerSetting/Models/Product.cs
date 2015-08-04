namespace Fanex.CustomerSetting.Models
{
    using System;
    using System.Collections.Generic;

    internal class Product : Entity<byte>
    {
        public string Name { get; set; }

        public bool IsDisabled { get; set; }

        public bool IsHidden { get; set; }

        public IEnumerable<SettingType> SettingTypes { get; set; }
    }
}
