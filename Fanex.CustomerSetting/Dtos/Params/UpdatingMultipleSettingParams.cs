namespace Fanex.CustomerSetting.Dtos.Params
{
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Models;

    public class UpdatingMultipleSettingParams : UpdatingSettingParams
    {
        public IEnumerable<Customer> Customers { get; set; }
    }
}