namespace Fanex.CustomerSetting.Core.Models.Params
{
    using Fanex.CustomerSetting.Core.Enumerations;

    public class UpdateCustomerSettingParams
    {
        private ISettingContext SettingContext { get; set; }

        private Customer Customer { get; set; }

        private SettingType SettingType { get; set; }

        private CustomerLevel? UplineLevel { get; set; }
    }
}