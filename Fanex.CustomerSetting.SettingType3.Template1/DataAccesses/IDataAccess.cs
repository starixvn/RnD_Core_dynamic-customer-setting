namespace Fanex.CustomerSetting.SettingType3.Template1.DataAccesses
{
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Models.Enumerations;
    using Fanex.CustomerSetting.Models.Requests;
    using Fanex.CustomerSetting.Models.Results;
    using Fanex.CustomerSetting.SettingType3.Template1.Dtos;

    public interface IDataAccess
    {
        IEnumerable<CommissionItemDto> GetCustomerCommissionItemsRawData(CustomerSettingQuery query);

        IResult UpdateCustomerCommission(
            ISettingContext settingContext,
            int customerId,
            Level customerLevel,
            Level uplineLevel,
            string commissionXml);
    }
}