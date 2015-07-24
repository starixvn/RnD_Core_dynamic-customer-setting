namespace Fanex.CustomerSetting.SettingType3.Template1.DataAccesses
{
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Params;
    using Fanex.CustomerSetting.Results;
    using Fanex.CustomerSetting.SettingType3.Template1.Dtos;

    public interface IDataAccess
    {
        IEnumerable<CommissionItemDto> GetCustomerCommissionItemsRawData(QueryCustomerSettingParam queryParam);

        IResult UpdateCustomerCommission(
            ISettingContext settingContext,
            int customerId,
            Level customerLevel,
            Level uplineLevel,
            byte productId,
            string commissionXml);
    }
}