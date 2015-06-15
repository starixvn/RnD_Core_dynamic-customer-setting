namespace Fanex.CustomerSetting.Core.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.CustomerSetting.Core.Results;

    public class Setting : ISetting
    {
        public Customer Customer { get; set; }

        public Product Product { get; set; }

        public IList<SettingType> SettingTypes { get; set; }

        public IResults Validate()
        {
            throw new NotImplementedException();
        }
    }
}
