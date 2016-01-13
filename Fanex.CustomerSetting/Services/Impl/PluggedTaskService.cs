namespace Fanex.CustomerSetting.Services.Impl
{
    using System.Collections.Generic;
    using Configurations;
    using Models;
    using Models.Enumerations;
    using Models.PlugableTasks;
    using Models.Requests;
    using Models.Results;

    public class PluggedTaskService : IPluggedTaskService
    {
        private readonly ISettingContext _settingContext;

        public PluggedTaskService(ISettingContext settingContext)
        {
            _settingContext = settingContext;
            PluggableTaskConfiguration.Initialize();
        }

        public void Run(byte productId, CustomerSettingQuery query)
        {
            var tasks = PluggableTaskConfiguration.PickTasks<IPreQuerySettingTask>(productId);

            foreach (IPreQuerySettingTask task in tasks)
            {
                task.Invoke(_settingContext, query);
            }
        }

        public void Run(byte productId, CustomerSettingQuery query, Setting setting)
        {
            var tasks = PluggableTaskConfiguration.PickTasks<IPostQuerySettingTask>(productId);

            foreach (IPostQuerySettingTask task in tasks)
            {
                task.Invoke(_settingContext, query, setting);
            }
        }

        public void Run(byte productId, UpdateCustomerSettingCommand command)
        {
            var tasks = PluggableTaskConfiguration.PickTasks<IPreUpdateSettingTask>(productId);

            foreach (IPreUpdateSettingTask task in tasks)
            {
                task.Invoke(_settingContext, command);
            }
        }

        public void Run(byte productId, UpdateCustomerSettingCommand command, IResult result)
        {
            var tasks = PluggableTaskConfiguration.PickTasks<IPostUpdateSettingTask>(productId);

            foreach (IPostUpdateSettingTask task in tasks)
            {
                task.Invoke(_settingContext, command, result);
            }
        }

        public void Run(byte productId, MultipleUpdateCustomerSettingCommand command)
        {
            var tasks = PluggableTaskConfiguration.PickTasks<IPreMultipleUpdateSettingTask>(productId);

            foreach (IPreMultipleUpdateSettingTask task in tasks)
            {
                task.Invoke(_settingContext, command);
            }
        }

        public void Run(byte productId, MultipleUpdateCustomerSettingCommand command, IResult result)
        {
            var tasks = PluggableTaskConfiguration.PickTasks<IPostMultipleUpdateSettingTask>(productId);

            foreach (IPostMultipleUpdateSettingTask task in tasks)
            {
                task.Invoke(_settingContext, command, result);
            }
        }

        public void Run(byte productId, short settingTypeId, CustomerSettingQuery query)
        {
            var tasks = PluggableTaskConfiguration.PickTasks<IPreQuerySettingTypeTask>(productId, settingTypeId);

            foreach (IPreQuerySettingTypeTask task in tasks)
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
            var tasks = PluggableTaskConfiguration.PickTasks<IPostQuerySettingTypeTask>(productId, settingTypeId);

            foreach (IPostQuerySettingTypeTask task in tasks)
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
            var tasks = PluggableTaskConfiguration.PickTasks<IPreUpdateSettingTypeTask>(productId, settingTypeId);

            foreach (IPreUpdateSettingTypeTask task in tasks)
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
            SettingResult updateResult)
        {
            var tasks = PluggableTaskConfiguration.PickTasks<IPostUpdateSettingTypeTask>(productId, settingTypeId);

            foreach (IPostUpdateSettingTypeTask task in tasks)
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
            var tasks = PluggableTaskConfiguration.PickTasks<IPreMultipleUpdateSettingTypeTask>(productId, settingTypeId);

            foreach (IPreMultipleUpdateSettingTypeTask task in tasks)
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
            IEnumerable<SettingResult> updateResults)
        {
            var tasks = PluggableTaskConfiguration.PickTasks<IPostMultipleUpdateSettingTypeTask>(productId, settingTypeId);

            foreach (IPostMultipleUpdateSettingTypeTask task in tasks)
            {
                task.Invoke(_settingContext, customerIds, customerLevel, uplineLevel, productId, settingTypeData, updateResults);
            }
        }
    }
}