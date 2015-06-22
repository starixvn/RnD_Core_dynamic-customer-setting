namespace Fanex.CustomerSetting.Core.Dtos.Params
{
    using Fanex.CustomerSetting.Core.Models;

    public class GetCustomerSettingTypeParams
    {
        public User Actor { get; set; }

        public SettingQuery SettingQuery { get; set; }
    }
}