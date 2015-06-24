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

        public ISetting GetCustomerSetting(QuerySettingParams querySettingParams)
        {
            CheckSettingQueryArguments(querySettingParams);
            ValidateSettingQuery(querySettingParams);
            ISetting setting = new Setting();
            setting.Customer = GetCustomer(querySettingParams.CustomerId, querySettingParams.CustomerLevel);
            setting.Product = GetProduct(querySettingParams.ProductId);
            setting.SettingTypes = GetCustomerSettingTypes(querySettingParams, setting.Product);

            return setting;
        }

        private void CheckSettingQueryArguments(QuerySettingParams queryParams)
        {
            Check.Argument.IsNotNull(queryParams, "settingQuery");
            Check.Argument.IsNotNegativeOrZero(queryParams.CustomerId, "queryParams.CustomerId");
            Check.Argument.IsNotNegativeOrZero((byte)queryParams.CustomerLevel, "queryParams.CustomerLevel");
            Check.Argument.IsNotNegativeOrZero(queryParams.ProductId, "queryParams.ProductId");
        }

        private void ValidateSettingQuery(QuerySettingParams queryParams)
        {
            var validationResult = _validationService.ValidateQuerySetting(queryParams);
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

        private IList<SettingType> GetCustomerSettingTypes(QuerySettingParams queryParams, Product product)
        {
            var customerSettingTypes = new List<SettingType>();
            foreach (var productSettingType in product.SettingTypes)
            {
                RunPreGetSettingTasks(queryParams, productSettingType);
                var customerSettingType = GetCustomerSettingType(queryParams, productSettingType);
                customerSettingType = RunPostGetSettingTasks(queryParams, customerSettingType);
                customerSettingTypes.Add(customerSettingType);
            }

            return customerSettingTypes;
        }

        private void RunPreGetSettingTasks(QuerySettingParams queryParams, SettingType productSettingType)
        {
            SettingPreTasks preTasks = _settingResolver.ResolveSettingPreTasks(productSettingType);
            if (preTasks != null
                && !preTasks.GetType().GetMethod("PreGetSetting").IsVirtual)
            {
                preTasks.PreGetSetting(queryParams);
            }
        }

        private SettingType GetCustomerSettingType(QuerySettingParams queryParams, SettingType productSettingType)
        {
            var settingDataAccess = _settingResolver.ResolveSettingDataAccess(productSettingType);
            var customerSettingType = settingDataAccess.GetCustomerSettingType(_settingContext.Actor, queryParams);
            customerSettingType.Id = productSettingType.Id;
            customerSettingType.SettingTemplate = productSettingType.SettingTemplate;
            return customerSettingType;
        }

        private SettingType RunPostGetSettingTasks(
            QuerySettingParams queryParams,
            SettingType customerSettingType)
        {
            var postTasks = _settingResolver.ResolveSettingPostTasks(customerSettingType);
            if (postTasks != null
                && !postTasks.GetType().GetMethod("PostGetSetting").IsVirtual)
            {
                customerSettingType = postTasks.PostGetSetting(queryParams, customerSettingType);
            }

            return customerSettingType;
        }

        public IResult UpdateCustomerSetting(UpdatingSettingParams updatingParams)
        {
            CheckUpdatingSettingArguments(updatingParams);
            var validationResult = ValidateUpdatingSetting(updatingParams);
            if (!validationResult.IsSuccessful)
            {
                return validationResult;
            }

            return UpdateSingleCustomerSetting(updatingParams);
        }

        private void CheckUpdatingSettingArguments(UpdatingSettingParams updatingParams)
        {
            Check.Argument.IsNotNull(updatingParams, "updatingParams");
            Check.Argument.IsNotNull(updatingParams.Setting, "updatingParams.Setting");
            Check.Argument.IsNotNegativeOrZero(updatingParams.Setting.Customer.Id, "updatingParams.Setting.Customer.Id");
            Check.Argument.IsNotNegativeOrZero(updatingParams.Setting.Product.Id, "updatingParams.CustomerLevel");
            Check.Argument.IsNotNull(updatingParams.Setting.SettingTypes, "updatingParams.Setting");
            Check.Argument.IsNotNegativeOrZero(updatingParams.Setting.SettingTypes.Count(), "updatingParams.Setting.SettingTypes.Count");
        }

        private IResult ValidateUpdatingSetting(UpdatingSettingParams updatingParams)
        {
            return _validationService.ValidateUpdatingSetting(updatingParams);
        }

        private IResult UpdateSingleCustomerSetting(UpdatingSettingParams updatingParams)
        {
            var settingTypes = updatingParams.Setting.SettingTypes;
            var customer = updatingParams.Setting.Customer;
            var uplineLevel = updatingParams.UplineLevel;
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

            var updatingResult = UpdateSetting(customer, settingType, uplineLevel);
            if (updatingResult.IsSuccessful)
            {
                updatingResult = RunPostUpdateSettingTasks(customer, settingType, uplineLevel, updatingResult);
            }

            return updatingResult;
        }

        private IResult RunPreUpdateSettingTasks(Customer customer, SettingType settingType, CustomerLevel? uplineLevel)
        {
            var preTasks = _settingResolver.ResolveSettingPreTasks(settingType);
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
            var settingTypeDataAccess = _settingResolver.ResolveSettingDataAccess(settingType);
            var result = settingTypeDataAccess.UpdateCustomerSetting(_settingContext, customer, settingType, uplineLevel);

            return result;
        }

        private IResult RunPostUpdateSettingTasks(
            Customer customer,
            SettingType settingType,
            CustomerLevel? uplineLevel,
            IResult updatingResult)
        {
            var postTasks = _settingResolver.ResolveSettingPostTasks(settingType);
            if (postTasks != null
                && !postTasks.GetType().GetMethod("PostUpdateSetting").IsVirtual)
            {
                updatingResult = postTasks.PostUpdateSetting(customer, settingType, uplineLevel, updatingResult);
            }

            return updatingResult;
        }

        public IResult UpdateMultipleCustomerSetting(UpdateMultipleSettingParams updatingMultipleParams)
        {
            CheckUpdatingMultipleAgruments(updatingMultipleParams);
            var validationResult = ValidateUpdatingMultipleSetting(updatingMultipleParams);
            if (!validationResult.IsSuccessful)
            {
                return validationResult;
            }

            var customerLevel = updatingMultipleParams.Setting.Customer.Level;
            var uplineLevel = updatingMultipleParams.UplineLevel;
            var customers = updatingMultipleParams.Customers;
            var settingTypes = updatingMultipleParams.Setting.SettingTypes;
            var results = UpdateSettingTypesForMultipleCustomers(settingTypes, customers, customerLevel, uplineLevel);

            return new Result(results);
        }

        private void CheckUpdatingMultipleAgruments(UpdateMultipleSettingParams updatingMultipleParams)
        {
            Check.Argument.IsNotNull(updatingMultipleParams, "updatingMultipleParams");
            Check.Argument.IsNotNull(updatingMultipleParams.Setting, "updatingMultipleParams.Setting");
            Check.Argument.IsNotNegativeOrZero(updatingMultipleParams.Setting.Product.Id, "updatingMultipleParams.Setting.Product.Id");
            Check.Argument.IsNotNegativeOrZero(updatingMultipleParams.Setting.SettingTypes.Count(), "updatingMultipleParams.Setting.SettingTypes.Count");
            Check.Argument.IsNotNegativeOrZero(updatingMultipleParams.Customers.Count(), "updatingMultipleParams.Customers.Count");
        }

        private IResult ValidateUpdatingMultipleSetting(UpdateMultipleSettingParams updatingMultipleParams)
        {
            return _validationService.ValidateUpdatingMultipleSetting(updatingMultipleParams);
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
                if (preTasksResult.IsStopUpdating)
                {
                    return preTasksResult.Result;
                }
            }

            //// We need to GetCustomersNeedForUpdating here, because in RunPreUpdateMultipleSettingTasks above
            //// because we already updated setting for them, so we need to remove them out of customer list.
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
            var preTasks = _settingResolver.ResolveSettingPreTasks(settingType);
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
                var updatingParams = CreateSingleUpdatingSettingParams(customer.Id, customerLevel, settingType, uplineLevel);
                var updateResult = UpdateSingleCustomerSetting(updatingParams);
                settingTypeResults.Add(new Result(customer.Id, customer.UserName, updateResult));
            }

            return settingTypeResults;
        }

        private UpdatingSettingParams CreateSingleUpdatingSettingParams(
            int customerId,
            CustomerLevel customerLevel,
            SettingType settingType,
            CustomerLevel? uplineLevel)
        {
            var updateParams = new UpdatingSettingParams
            {
                Setting = new Setting
                {
                    Customer = new Customer { Id = customerId, Level = customerLevel },
                    SettingTypes = new List<SettingType> { settingType }
                },
                UplineLevel = uplineLevel
            };

            return updateParams;
        }

        private IResult RunPostUpdateMultipleSettingTasks(
            IEnumerable<Customer> customers,
            SettingType settingType,
            CustomerLevel? uplineLevel,
            IResult settingTypeMultipleResult)
        {
            var postTasks = _settingResolver.ResolveSettingPostTasks(settingType);
            if (postTasks != null
                && !postTasks.GetType().GetMethod("PostUpdateMultipleSetting").IsVirtual)
            {
                settingTypeMultipleResult = postTasks.PostUpdateMultipleSetting(customers, settingType, uplineLevel, settingTypeMultipleResult);
            }

            return settingTypeMultipleResult;
        }
    }
}