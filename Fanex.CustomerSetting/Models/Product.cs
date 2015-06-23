namespace Fanex.CustomerSetting.Models
{
    using System.Collections.Generic;

    public class Product
    {
        public byte Id { get; set; }

        public string Name { get; set; }

        public bool IsDisabled { get; set; }

        public bool IsHidden { get; set; }

        public IEnumerable<SettingType> SettingTypes { get; set; }
    }
}
