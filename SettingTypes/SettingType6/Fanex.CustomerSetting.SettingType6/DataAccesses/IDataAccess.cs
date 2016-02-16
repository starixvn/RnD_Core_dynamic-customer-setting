namespace Fanex.CustomerSetting.SettingType6.DataAccesses
{
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Models.Enumerations;
    using Fanex.CustomerSetting.Models.Requests;
    using Fanex.CustomerSetting.Models.Results;
    using Fanex.CustomerSetting.SettingType6.Models;

    internal interface IDataAccess
    {
        IEnumerable<WinLossLimitItem> GetCustomerWinLossLimit(CustomerSettingQuery query);

        IResult UpdateCustomerWinLossLimit(ISettingContext settingContext, int customerId, Level customerLevel, Level uplineLevel, string winLossLimitXml);

        CurrentWinLossAmountItem GetCustomerCurrentWinLossAmount(int customerId, short sportTypeId, short betTypeId);
    }
}