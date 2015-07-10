namespace Fanex.CustomerSetting.Dtos.Results
{
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Models;

    public class PreUpdateMultipleCustomersSettingTypeResult
    {
        public IEnumerable<int> CustomerIdsNeedForSingleUpdating { get; set; }

        public bool IsStopUpdating { get; set; }

        public bool IsReturnResultToUser { get; set; }

        public UpdatingSettingResult Result { get; set; }
    }
}