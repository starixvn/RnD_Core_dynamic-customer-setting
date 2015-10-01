namespace Fanex.CustomerSetting.Models
{
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Models.Enumerations;

    public interface ISetting
    {
        CustomerInfo CustomerInfo { get; set; }

        int CustomerId { get; set; }

        Level CustomerLevel { get; set; }

        byte ProductId { get; set; }

        string ProductName { get; }

        IEnumerable<SettingType> SettingTypes { get; set; }
    }
}