namespace Fanex.CustomerSetting.SettingType22.DataAccesses
{
    using System.Collections.Generic;
    using CustomerSetting.Models;
    using CustomerSetting.Models.Enumerations;
    using CustomerSetting.Models.Requests;
    using CustomerSetting.Models.Results;
    using Models;
    using Models.Requests;

    internal interface IDataAccess
    {
        IEnumerable<WinLossLimitItem> GetCustomerWinLossLimit(CustomerSettingQuery query);

        IResult UpdateCustomerWinLossLimit(ISettingContext settingContext, int customerId, Level customerLevel, Level uplineLevel, string winLossLimitXml);

        IEnumerable<CurrentWinLossAmountItem> GetCustomerCurrentWinLossAmount(CurrentWinLossAmountQuery query);
    }
}