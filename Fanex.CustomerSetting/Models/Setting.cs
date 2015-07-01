namespace Fanex.CustomerSetting.Models
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class Setting : ISetting
    {
        public Customer Customer { get; set; }

        public byte ProductId { get; set; }

        public string ProductName { get; set; }

        public IEnumerable<SettingType> SettingTypes { get; set; }
    }
}