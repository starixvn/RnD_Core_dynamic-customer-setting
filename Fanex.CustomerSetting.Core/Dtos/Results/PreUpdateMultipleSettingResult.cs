namespace Fanex.CustomerSetting.Core.Dtos.Results
{
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Core.Models;

    public class PreUpdateMultipleSettingResult
    {
        public IEnumerable<Customer> CustomersNeedForUpdating { get; set; }

        public IResult Result { get; set; }
    }
}