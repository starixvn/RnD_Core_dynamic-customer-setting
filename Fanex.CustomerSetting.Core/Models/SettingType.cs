namespace Fanex.CustomerSetting.Core.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.CustomerSetting.Core.Results;
    using Fanex.CustomerSetting.Core.Results.Impl;

    public class SettingType
    {
        public byte Id { get; set; }

        public byte SettingTemplate { get; set; }

        public IEnumerable<ISettingItem> SettingItems { get; set; }

        public virtual string ToXml()
        {
            return string.Empty;
        }

        public IResults Validate()
        {
            return new Results
            {
                Id = Id,
                ResultList = SettingItems.Select(item => item.Validate())
            };
        }
    }
}