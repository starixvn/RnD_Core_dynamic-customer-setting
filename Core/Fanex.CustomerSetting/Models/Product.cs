namespace Fanex.CustomerSetting.Models
{
    using System.Collections.Generic;

    public class Product : Entity<byte>
    {
        public string Name { get; set; }

        public bool IsDisabled { get; set; }

        public bool IsHidden { get; set; }

        public IEnumerable<SettingType> SettingTypes { get; set; }
    }
}