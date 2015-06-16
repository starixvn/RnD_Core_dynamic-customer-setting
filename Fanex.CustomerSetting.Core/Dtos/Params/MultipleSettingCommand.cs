namespace Fanex.CustomerSetting.Core.Dtos.Params
{
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Core.Models;

    public class MultipleSettingCommand : SettingCommand
    {
        public IList<Customer> Customers { get; set; }
    }
}