namespace Fanex.CustomerSetting.Core.Models.Params
{
    using System.Collections.Generic;

    public class MultipleSettingCommand : SettingCommand
    {
        private IList<Customer> Customers { get; set; }
    }
}