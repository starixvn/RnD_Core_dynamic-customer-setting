namespace Fanex.CustomerSetting.Models
{
    using System.Collections.Generic;

    public interface ISetting
    {
        Customer Customer { get; set; }

        Product Product { get; set; }

        IEnumerable<SettingType> SettingTypes { get; set; }
    }
}