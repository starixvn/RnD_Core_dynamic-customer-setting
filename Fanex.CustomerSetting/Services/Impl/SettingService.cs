namespace Fanex.CustomerSetting.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
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
        private readonly IResourceService _resourceService;
        private readonly ILogger _logger;

        public SettingService(
            ISettingContext settingContext,
            IValidationService validationService = null,
            IProductService productService = null,
            ISettingResolver settingResolver = null,
            IDynamicRepository dynamicRepository = null,
            IResourceService resourceService = null,
            ILogger logger = null)
        {
            Check.Argument.IsNotNull(settingContext, "settingContext");
            Check.Argument.IsNotNull(settingContext.Actor, "settingContext.Actor");
            Check.Argument.IsNotNegativeOrZero(settingContext.Actor.Id, "settingContext.Actor.Id");
            Check.Argument.IsNotNegativeOrZero((byte)settingContext.Actor.Level, "settingContext.Actor.Level");

            _settingContext = settingContext;
            _productService = productService ?? new ProductService();
            _validationService = validationService ?? new ValidationService(_settingContext);
            _settingResolver = settingResolver ?? new SettingResolver();
            _dynamicRepository = dynamicRepository ?? new DynamicRepository();
            _resourceService = resourceService ?? new ResourceService();
            _logger = logger ?? Logger.Log;
        }

        public ISetting GetCustomerSetting(QuerySettingParam queryParam)
        {
            CheckQuerySettingArguments(queryParam);

            ValidateQuerySetting(queryParam);

            var product = GetProduct(queryParam.ProductId);

            ISetting setting = new Setting();
            setting.ProductId = product.Id;
            setting.ProductName = product.Name;
            setting.Customer = GetCustomer(queryParam.CustomerId, queryParam.CustomerLevel);
            setting.SettingTypes = GetCustomerSettingTypes(queryParam, product.SettingTypes);

            return setting;
        }

        private void CheckQuerySettingArguments(QuerySettingParam queryParam)
        {
            Check.Argument.IsNotNull(queryParam, "queryParam");
            Check.Argument.IsNotNegativeOrZero(queryParam.CustomerId, "queryParam.CustomerId");
            Check.Argument.IsNotNegativeOrZero((byte)queryParam.CustomerLevel, "queryParam.CustomerLevel");
            Check.Argument.IsNotNegativeOrZero(queryParam.ProductId, "queryParam.ProductId");
        }

        private void ValidateQuerySetting(QuerySettingParam queryParam)
        {
            var validationResult = _validationService.ValidateQuerySetting(queryParam);
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
            string productName = _resourceService.GetProductName(productId, _settingContext.Language);

            return new Product
            {
                Id = productId,
                Name = productName,
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
                customer.Id = customerId;
                customer.Level = customerLevel;
            }

            return customer;
        }

        /// <summary>
        /// Get {Customer}{SettingType} => means Get SuperPositionTaking, MasterCommission ....
        /// </summary>
        /// <param name="queryParam">The query setting parameters.</param>
        /// <param name="productSettingTypes">The product setting types.</param>
        /// <returns>Customer setting on each setting type.</returns>
        private IList<SettingType> GetCustomerSettingTypes(QuerySettingParam queryParam, IEnumerable<SettingType> productSettingTypes)
        {
            var customerSettingTypes = new List<SettingType>();
            foreach (var productSettingType in productSettingTypes)
            {
                try
                {
                    RunPreGetCustomerSettingTypeTasks(queryParam, productSettingType);
                    var customerSettingType = GetCustomerSettingType(queryParam, productSettingType);
                    customerSettingType = RunPostGetCustomerSettingTypeTasks(queryParam, customerSettingType);
                    if (customerSettingType.SettingItems.Any())
                    {
                        customerSettingTypes.Add(customerSettingType);
                    }
                }
                catch (Exception ex)
                {
                    LogError(ex);
                }
            }

            return customerSettingTypes;
        }

        private void RunPreGetCustomerSettingTypeTasks(QuerySettingParam queryParam, SettingType productSettingType)
        {
            SettingPreTasks preTasks = _settingResolver.ResolveSettingPreTasks(productSettingType);
            if (preTasks != null
                && IsOverride(preTasks.GetType().GetMethod("PreGetCustomerSettingType")))
            {
                preTasks.PreGetCustomerSettingType(_settingContext, queryParam);
            }
        }

        private SettingType GetCustomerSettingType(QuerySettingParam queryParam, SettingType productSettingType)
        {
            var settingDataAccess = _settingResolver.ResolveSettingDataAccess(productSettingType);
            var customerSettingType = settingDataAccess.GetCustomerSettingType(_settingContext.Actor, queryParam);
            customerSettingType.Id = productSettingType.Id;
            customerSettingType.SettingTemplate = productSettingType.SettingTemplate;
            customerSettingType.SettingLevel = productSettingType.SettingLevel;
            return customerSettingType;
        }

        private SettingType RunPostGetCustomerSettingTypeTasks(
            QuerySettingParam queryParam,
            SettingType customerSettingType)
        {
            var postTasks = _settingResolver.ResolveSettingPostTasks(customerSettingType);
            if (postTasks != null
                && IsOverride(postTasks.GetType().GetMethod("PostGetCustomerSettingType")))
            {
                customerSettingType = postTasks.PostGetCustomerSettingType(_settingContext, queryParam, customerSettingType);
            }

            return customerSettingType;
        }

        /// <summary>
        /// Updates the customer setting, includes list of setting types.
        /// </summary>
        /// <param name="updatingParam">The updating parameters.</param>
        /// <returns>Result after updating customer setting.</returns>
        public IResult UpdateCustomerSetting(UpdatingSettingParam updatingParam)
        {
            CheckUpdatingSettingArguments(updatingParam);
            var validationResult = ValidateUpdatingSetting(updatingParam);
            if (!validationResult.IsSuccessful)
            {
                return validationResult;
            }

            return UpdateSingleCustomerSetting(updatingParam);
        }

        private void CheckUpdatingSettingArguments(UpdatingSettingParam updatingParam)
        {
            Check.Argument.IsNotNull(updatingParam, "updatingParam");
            Check.Argument.IsNotNull(updatingParam.Setting, "updatingParam.Setting");
            Check.Argument.IsNotNegativeOrZero(updatingParam.Setting.Customer.Id, "updatingParam.Setting.Customer.Id");
            Check.Argument.IsNotNegativeOrZero(updatingParam.Setting.ProductId, "updatingParam.Setting.ProductId");
            Check.Argument.IsNotNull(updatingParam.Setting.SettingTypes, "updatingParam.Setting.SettingTypes");
            Check.Argument.IsNotNegativeOrZero(updatingParam.Setting.SettingTypes.Count(), "updatingParam.Setting.SettingTypes.Count");
        }

        private IResult ValidateUpdatingSetting(UpdatingSettingParam updatingParam)
        {
            return _validationService.ValidateUpdatingSetting(updatingParam);
        }

        private UpdatingSettingResult UpdateSingleCustomerSetting(UpdatingSettingParam updatingParam)
        {
            var settingTypes = updatingParam.Setting.SettingTypes;
            var customer = updatingParam.Setting.Customer;
            var uplineLevel = updatingParam.UplineLevel;
            var productId = updatingParam.Setting.ProductId;
            var settingTypeResults = new List<SettingResult>();
            SettingResult updatingSettingTypeResult = null;

            foreach (var settingType in settingTypes)
            {
                try
                {
                    updatingSettingTypeResult = UpdateCustomerSettingType(customer, settingType, productId, uplineLevel);
                }
                catch (Exception ex)
                {
                    updatingSettingTypeResult = new SettingResult(Result.SystemError.Code, Result.SystemError.Message, 0, customer.Id, customer.UserName, settingType.Id, settingType.Name);
                    LogError(ex);
                }

                settingTypeResults.Add(updatingSettingTypeResult);
            }

            return new UpdatingSettingResult(settingTypeResults);
        }

        private SettingResult UpdateCustomerSettingType(Customer customer, SettingType settingType, byte productId, CustomerLevel? uplineLevel)
        {
            var preTaskResult = RunPreUpdateCustomerSettingTypeTasks(customer, settingType, uplineLevel);
            if (!preTaskResult.IsSuccessful)
            {
                return preTaskResult;
            }

            var result = RunUpdateCustomerSettingType(customer, settingType, productId, uplineLevel);
            var settingResult = new SettingResult(result.Code, result.Message, result.MsgType, customer.Id, customer.FirstName, settingType.Id, settingType.Name);

            return RunPostUpdateCustomerSettingTypeTasks(customer, settingType, uplineLevel, settingResult);
        }

        private SettingResult RunPreUpdateCustomerSettingTypeTasks(Customer customer, SettingType settingType, CustomerLevel? uplineLevel)
        {
            var preTasks = _settingResolver.ResolveSettingPreTasks(settingType);
            if (preTasks != null
                && IsOverride(preTasks.GetType().GetMethod("PreUpdateCustomerSettingType")))
            {
                return preTasks.PreUpdateCustomerSettingType(_settingContext, customer, settingType, uplineLevel);
            }

            return SettingResult.SuccessSettingResult;
        }

        private IResult RunUpdateCustomerSettingType(Customer customer, SettingType settingType, byte productId, CustomerLevel? uplineLevel)
        {
            var settingTypeDataAccess = _settingResolver.ResolveSettingDataAccess(settingType);
            return settingTypeDataAccess.UpdateCustomerSettingType(_settingContext, customer, settingType, productId, uplineLevel);
        }

        private SettingResult RunPostUpdateCustomerSettingTypeTasks(
            Customer customer,
            SettingType settingType,
            CustomerLevel? uplineLevel,
            SettingResult result)
        {
            var postTasks = _settingResolver.ResolveSettingPostTasks(settingType);
            if (postTasks != null
                && IsOverride(postTasks.GetType().GetMethod("PostUpdateCustomerSettingType")))
            {
                return postTasks.PostUpdateCustomerSettingType(_settingContext, customer, settingType, uplineLevel, result);
            }

            return result;
        }

        public IResult UpdateMultipleCustomerSetting(UpdatingMultipleSettingParam updatingMultipleParam)
        {
            CheckUpdatingMultipleCustomerSettingAgruments(updatingMultipleParam);
            var validationResult = ValidateUpdatingMultipleCustomerSetting(updatingMultipleParam);
            if (!validationResult.IsSuccessful)
            {
                return validationResult;
            }

            var results = UpdateSettingTypesForMultipleCustomers(updatingMultipleParam);

            return new UpdatingSettingResult(results);
        }

        private void CheckUpdatingMultipleCustomerSettingAgruments(UpdatingMultipleSettingParam updatingMultipleParam)
        {
            Check.Argument.IsNotNull(updatingMultipleParam, "updatingMultipleParam");
            Check.Argument.IsNotNull(updatingMultipleParam.Setting, "updatingMultipleParam.Setting");
            Check.Argument.IsNotNegativeOrZero(updatingMultipleParam.Setting.ProductId, "updatingMultipleParam.Setting.Product.Id");
            Check.Argument.IsNotNegativeOrZero(updatingMultipleParam.Setting.SettingTypes.Count(), "updatingMultipleParam.Setting.SettingTypes.Count");
            Check.Argument.IsNotNegativeOrZero(updatingMultipleParam.Customers.Count(), "updatingMultipleParam.Customers.Count");
        }

        private IResult ValidateUpdatingMultipleCustomerSetting(UpdatingMultipleSettingParam updatingMultipleParam)
        {
            return _validationService.ValidateUpdatingMultipleSetting(updatingMultipleParam);
        }

        private List<SettingResult> UpdateSettingTypesForMultipleCustomers(UpdatingMultipleSettingParam updatingMultipleParam)
        {
            var customerLevel = updatingMultipleParam.Setting.Customer.Level;
            var uplineLevel = updatingMultipleParam.UplineLevel;
            var customers = updatingMultipleParam.Customers;
            var settingTypes = updatingMultipleParam.Setting.SettingTypes;
            var productId = updatingMultipleParam.Setting.ProductId;
            var results = new List<SettingResult>();

            foreach (var settingType in settingTypes)
            {
                try
                {
                    var result = UpdateSetingTypeForMultipleCustomers(customers, customerLevel, uplineLevel, settingType, productId);
                    results.AddRange(result.SubResults);
                }
                catch (Exception ex)
                {
                    var errorResult = new SettingResult(Result.SystemError.Code, Result.SystemError.Message, 0, 0, string.Empty, settingType.Id, settingType.Name);
                    results.Add(errorResult);
                    LogError(ex);
                }
            }

            return results;
        }

        private UpdatingSettingResult UpdateSetingTypeForMultipleCustomers(
            IEnumerable<Customer> customers,
            CustomerLevel customerLevel,
            CustomerLevel? uplineLevel,
            SettingType settingType,
            byte productId)
        {
            var preTasksResult = RunPreUpdateMultipleCustomersSettingTypeTasks(customers, customerLevel, settingType, uplineLevel);
            if (!preTasksResult.Result.IsSuccessful && preTasksResult.IsStopUpdating)
            {
                return preTasksResult.Result;
            }

            //// We need to GetCustomersNeedForUpdating here, because in RunPreUpdateMultipleSettingTasks above
            //// because we already updated setting for them, so we need to remove them out of customer list.
            var customersNeedForUpdating = GetCustomersNeedForSingleUpdating(preTasksResult.CustomersNeedForSingleUpdating, customers);
            var results = UpdateSettingTypeOfEachCustomer(customersNeedForUpdating, customerLevel, settingType, productId, uplineLevel);
            if (preTasksResult.IsReturnResultToUser
                && preTasksResult.Result.SubResults != null
                && preTasksResult.Result.SubResults.Any())
            {
                results.AddRange(preTasksResult.Result.SubResults);
            }

            return RunPostUpdateMultipleCustomersSettingTypeTasks(customers, settingType, uplineLevel, new UpdatingSettingResult(results));
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
            if (preTasks != null && IsOverride(preTasks.GetType().GetMethod("PreUpdateMultipleCustomersSettingType")))
            {
                return preTasks.PreUpdateMultipleCustomersSettingType(_settingContext, customers, customerLevel, settingType, uplineLevel);
            }

            return new PreUpdateMultipleCustomersSettingTypeResult
            {
                Result = UpdatingSettingResult.SuccessUpdatingSettingResult
            };
        }

        private List<SettingResult> UpdateSettingTypeOfEachCustomer(
            IEnumerable<Customer> customers,
            CustomerLevel customerLevel,
            SettingType settingType,
            byte productId,
            CustomerLevel? uplineLevel)
        {
            var settingTypeResults = new List<SettingResult>();
            foreach (var customer in customers)
            {
                var updatingParam = CreateUpdatingSettingParamForSingleCustomer(customer.Id, customerLevel, settingType, productId, uplineLevel);
                var updateResult = UpdateSingleCustomerSetting(updatingParam);
                settingTypeResults.AddRange(updateResult.SubResults);
            }

            return settingTypeResults;
        }

        private UpdatingSettingParam CreateUpdatingSettingParamForSingleCustomer(
            int customerId,
            CustomerLevel customerLevel,
            SettingType settingType,
            byte productId,
            CustomerLevel? uplineLevel)
        {
            var updateParam = new UpdatingSettingParam
            {
                Setting = new Setting
                {
                    Customer = new Customer { Id = customerId, Level = customerLevel },
                    ProductId = productId,
                    SettingTypes = new List<SettingType> { settingType }
                },
                UplineLevel = uplineLevel
            };

            return updateParam;
        }

        private UpdatingSettingResult RunPostUpdateMultipleCustomersSettingTypeTasks(
            IEnumerable<Customer> customers,
            SettingType settingType,
            CustomerLevel? uplineLevel,
            UpdatingSettingResult settingTypeMultipleResult)
        {
            var postTasks = _settingResolver.ResolveSettingPostTasks(settingType);
            if (postTasks != null
                && IsOverride(postTasks.GetType().GetMethod("PostUpdateMultipleCustomersSettingType")))
            {
                return postTasks.PostUpdateMultipleCustomersSettingType(_settingContext, customers, settingType, uplineLevel, settingTypeMultipleResult);
            }

            return settingTypeMultipleResult;
        }

        private void LogError(Exception ex)
        {
            _logger.Error(ex.Message, ex);
        }

        public bool IsOverride(MethodInfo methodInfo)
        {
            if (methodInfo == null)
            {
                throw new ArgumentNullException("methodInfo", "methodInfo cannot be null");
            }

            return methodInfo.GetBaseDefinition().DeclaringType != methodInfo.DeclaringType;
        }
    }
}