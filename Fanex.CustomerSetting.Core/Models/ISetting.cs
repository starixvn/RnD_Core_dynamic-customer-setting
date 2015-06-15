namespace Fanex.CustomerSetting.Core.Models
{
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Core.Results;

    public interface ISetting
    {
        Customer Customer { get; set; }

        Product Product { get; set; }

        IList<SettingType> SettingTypes { get; set; }

        IResults Validate();
    }
}