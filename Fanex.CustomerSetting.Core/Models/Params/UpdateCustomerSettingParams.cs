namespace Fanex.CustomerSetting.Core.Models.Params
{
    using Fanex.CustomerSetting.Core.Enumerations;

    public class UpdateCustomerSettingParams
    {
        public ISettingContext SettingContext { get; set; }

        public Customer Customer { get; set; }

        public SettingType SettingType { get; set; }

        public CustomerLevel? UplineLevel { get; set; }
    }
}