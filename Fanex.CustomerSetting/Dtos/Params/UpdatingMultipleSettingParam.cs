namespace Fanex.CustomerSetting.Dtos.Params
{
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Models;

    public class UpdatingMultipleSettingParam : UpdatingSettingParam
    {
        public IEnumerable<Customer> Customers { get; set; }
    }
}