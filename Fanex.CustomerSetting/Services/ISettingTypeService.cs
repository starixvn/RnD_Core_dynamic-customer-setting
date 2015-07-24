namespace Fanex.CustomerSetting.Services
{
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Models.Enumerations;
    using Fanex.CustomerSetting.Models.Requests;
    using Fanex.CustomerSetting.Models.Results;

    public interface ISettingTypeService
    {
        SettingType GetCustomerSettingType(
            ISettingContext settingContext, 
            CustomerSettingQuery query, 
            SettingType productSettingType);

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
