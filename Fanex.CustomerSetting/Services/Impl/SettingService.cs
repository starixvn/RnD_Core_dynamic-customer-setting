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
    using Fanex.Logging;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Service class for get single customer setting/ update single/multiple setting
    /// Setting includes list of setting types (pt, bet limit, commission ....)
    /// Customer can be super, master, agent, member
    /// So we call {Customer}{SettingType} => means SuperPositionTaking, MasterCommission ...
    /// </summary>
    public class SettingService : ISettingService
    {
        private readonly ISettingContext _settingContext;
        private readonly IValidationService _validationService;
        private readonly IProductService _productService;
        private readonly ISettingResolver _settingResolver;
        private readonly IDynamicRepository _dynamicRepository;
        private readonly ILogger _logger;

        public SettingService(
            ISettingContext settingContext,
            IValidationService validationService = null,
            IProductService productService = null,
            ISettingResolver settingResolver = null,
            IDynamicRepository dynamicRepository = null,
            ILogger logger = null)
        {
            _settingContext = settingContext;
            _productService = productService ?? new ProductService();
            _validationService = validationService ?? new ValidationService(_settingContext);
            _settingResolver = settingResolver ?? new SettingResolver();
            _dynamicRepository = dynamicRepository ?? new DynamicRepository();
            _logger = logger ?? Logger.Log;
        }

        public ISetting GetCustomerSetting(QuerySettingParams queryParams)
        {
            CheckQuerySettingArguments(queryParams);
            ValidateQuerySetting(queryParams);
            ISetting setting = new Setting();
            setting.Customer = GetCustomer(queryParams.CustomerId, queryParams.CustomerLevel);
            setting.Product = GetProduct(queryParams.ProductId);
            setting.SettingTypes = GetCustomerSettingTypes(queryParams, setting.Product.SettingTypes);

            return setting;
        }

        private void CheckQuerySettingArguments(QuerySettingParams queryParams)
        {
            Check.Argument.IsNotNull(queryParams, "settingQuery");
            Check.Argument.IsNotNegativeOrZero(queryParams.CustomerId, "queryParams.CustomerId");
            Check.Argument.IsNotNegativeOrZero((byte)queryParams.CustomerLevel, "queryParams.CustomerLevel");
            Check.Argument.IsNotNegativeOrZero(queryParams.ProductId, "queryParams.ProductId");
        }

        private void ValidateQuerySetting(QuerySettingParams queryParams)
        {
            var validationResult = _validationService.ValidateQuerySetting(queryParams);
            if (!validationResult.IsSuccessful)
            {
                throw new InvalidOperationException(validationResult.Message);
            }
        }

        /// <summary>
        /// Gets product information, includes list of product setting types.
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Get {Customer}{SettingType} => means Get SuperPositionTaking, MasterCommission ....
        /// </summary>
        /// <param name="queryParams">The query setting parameters.</param>
        /// <param name="productSettingTypes">The product setting types.</param>
        /// <returns>Customer setting on each setting type.</returns>
        private IList<SettingType> GetCustomerSettingTypes(QuerySettingParams queryParams, IEnumerable<SettingType> productSettingTypes)
        {
            var customerSettingTypes = new List<SettingType>();
            foreach (var productSettingType in productSettingTypes)
            {
                try
                {
                    RunPreGetCustomerSettingTypeTasks(queryParams, productSettingType);
                    var customerSettingType = GetCustomerSettingType(queryParams, productSettingType);
                    customerSettingType = RunPostGetCustomerSettingTypeTasks(queryParams, customerSettingType);
                    customerSettingTypes.Add(customerSettingType);
                }
                catch (Exception ex)
                {
                    LogError(ex);
                }
            }

            return customerSettingTypes;
        }

        private void RunPreGetCustomerSettingTypeTasks(QuerySettingParams queryParams, SettingType productSettingType)
        {
            SettingPreTasks preTasks = _settingResolver.ResolveSettingPreTasks(productSettingType);
            if (preTasks != null
                && !preTasks.GetType().GetMethod("PreGetCustomerSettingType").IsVirtual)
            {
                preTasks.PreGetCustomerSettingType(_settingContext, queryParams);
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

        private SettingType RunPostGetCustomerSettingTypeTasks(
            QuerySettingParams queryParams,
            SettingType customerSettingType)
        {
            var postTasks = _settingResolver.ResolveSettingPostTasks(customerSettingType);
            if (postTasks != null
                && !postTasks.GetType().GetMethod("PostGetCustomerSettingType").IsVirtual)
            {
                customerSettingType = postTasks.PostGetCustomerSettingType(_settingContext, queryParams, customerSettingType);
            }

            return customerSettingType;
        }

        /// <summary>
        /// Updates the customer setting, includes list of setting types.
        /// </summary>
        /// <param name="updatingParams">The updating parameters.</param>
        /// <returns>Result after updating customer setting.</returns>
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
                try
                {
                    results.Add(UpdateCustomerSettingType(customer, settingType, uplineLevel));
                }
                catch (Exception ex)
                {
                    results.Add(new Result(settingType.Id, settingType.Name, Result.SystemError));
                    LogError(ex);
                }
            }

            return new Result(results);
        }

        private IResult UpdateCustomerSettingType(Customer customer, SettingType settingType, CustomerLevel? uplineLevel)
        {
            var preTaskResult = RunPreUpdateCustomerSettingTypeTasks(customer, settingType, uplineLevel);
            if (!preTaskResult.IsSuccessful)
            {
                return preTaskResult;
            }

            var updatingResult = RunUpdateCustomerSettingType(customer, settingType, uplineLevel);
            updatingResult = RunPostUpdateCustomerSettingTypeTasks(customer, settingType, uplineLevel, updatingResult);

            return updatingResult;
        }

        private IResult RunPreUpdateCustomerSettingTypeTasks(Customer customer, SettingType settingType, CustomerLevel? uplineLevel)
        {
            var preTasks = _settingResolver.ResolveSettingPreTasks(settingType);
            if (preTasks != null
                && !preTasks.GetType().GetMethod("PreUpdateCustomerSettingType").IsVirtual)
            {
                return preTasks.PreUpdateCustomerSettingType(_settingContext, customer, settingType, uplineLevel);
            }

            return Result.Success;
        }

        private IResult RunUpdateCustomerSettingType(Customer customer, SettingType settingType, CustomerLevel? uplineLevel)
        {
            var settingTypeDataAccess = _settingResolver.ResolveSettingDataAccess(settingType);
            var result = settingTypeDataAccess.UpdateCustomerSettingType(_settingContext, customer, settingType, uplineLevel);

            return result;
        }

        private IResult RunPostUpdateCustomerSettingTypeTasks(
            Customer customer,
            SettingType settingType,
            CustomerLevel? uplineLevel,
            IResult updatingResult)
        {
            var postTasks = _settingResolver.ResolveSettingPostTasks(settingType);
            if (postTasks != null
                && !postTasks.GetType().GetMethod("PostUpdateCustomerSettingType").IsVirtual)
            {
                updatingResult = postTasks.PostUpdateCustomerSettingType(_settingContext, customer, settingType, uplineLevel, updatingResult);
            }

            return updatingResult;
        }

        public IResult UpdateMultipleCustomerSetting(UpdatingMultipleSettingParams updatingMultipleParams)
        {
            CheckUpdatingMultipleCustomerSettingAgruments(updatingMultipleParams);
            var validationResult = ValidateUpdatingMultipleCustomerSetting(updatingMultipleParams);
            if (!validationResult.IsSuccessful)
            {
                return validationResult;
            }

            var results = UpdateSettingTypesForMultipleCustomers(updatingMultipleParams);

            return new Result(results);
        }

        private void CheckUpdatingMultipleCustomerSettingAgruments(UpdatingMultipleSettingParams updatingMultipleParams)
        {
            Check.Argument.IsNotNull(updatingMultipleParams, "updatingMultipleParams");
            Check.Argument.IsNotNull(updatingMultipleParams.Setting, "updatingMultipleParams.Setting");
            Check.Argument.IsNotNegativeOrZero(updatingMultipleParams.Setting.Product.Id, "updatingMultipleParams.Setting.Product.Id");
            Check.Argument.IsNotNegativeOrZero(updatingMultipleParams.Setting.SettingTypes.Count(), "updatingMultipleParams.Setting.SettingTypes.Count");
            Check.Argument.IsNotNegativeOrZero(updatingMultipleParams.Customers.Count(), "updatingMultipleParams.Customers.Count");
        }

        private IResult ValidateUpdatingMultipleCustomerSetting(UpdatingMultipleSettingParams updatingMultipleParams)
        {
            return _validationService.ValidateUpdatingMultipleSetting(updatingMultipleParams);
        }

        private List<IResult> UpdateSettingTypesForMultipleCustomers(UpdatingMultipleSettingParams updatingMultipleParams)
        {
            var customerLevel = updatingMultipleParams.Setting.Customer.Level;
            var uplineLevel = updatingMultipleParams.UplineLevel;
            var customers = updatingMultipleParams.Customers;
            var settingTypes = updatingMultipleParams.Setting.SettingTypes;
            var results = new List<IResult>();

            foreach (var settingType in settingTypes)
            {
                try
                {
                    var result = UpdateSetingTypeForMultipleCustomers(customers, customerLevel, uplineLevel, settingType);
                    results.AddRange(result.SubResults);
                }
                catch (Exception ex)
                {
                    results.Add(new Result(settingType.Id, settingType.Name, Result.SystemError));
                    LogError(ex);
                }
            }

            return results;
        }

        private IResult UpdateSetingTypeForMultipleCustomers(
            IEnumerable<Customer> customers,
            CustomerLevel customerLevel,
            CustomerLevel? uplineLevel,
            SettingType settingType)
        {
            var preTasksResult = RunPreUpdateMultipleCustomersSettingTypeTasks(customers, customerLevel, settingType, uplineLevel);
            if (!preTasksResult.Result.IsSuccessful && preTasksResult.IsStopUpdating)
            {
                return preTasksResult.Result;
            }

            //// We need to GetCustomersNeedForUpdating here, because in RunPreUpdateMultipleSettingTasks above
            //// because we already updated setting for them, so we need to remove them out of customer list.
            var customersNeedForUpdating = GetCustomersNeedForSingleUpdating(preTasksResult.CustomersNeedForSingleUpdating, customers);
            var results = UpdateSettingTypeOfEachCustomer(customersNeedForUpdating, customerLevel, settingType, uplineLevel);
            IResult updatedResult = new Result(results);
            updatedResult = RunPostUpdateMultipleCustomersSettingTypeTasks(customers, settingType, uplineLevel, updatedResult);

            return updatedResult;
        }

        /// <summary>
        /// This is for the case we want to update multiple member has same position setting group
        /// We need to update grouped member on pre tasks and return un-grouped member to update one by one.
        /// </summary>
        /// <param name="customersReturnFromPreTask">Customer list return from pre task, may be different with original customer list.</param>
        /// <param name="originalCustomers">Original customer list.</param>
        /// <returns>List customer will be update one by one.</returns>
        private IEnumerable<Customer> GetCustomersNeedForSingleUpdating(
            IEnumerable<Customer> customersReturnFromPreTask,
            IEnumerable<Customer> originalCustomers)
        {
            if (customersReturnFromPreTask != null
                && customersReturnFromPreTask.Any())
            {
                return customersReturnFromPreTask;
            }

            return originalCustomers;
        }

        private PreUpdateMultipleCustomersSettingTypeResult RunPreUpdateMultipleCustomersSettingTypeTasks(
            IEnumerable<Customer> customers,
            CustomerLevel customerLevel,
            SettingType settingType,
            CustomerLevel? uplineLevel)
        {
            var preTasks = _settingResolver.ResolveSettingPreTasks(settingType);
            if (preTasks != null && !preTasks.GetType().GetMethod("PreUpdateMultipleCustomersSettingType").IsVirtual)
            {
                return preTasks.PreUpdateMultipleCustomersSettingType(_settingContext, customers, customerLevel, settingType, uplineLevel);
            }

            return new PreUpdateMultipleCustomersSettingTypeResult
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
                var updatingParams = CreateUpdatingSettingParamsForSingleCustomer(customer.Id, customerLevel, settingType, uplineLevel);
                var updateResult = UpdateSingleCustomerSetting(updatingParams);
                settingTypeResults.Add(new Result(customer.Id, customer.UserName, updateResult));
            }

            return settingTypeResults;
        }

        private UpdatingSettingParams CreateUpdatingSettingParamsForSingleCustomer(
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

        private IResult RunPostUpdateMultipleCustomersSettingTypeTasks(
            IEnumerable<Customer> customers,
            SettingType settingType,
            CustomerLevel? uplineLevel,
            IResult settingTypeMultipleResult)
        {
            var postTasks = _settingResolver.ResolveSettingPostTasks(settingType);
            if (postTasks != null
                && !postTasks.GetType().GetMethod("PostUpdateMultipleCustomersSettingType").IsVirtual)
            {
                return postTasks.PostUpdateMultipleCustomersSettingType(_settingContext, customers, settingType, uplineLevel, settingTypeMultipleResult);
            }

            return settingTypeMultipleResult;
        }

        private void LogError(Exception ex)
        {
            _logger.Error(ex.Message, ex);
        }
    }
}