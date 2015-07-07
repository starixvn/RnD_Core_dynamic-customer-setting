namespace Fanex.CustomerSetting.Models
{
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Enumerations;

    public interface ISetting
    {
        CustomerInfo CustomerInfo { get; }

        int CustomerId { get; set; }

        CustomerLevel CustomerLevel { get; set; }

        byte ProductId { get; set; }

        string ProductName { get; }

        IEnumerable<SettingType> SettingTypes { get; set; }
    }
}