namespace Fanex.CustomerSetting.SettingType4.Template1.DataAccesses
{
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Dtos.Params;
    using Fanex.CustomerSetting.Dtos.Results;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.SettingType4.Template1.Dtos;

    public interface IDataAccess
    {
        IEnumerable<ProductStatusItemDto> GetCustomerProductStatusItemsRawData(QuerySettingParam queryParam);

        IResult UpdateCustomerProductStatus(
            ISettingContext settingContext,
            int customerId,
            Level customerLevel,
            Level uplineLevel,
            byte productId,
            string productStatusXml);
    }
}