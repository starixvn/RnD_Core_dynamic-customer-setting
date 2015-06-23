namespace Fanex.CustomerSetting.Models
{
    using System;
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Dtos.Results;

    public class Setting : ISetting
    {
        public Customer Customer { get; set; }

        public Product Product { get; set; }

        public IEnumerable<SettingType> SettingTypes { get; set; }
    }
}