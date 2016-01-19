namespace Fanex.CustomerSetting.Services.Impl
{
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Configurations;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Models.Enumerations;
    using Fanex.CustomerSetting.Models.PlugableTasks;
    using Fanex.CustomerSetting.Models.Requests;
    using Fanex.CustomerSetting.Models.Results;

    public class PluggedTaskService : IPluggedTaskService
    {
        private readonly ISettingContext _settingContext;

        public PluggedTaskService(ISettingContext settingContext)
        {
            _settingContext = settingContext;
        }

        public void Run(byte productId, CustomerSettingQuery query)
        {
            var tasks = PluggableTaskConfiguration.PickTasks<IPreQueryCustomerSettingTask>(_settingContext.System, productId);

            foreach (IPreQueryCustomerSettingTask task in tasks)
            {
                task.Invoke(_settingContext, query);
            }
        }

        public void Run(byte productId, CustomerSettingQuery query, ISetting setting)
        {
            var tasks = PluggableTaskConfiguration.PickTasks<IPostQueryCustomerSettingTask>(_settingContext.System, productId);

            foreach (IPostQueryCustomerSettingTask task in tasks)
            {
                task.Invoke(_settingContext, query, setting);
            }
        }

        public void Run(byte productId, UpdateCustomerSettingCommand command)
        {
            var tasks = PluggableTaskConfiguration.PickTasks<IPreUpdateCustomerSettingTask>(_settingContext.System, productId);

            foreach (IPreUpdateCustomerSettingTask task in tasks)
            {
                task.Invoke(_settingContext, command);
            }
        }

        public void Run(byte productId, UpdateCustomerSettingCommand command, IResult result)
        {
            var tasks = PluggableTaskConfiguration.PickTasks<IPostUpdateCustomerSettingTask>(_settingContext.System, productId);

            foreach (IPostUpdateCustomerSettingTask task in tasks)
            {
                task.Invoke(_settingContext, command, result);
            }
        }

        public void Run(byte productId, MultipleUpdateCustomerSettingCommand command)
        {
            var tasks = PluggableTaskConfiguration.PickTasks<IPreUpdateMultipleCustomersSettingTask>(_settingContext.System, productId);

            foreach (IPreUpdateMultipleCustomersSettingTask task in tasks)
            {
                task.Invoke(_settingContext, command);
            }
        }

        public void Run(byte productId, MultipleUpdateCustomerSettingCommand command, IResult result)
        {
            var tasks = PluggableTaskConfiguration.PickTasks<IPostUpdateMultipleCustomersSettingTask>(_settingContext.System, productId);

            foreach (IPostUpdateMultipleCustomersSettingTask task in tasks)
            {
                task.Invoke(_settingContext, command, result);
            }
        }

        public void Run(byte productId, short settingTypeId, CustomerSettingQuery query)
        {
            var tasks = PluggableTaskConfiguration.PickTasks<IPreQueryCustomerSettingTypeTask>(_settingContext.System, productId, settingTypeId);

            foreach (IPreQueryCustomerSettingTypeTask task in tasks)
            {
                task.Invoke(_settingContext, query);
            }
        }

        public void Run(
            byte productId,
            short settingTypeId,
            CustomerSettingQuery query,
            SettingType settingTypeData)
        {
            var tasks = PluggableTaskConfiguration.PickTasks<IPostQueryCustomerSettingTypeTask>(_settingContext.System, productId, settingTypeId);

            foreach (IPostQueryCustomerSettingTypeTask task in tasks)
            {
                task.Invoke(_settingContext, query, settingTypeData);
            }
        }

        public void Run(
            byte productId,
            short settingTypeId,
            int customerId,
            Level customerLevel,
            Level uplineLevel,
            SettingType settingTypeData)
        {
            var tasks = PluggableTaskConfiguration.PickTasks<IPreUpdateCustomerSettingTypeTask>(_settingContext.System, productId, settingTypeId);

            foreach (IPreUpdateCustomerSettingTypeTask task in tasks)
            {
                task.Invoke(_settingContext, customerId, customerLevel, uplineLevel, productId, settingTypeData);
            }
        }

        public void Run(
            byte productId,
            short settingTypeId,
            int customerId,
            Level customerLevel,
            Level uplineLevel,
            SettingType settingTypeData,
            IResult updateResult)
        {
            var tasks = PluggableTaskConfiguration.PickTasks<IPostUpdateCustomerSettingTypeTask>(_settingContext.System, productId, settingTypeId);

            foreach (IPostUpdateCustomerSettingTypeTask task in tasks)
            {
                task.Invoke(_settingContext, customerId, customerLevel, uplineLevel, productId, settingTypeData, updateResult);
            }
        }

        public void Run(
            byte productId,
            short settingTypeId,
            IEnumerable<int> customerIds,
            Level customerLevel,
            Level uplineLevel,
            SettingType settingTypeData)
        {
            var tasks = PluggableTaskConfiguration.PickTasks<IPreUpdateMultipleCustomersSettingTypeTask>(_settingContext.System, productId, settingTypeId);

            foreach (IPreUpdateMultipleCustomersSettingTypeTask task in tasks)
            {
                task.Invoke(_settingContext, customerIds, customerLevel, uplineLevel, productId, settingTypeData);
            }
        }

        public void Run(
            byte productId,
            short settingTypeId,
            IEnumerable<int> customerIds,
            Level customerLevel,
            Level uplineLevel,
            SettingType settingTypeData,
            IEnumerable<IResult> updateResults)
        {
            var tasks = PluggableTaskConfiguration.PickTasks<IPostUpdateMultipleCustomersSettingTypeTask>(_settingContext.System, productId, settingTypeId);

            foreach (IPostUpdateMultipleCustomersSettingTypeTask task in tasks)
            {
                task.Invoke(_settingContext, customerIds, customerLevel, uplineLevel, productId, settingTypeData, updateResults);
            }
        }
    }
}