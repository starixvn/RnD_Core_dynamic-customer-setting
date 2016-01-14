namespace Fanex.CustomerSetting.Services
{
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Models.Enumerations;
    using Fanex.CustomerSetting.Models.Requests;
    using Fanex.CustomerSetting.Models.Results;

    public interface IPluggedTaskService
    {
        void Run(byte productId, CustomerSettingQuery query);

        void Run(byte productId, CustomerSettingQuery query, Setting setting);

        void Run(byte productId, UpdateCustomerSettingCommand command);

        void Run(byte productId, UpdateCustomerSettingCommand command, IResult result);

        void Run(byte productId, MultipleUpdateCustomerSettingCommand command);

        void Run(byte productId, MultipleUpdateCustomerSettingCommand command, IResult result);

        void Run(byte productId, short settingTypeId, CustomerSettingQuery query);

        void Run(byte productId, short settingTypeId, CustomerSettingQuery query, SettingType settingTypeData);

        void Run(
            byte productId,
            short settingTypeId,
            int customerId,
            Level customerLevel,
            Level uplineLevel,
            SettingType settingTypeData);

        void Run(
            byte productId,
            short settingTypeId,
            int customerId,
            Level customerLevel,
            Level uplineLevel,
            SettingType settingTypeData,
            SettingResult updateResult);

        void Run(
            byte productId,
            short settingTypeId,
            IEnumerable<int> customerIds,
            Level customerLevel,
            Level uplineLevel,
            SettingType settingTypeData);

        void Run(
            byte productId,
            short settingTypeId,
            IEnumerable<int> customerIds,
            Level customerLevel,
            Level uplineLevel,
            SettingType settingTypeData,
            IEnumerable<SettingResult> updateResults);
    }
}