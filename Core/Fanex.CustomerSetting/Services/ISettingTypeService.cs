namespace Fanex.CustomerSetting.Services
{
    using System.Collections.Generic;
    using Models;
    using Models.Enumerations;
    using Models.Requests;
    using Models.Results;

    public interface ISettingTypeService
    {
        SettingType GetCustomerSettingType(
            ISettingContext settingContext,
            CustomerSettingQuery query);

        SettingResult UpdateCustomerSettingType(
            ISettingContext settingContext,
            int customerId,
            Level customerLevel,
            Level uplineLevel,
            byte productId,
            SettingType settingType);

        IEnumerable<SettingResult> UpdateMultipleCustomerSettingType(
            ISettingContext settingContext,
            IEnumerable<int> customerIds,
            Level customerLevel,
            Level uplineLevel,
            byte productId,
            SettingType settingType);
    }
}