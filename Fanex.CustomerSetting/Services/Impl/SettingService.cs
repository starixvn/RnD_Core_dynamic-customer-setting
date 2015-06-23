namespace Fanex.CustomerSetting.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Common.Helper;
    using Fanex.CustomerSetting.Criteria;
    using Fanex.CustomerSetting.Dtos.Params;
    using Fanex.CustomerSetting.Dtos.Results;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Handlers;
    using Fanex.CustomerSetting.Handlers.Impl;
    using Fanex.CustomerSetting.Models;
    using Fanex.NAP.Framework.Data;

    public class SettingService : ISettingService
    {
        private readonly ISettingContext _settingContext;
        private readonly IValidationService _validationService;
        private readonly IProductService _productService;
        private readonly ISettingResolver _settingResolver;
        private readonly IDynamicRepository _dynamicRepository;

        public SettingService()
            : this(null, null, null, null, null)
        {
        }

        public SettingService(
            ISettingContext settingContext,
            IValidationService validationService,
            IProductService productService,
            ISettingResolver settingResolver,
            IDynamicRepository dynamicRepository)
        {
            _settingContext = settingContext;
            _productService = productService ?? new ProductService();
            _validationService = validationService ?? new ValidationService();
            _settingResolver = settingResolver ?? new SettingResolver(_settingContext);
            _dynamicRepository = dynamicRepository ?? new DynamicRepository();
        }

        public ISetting GetCustomerSetting(SettingQuery settingQuery)
        {
            CheckSettingQueryArguments(settingQuery);
            ValidateSettingQuery(settingQuery);
            ISetting setting = new Setting();
            setting.Customer = GetCustomer(settingQuery.CustomerId, settingQuery.CustomerLevel);
            setting.Product = GetProduct(settingQuery.ProductId);
            setting.SettingTypes = GetCustomerSettingTypes(settingQuery, setting.Product);

            return setting;
        }

        private void CheckSettingQueryArguments(SettingQuery settingQuery)
        {
            Check.Argument.IsNotNull(settingQuery, "settingQuery");
            Check.Argument.IsNotNegativeOrZero(settingQuery.CustomerId, "settingQuery.CustomerId");
            Check.Argument.IsNotNegativeOrZero((byte)settingQuery.CustomerLevel, "settingQuery.CustomerLevel");
            Check.Argument.IsNotNegativeOrZero(settingQuery.ProductId, "settingQuery.ProductId");
        }

        private void ValidateSettingQuery(SettingQuery settingQuery)
        {
            var validationResult = _validationService.ValidateSettingQuery(settingQuery);
            if (!validationResult.IsSuccessful)
            {
                throw new InvalidOperationException(validationResult.Message);
            }
        }

        private Product GetProduct(byte productId)
        {
            IEnumerable<SettingType> productSettingTypes = _productService.GetProductSettingTypes(productId);

            return new Product
            {
                Id = productId,
                SettingTypes = productSettingTypes
            };
        }

        private Customer GetCustomer(int customerId, CustomerLevel customerLevel)
        {
            var customer = _dynamicRepository.Get<Customer>(
                new CustomerQueryCriteria
                {
                    CustId = customerId
                });

            if (customer != null)
            {
                customer.Level = customerLevel;
            }

            return customer;
        }

        private IList<SettingType> GetCustomerSettingTypes(SettingQuery settingQuery, Product product)
        {
            var customerSettingTypes = new List<SettingType>();
            foreach (var productSettingType in product.SettingTypes)
            {
                RunPreGetSettingTasks(settingQuery, productSettingType);
                var customerSettingType = GetCustomerSettingType(settingQuery, productSettingType);
                customerSettingType = RunPostGetSettingTasks(settingQuery, customerSettingType);
                customerSettingTypes.Add(customerSettingType);
            }

            return customerSettingTypes;
        }

        private void RunPreGetSettingTasks(SettingQuery settingQuery, SettingType productSettingType)
        {
            SettingPreTasks preTasks = _settingResolver.ResolveSettingPreTasks(productSettingType);
            if (preTasks != null
                && !preTasks.GetType().GetMethod("PreGetSetting").IsVirtual)
            {
                preTasks.PreGetSetting(settingQuery);
            }
        }

        private SettingType GetCustomerSettingType(SettingQuery settingQuery, SettingType productSettingType)
        {
            var settingDataAccessFactory = _settingResolver.ResolveSettingDataAccessFactory(productSettingType);
            var settingDataAccess = settingDataAccessFactory.Create(settingQuery.CustomerLevel);
            var customerSettingType = settingDataAccess.GetCustomerSettingType(_settingContext.Actor, settingQuery);
            customerSettingType.Id = productSettingType.Id;
            customerSettingType.SettingTemplate = productSettingType.SettingTemplate;
            return customerSettingType;
        }

        private SettingType RunPostGetSettingTasks(
            SettingQuery settingQuery,
            SettingType customerSettingType)
        {
            SettingPostTasks postTasks = _settingResolver.ResolveSettingPostTasks(customerSettingType);
            if (postTasks != null
                && !postTasks.GetType().GetMethod("PostGetSetting").IsVirtual)
            {
                customerSettingType = postTasks.PostGetSetting(settingQuery, customerSettingType);
            }

            return customerSettingType;
        }

        public IResult UpdateCustomerSetting(SettingCommand settingCommand)
        {
            CheckSettingCommandArguments(settingCommand);
            var validationResult = ValidateSettingCommand(settingCommand);
            if (!validationResult.IsSuccessful)
            {
                return validationResult;
            }

            return UpdateSingleCustomerSetting(settingCommand);
        }

        private void CheckSettingCommandArguments(SettingCommand settingCommand)
        {
            Check.Argument.IsNotNull(settingCommand, "settingCommand");
            Check.Argument.IsNotNull(settingCommand.Setting, "settingCommand.Setting");
            Check.Argument.IsNotNegativeOrZero(settingCommand.Setting.Customer.Id, "settingCommand.Setting.Customer.Id");
            Check.Argument.IsNotNegativeOrZero(settingCommand.Setting.Product.Id, "settingQuery.CustomerLevel");
            Check.Argument.IsNotNull(settingCommand.Setting.SettingTypes, "settingCommand.Setting");
            Check.Argument.IsNotNegativeOrZero(settingCommand.Setting.SettingTypes.Count(), "settingCommand.Setting.SettingTypes.Count");
        }

        private IResult ValidateSettingCommand(SettingCommand settingCommand)
        {
            return _validationService.ValidateSettingCommand(settingCommand);
        }

        private IResult UpdateSingleCustomerSetting(SettingCommand settingCommand)
        {
            var settingTypes = settingCommand.Setting.SettingTypes;
            var customer = settingCommand.Setting.Customer;
            var uplineLevel = settingCommand.UplineLevel;
            var results = new List<IResult>();

            foreach (var settingType in settingTypes)
            {
                var result = UpdateCustomerSettingType(customer, settingType, uplineLevel);
                results.Add(result);
            }

            return new Result(results);
        }

        private IResult UpdateCustomerSettingType(Customer customer, SettingType settingType, CustomerLevel? uplineLevel)
        {
            var preTaskResult = RunPreUpdateSettingTasks(customer, settingType, uplineLevel);
            if (!preTaskResult.IsSuccessful)
            {
                return preTaskResult;
            }

            var updateResult = UpdateSetting(customer, settingType, uplineLevel);
            if (updateResult.IsSuccessful)
            {
                updateResult = RunPostUpdateSettingTasks(customer, settingType, uplineLevel, updateResult);
            }

            return updateResult;
        }

        private IResult RunPreUpdateSettingTasks(Customer customer, SettingType settingType, CustomerLevel? uplineLevel)
        {
            SettingPreTasks preTasks = _settingResolver.ResolveSettingPreTasks(settingType);
            if (preTasks != null
                && !preTasks.GetType().GetMethod("PreUpdateSetting").IsVirtual)
            {
                var preTaskResult = preTasks.PreUpdateSetting(customer, settingType, uplineLevel);
                return preTaskResult;
            }

            return Result.Success;
        }

        private IResult UpdateSetting(Customer customer, SettingType settingType, CustomerLevel? uplineLevel)
        {
            var settingTypeDataAccessFactory = _settingResolver.ResolveSettingDataAccessFactory(settingType);
            var settingTypeDataAccess = settingTypeDataAccessFactory.Create(customer.Level);
            var result = settingTypeDataAccess.UpdateCustomerSetting(_settingContext, customer, settingType, uplineLevel);

            return result;
        }

        private IResult RunPostUpdateSettingTasks(
            Customer customer,
            SettingType settingType,
            CustomerLevel? uplineLevel,
            IResult updateSettingResult)
        {
            SettingPostTasks postTasks = _settingResolver.ResolveSettingPostTasks(settingType);
            if (postTasks != null
                && !postTasks.GetType().GetMethod("PostUpdateSetting").IsVirtual)
            {
                updateSettingResult = postTasks.PostUpdateSetting(customer, settingType, uplineLevel, updateSettingResult);
            }

            return updateSettingResult;
        }

        public IResult UpdateMultipleCustomerSetting(MultipleSettingCommand multipleSettingCommand)
        {
            CheckMultipleSettingCommandAgruments(multipleSettingCommand);
            var validationResult = ValidateMultipleSettingCommand(multipleSettingCommand);
            if (!validationResult.IsSuccessful)
            {
                return validationResult;
            }

            var customerLevel = multipleSettingCommand.Setting.Customer.Level;
            var uplineLevel = multipleSettingCommand.UplineLevel;
            var customers = multipleSettingCommand.Customers;
            var settingTypes = multipleSettingCommand.Setting.SettingTypes;
            var results = UpdateSettingTypesForMultipleCustomers(settingTypes, customers, customerLevel, uplineLevel);

            return new Result(results);
        }

        private void CheckMultipleSettingCommandAgruments(MultipleSettingCommand multipleSettingCommand)
        {
            Check.Argument.IsNotNull(multipleSettingCommand, "multipleSettingCommand");
            Check.Argument.IsNotNull(multipleSettingCommand.Setting, "multipleSettingCommand.Setting");
            Check.Argument.IsNotNegativeOrZero(multipleSettingCommand.Setting.Product.Id, "multipleSettingCommand.Setting.Product.Id");
            Check.Argument.IsNotNegativeOrZero(multipleSettingCommand.Setting.SettingTypes.Count(), "multipleSettingCommand.Setting.SettingTypes.Count");
            Check.Argument.IsNotNegativeOrZero(multipleSettingCommand.Customers.Count(), "multipleSettingCommand.Customers.Count");
        }

        private IResult ValidateMultipleSettingCommand(MultipleSettingCommand multipleSettingCommand)
        {
            return _validationService.ValidateMultipleSettingCommand(multipleSettingCommand);
        }

        private List<IResult> UpdateSettingTypesForMultipleCustomers(
            IEnumerable<SettingType> settingTypes,
            IEnumerable<Customer> customers,
            CustomerLevel customerLevel,
            CustomerLevel? uplineLevel)
        {
            var results = new List<IResult>();

            foreach (var settingType in settingTypes)
            {
                var result = UpdateSetingTypeForMultipleCustomers(customers, customerLevel, uplineLevel, settingType);
                results.AddRange(result.SubResults);
            }

            return results;
        }

        private IResult UpdateSetingTypeForMultipleCustomers(
            IEnumerable<Customer> customers,
            CustomerLevel customerLevel,
            CustomerLevel? uplineLevel,
            SettingType settingType)
        {
            var preTasksResult = RunPreUpdateMultipleSettingTasks(customers, customerLevel, settingType, uplineLevel);
            if (!preTasksResult.Result.IsSuccessful)
            {
                return preTasksResult.Result;
            }

            var customersNeedForUpdating = GetCustomersNeedForUpdating(preTasksResult.CustomersNeedForUpdating, customers);
            var results = UpdateSettingTypeOfEachCustomer(customersNeedForUpdating, customerLevel, settingType, uplineLevel);
            IResult updatedResult = new Result(results);

            if (updatedResult.IsSuccessful)
            {
                updatedResult = RunPostUpdateMultipleSettingTasks(customers, settingType, uplineLevel, updatedResult);
            }

            return updatedResult;
        }

        /// <summary>
        /// This is for the case we want to update multiple member has same position setting group
        /// We need to update grouped member on pre tasks and return un-grouped member to update one by one.
        /// </summary>
        /// <param name="customersReturnFromPreTask">Customer list return from pre task, may be different with original customer list.</param>
        /// <param name="originalCustomers">Original customer list.</param>
        /// <returns>List customer will be update one by one.</returns>
        private IEnumerable<Customer> GetCustomersNeedForUpdating(
            IEnumerable<Customer> customersReturnFromPreTask,
            IEnumerable<Customer> originalCustomers)
        {
            if (customersReturnFromPreTask != null && customersReturnFromPreTask.Any())
            {
                return customersReturnFromPreTask;
            }

            return originalCustomers;
        }

        private PreUpdateMultipleSettingResult RunPreUpdateMultipleSettingTasks(
            IEnumerable<Customer> customers,
            CustomerLevel customerLevel,
            SettingType settingType,
            CustomerLevel? uplineLevel)
        {
            SettingPreTasks preTasks = _settingResolver.ResolveSettingPreTasks(settingType);
            if (preTasks != null && !preTasks.GetType().GetMethod("PreUpdateMultipleSetting").IsVirtual)
            {
                var preTaskResult = preTasks.PreUpdateMultipleSetting(customers, customerLevel, settingType, uplineLevel);
                return preTaskResult;
            }

            return new PreUpdateMultipleSettingResult
            {
                Result = Result.Success
            };
        }

        private List<IResult> UpdateSettingTypeOfEachCustomer(
            IEnumerable<Customer> customers,
            CustomerLevel customerLevel,
            SettingType settingType,
            CustomerLevel? uplineLevel)
        {
            var settingTypeResults = new List<IResult>();
            foreach (var customer in customers)
            {
                var settingCommand = CreateSingleSettingCommand(customer.Id, customerLevel, settingType, uplineLevel);
                var updateResult = UpdateSingleCustomerSetting(settingCommand);
                settingTypeResults.Add(new Result(customer.Id, customer.UserName, updateResult));
            }

            return settingTypeResults;
        }

        private SettingCommand CreateSingleSettingCommand(
            int customerId,
            CustomerLevel customerLevel,
            SettingType settingType,
            CustomerLevel? uplineLevel)
        {
            var singleSettingCommand = new SettingCommand
            {
                Setting = new Setting
                {
                    Customer = new Customer { Id = customerId, Level = customerLevel },
                    SettingTypes = new List<SettingType> { settingType }
                },
                UplineLevel = uplineLevel
            };

            return singleSettingCommand;
        }

        private IResult RunPostUpdateMultipleSettingTasks(
            IEnumerable<Customer> customers,
            SettingType settingType,
            CustomerLevel? uplineLevel,
            IResult settingTypeMultipleResult)
        {
            SettingPostTasks postTasks = _settingResolver.ResolveSettingPostTasks(settingType);
            if (postTasks != null
                && !postTasks.GetType().GetMethod("PostUpdateMultipleSetting").IsVirtual)
            {
                settingTypeMultipleResult = postTasks.PostUpdateMultipleSetting(customers, settingType, uplineLevel, settingTypeMultipleResult);
            }

            return settingTypeMultipleResult;
        }
    }
}