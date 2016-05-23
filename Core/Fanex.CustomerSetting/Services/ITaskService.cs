namespace Fanex.CustomerSetting.Services
{
    using System.Collections.Generic;
    using Models;
    using Models.Enumerations;
    using Models.Requests;
    using Models.Results;

    public interface ITaskService
    {
        void Run(byte productId, CustomerSettingQuery query);

        void Run(byte productId, CustomerSettingQuery query, ISetting setting);

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
            IResult updateResult);

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
            IEnumerable<IResult> updateResults);
    }
}