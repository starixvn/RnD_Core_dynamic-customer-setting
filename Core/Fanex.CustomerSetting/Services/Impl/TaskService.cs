namespace Fanex.CustomerSetting.Services.Impl
{
    using System.Collections.Generic;
    using Configurations;
    using Models;
    using Models.Enumerations;
    using Models.Requests;
    using Models.Results;
    using Tasks;

    public class TaskService : ITaskService
    {
        private readonly ISettingContext _settingContext;

        public TaskService(ISettingContext settingContext)
        {
            _settingContext = settingContext;
        }

        public void Run(byte productId, CustomerSettingQuery query)
        {
            var tasks = TaskConfiguration.PickTasks<IPreQueryCustomerSettingTask>(_settingContext.System, productId);

            foreach (IPreQueryCustomerSettingTask task in tasks)
            {
                task.Invoke(_settingContext, query);
            }
        }

        public void Run(byte productId, CustomerSettingQuery query, ISetting setting)
        {
            var tasks = TaskConfiguration.PickTasks<IPostQueryCustomerSettingTask>(_settingContext.System, productId);

            foreach (IPostQueryCustomerSettingTask task in tasks)
            {
                task.Invoke(_settingContext, query, setting);
            }
        }

        public void Run(byte productId, UpdateCustomerSettingCommand command)
        {
            var tasks = TaskConfiguration.PickTasks<IPreUpdateCustomerSettingTask>(_settingContext.System, productId);

            foreach (IPreUpdateCustomerSettingTask task in tasks)
            {
                task.Invoke(_settingContext, command);
            }
        }

        public void Run(byte productId, UpdateCustomerSettingCommand command, IResult result)
        {
            var tasks = TaskConfiguration.PickTasks<IPostUpdateCustomerSettingTask>(_settingContext.System, productId);

            foreach (IPostUpdateCustomerSettingTask task in tasks)
            {
                task.Invoke(_settingContext, command, result);
            }
        }

        public void Run(byte productId, MultipleUpdateCustomerSettingCommand command)
        {
            var tasks = TaskConfiguration.PickTasks<IPreUpdateMultipleCustomersSettingTask>(_settingContext.System, productId);

            foreach (IPreUpdateMultipleCustomersSettingTask task in tasks)
            {
                task.Invoke(_settingContext, command);
            }
        }

        public void Run(byte productId, MultipleUpdateCustomerSettingCommand command, IResult result)
        {
            var tasks = TaskConfiguration.PickTasks<IPostUpdateMultipleCustomersSettingTask>(_settingContext.System, productId);

            foreach (IPostUpdateMultipleCustomersSettingTask task in tasks)
            {
                task.Invoke(_settingContext, command, result);
            }
        }

        public void Run(byte productId, short settingTypeId, CustomerSettingQuery query)
        {
            var tasks = TaskConfiguration.PickTasks<IPreQueryCustomerSettingTypeTask>(_settingContext.System, productId, settingTypeId);

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
            var tasks = TaskConfiguration.PickTasks<IPostQueryCustomerSettingTypeTask>(_settingContext.System, productId, settingTypeId);

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
            var tasks = TaskConfiguration.PickTasks<IPreUpdateCustomerSettingTypeTask>(_settingContext.System, productId, settingTypeId);

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
            var tasks = TaskConfiguration.PickTasks<IPostUpdateCustomerSettingTypeTask>(_settingContext.System, productId, settingTypeId);

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
            var tasks = TaskConfiguration.PickTasks<IPreUpdateMultipleCustomersSettingTypeTask>(_settingContext.System, productId, settingTypeId);

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
            var tasks = TaskConfiguration.PickTasks<IPostUpdateMultipleCustomersSettingTypeTask>(_settingContext.System, productId, settingTypeId);

            foreach (IPostUpdateMultipleCustomersSettingTypeTask task in tasks)
            {
                task.Invoke(_settingContext, customerIds, customerLevel, uplineLevel, productId, settingTypeData, updateResults);
            }
        }

        public void Run(byte productId, short settingTypeId, RedirectCommand command)
        {
            var tasks = TaskConfiguration.PickTasks<IPreRedirectTask>(_settingContext.System, productId, settingTypeId);

            foreach (IPreRedirectTask task in tasks)
            {
                task.Invoke(_settingContext, command);
            }
        }

        public void Run(byte productId, short settingTypeId, RedirectCommand command, object result)
        {
            var tasks = TaskConfiguration.PickTasks<IPostRedirectTask>(_settingContext.System, productId, settingTypeId);

            foreach (IPostRedirectTask task in tasks)
            {
                task.Invoke(_settingContext, command, result);
            }
        }
    }
}