namespace Fanex.CustomerSetting.Dtos.Params
{
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Models;

    public class MultipleSettingCommand : SettingCommand
    {
        public IEnumerable<Customer> Customers { get; set; }
    }
}