namespace Fanex.CustomerSetting.Dtos.Results
{
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Models;

    public class PreUpdateMultipleCustomersSettingTypeResult
    {
        public IEnumerable<Customer> CustomersNeedForSingleUpdating { get; set; }

        public bool IsStopUpdating { get; set; }

        public bool IsReturnResultToUser { get; set; }

        public IResult Result { get; set; }
    }
}