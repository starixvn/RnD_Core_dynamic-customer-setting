namespace Fanex.CustomerSetting.SettingType5.DataAccesses
{
    using System.Collections.Generic;
    using CustomerSetting.Models;
    using CustomerSetting.Models.Enumerations;
    using CustomerSetting.Models.Requests;
    using CustomerSetting.Models.Results;
    using Dtos;

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