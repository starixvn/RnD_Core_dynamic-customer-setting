namespace Fanex.CustomerSetting.SettingType2.Template1.DataAccesses
{
    using Fanex.CustomerSetting.Dtos.Params;
    using Fanex.CustomerSetting.Dtos.Results;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Models;

    public interface IBetLimitDataAccess
    {
        SettingType GetCustomerBetLimit(QuerySettingParam queryParam);

        IResult UpdateCustomerBetLimit(
            ISettingContext settingContext,
            int customerId,
            Level customerLevel,
            Level uplineLevel,
            byte productId,
            string betLimitXml);
    }
}