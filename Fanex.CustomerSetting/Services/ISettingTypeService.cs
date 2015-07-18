namespace Fanex.CustomerSetting.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.CustomerSetting.Dtos.Params;
    using Fanex.CustomerSetting.Dtos.Results;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Models;

    public interface ISettingTypeService
    {
        SettingType GetCustomerSettingType(ISettingContext settingContext, QuerySettingParam queryParam, SettingType productSettingType);

        SettingResult UpdateCustomerSettingType(ISettingContext settingContext, int customerId, Level customerLevel, Level uplineLevel, byte productId, SettingType settingType);

        IEnumerable<SettingResult> UpdateMultipleCustomersSettingType(ISettingContext settingContext, IEnumerable<int> customerIds, Level customerLevel, Level uplineLevel, byte productId, SettingType settingType);
    }
}
