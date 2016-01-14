namespace Fanex.CustomerSetting.SettingType5.DataAccesses
{
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Models.Enumerations;
    using Fanex.CustomerSetting.Models.Requests;
    using Fanex.CustomerSetting.Models.Results;
    using Fanex.CustomerSetting.SettingType5.Dtos;

    internal interface IDataAccess
    {
        IEnumerable<ChargeCommissionDto> GetCustomerChargeCommissionRawData(CustomerSettingQuery query);

        IResult UpdateCustomerChargeCommission(
            ISettingContext settingContext,
            int customerId,
            Level customerLevel,
            Level uplineLevel,
            string chargeCommissionXml);
    }
}