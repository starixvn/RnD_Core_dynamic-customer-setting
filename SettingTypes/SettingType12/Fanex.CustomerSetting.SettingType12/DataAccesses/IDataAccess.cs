namespace Fanex.CustomerSetting.SettingType12.DataAccesses
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
        IEnumerable<LossLimitItem> GetCustomerLossLimit(CustomerSettingQuery query);

        IResult UpdateCustomerLossLimit(ISettingContext settingContext, int customerId, Level customerLevel, Level uplineLevel, string lossLimitXml);

        CurrentLossAmountItem GetCustomerCurrentLossAmount(CurrentLossAmountQuery query);
    }
}