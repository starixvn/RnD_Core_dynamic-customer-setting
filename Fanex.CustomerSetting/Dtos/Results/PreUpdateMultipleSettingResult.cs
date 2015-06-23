namespace Fanex.CustomerSetting.Dtos.Results
{
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Models;

    public class PreUpdateMultipleSettingResult
    {
        public IEnumerable<Customer> CustomersNeedForUpdating { get; set; }

        public IResult Result { get; set; }
    }
}