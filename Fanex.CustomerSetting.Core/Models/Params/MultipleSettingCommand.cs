namespace Fanex.CustomerSetting.Core.Models.Params
{
    using System.Collections.Generic;

    public class MultipleSettingCommand : SettingCommand
    {
        public IList<Customer> Customers { get; set; }
    }
}