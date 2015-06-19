namespace Fanex.CustomerSetting.Core.Dtos.Params
{
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Core.Models;

    public class MultipleSettingCommand : SettingCommand
    {
        public IEnumerable<Customer> Customers { get; set; }
    }
}