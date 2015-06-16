namespace Fanex.CustomerSetting.Core.Models
{
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Core.Dtos.Results;

    public interface ISetting
    {
        Customer Customer { get; set; }

        Product Product { get; set; }

        IEnumerable<SettingType> SettingTypes { get; set; }
    }
}