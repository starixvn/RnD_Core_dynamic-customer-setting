namespace Fanex.CustomerSetting.Models
{
    using System.Collections.Generic;

    public interface ISetting
    {
        Customer Customer { get; set; }

        byte ProductId { get; set; }

        string ProductName { get; set; }

        IEnumerable<SettingType> SettingTypes { get; set; }
    }
}