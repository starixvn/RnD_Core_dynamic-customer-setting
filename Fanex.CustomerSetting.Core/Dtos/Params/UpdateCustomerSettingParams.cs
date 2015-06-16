namespace Fanex.CustomerSetting.Core.Dtos.Params
{
    using Fanex.CustomerSetting.Core.Enumerations;
    using Fanex.CustomerSetting.Core.Models;

    public class UpdateCustomerSettingParams
    {
        public ISettingContext SettingContext { get; set; }

        public Customer Customer { get; set; }

        public SettingType SettingType { get; set; }

        public CustomerLevel? UplineLevel { get; set; }
    }
}