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
            Check.Argument.IsNotNull(queryParam, "queryParam");

            ValidateQuerySetting(queryParam);

            var product = GetProduct(queryParam.ProductId);
            var customerInfo = GetCustomerInfo(queryParam.CustomerId);
            var customerSettingTypes = GetCustomerSettingTypes(queryParam, product.SettingTypes);

            ISetting setting = new Setting(
                queryParam.CustomerId,
                queryParam.CustomerLevel,
                customerInfo,
                product.Id,
                product.Name,
                customerSettingTypes);

            return setting;
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
            var productSettingTypes = _productService.GetProductSettingTypes(productId);
            var productName = _resourceService.GetProductName(productId, _settingContext.Language);

            return new Product
            {
                Id = productId,
                Name = productName,
                SettingTypes = productSettingTypes
            };
        }

        private CustomerInfo GetCustomerInfo(int customerId)
        {
            var customer = _dynamicRepository.Get<CustomerInfo>(
                new CustomerQueryCriteria
                {
                    CustId = customerId
                });

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
            Check.Argument.IsNotNull(updatingParam, "updatingParam");
            Check.Argument.IsNotNegativeOrZero(updatingParam.Setting.SettingTypes.Count(), "updatingParam.Setting.SettingTypes.Count");

            var validationResult = ValidateUpdatingSetting(updatingParam);
            if (!validationResult.IsSuccessful)
            {
                return validationResult;
            }

            return UpdateSingleCustomerSetting(updatingParam);
        }

        private IResult ValidateUpdatingSetting(UpdatingSettingParam updatingParam)
        {
            return _validationService.ValidateUpdatingSetting(updatingParam);
        }

        private UpdatingSettingResult UpdateSingleCustomerSetting(UpdatingSettingParam updatingParam)
        {
            var settingTypes = updatingParam.Setting.SettingTypes;
            var customerId = updatingParam.Setting.CustomerId;
            var customerLevel = updatingParam.Setting.CustomerLevel;
            var uplineLevel = updatingParam.UplineLevel;
            var productId = updatingParam.Setting.ProductId;
            var settingTypeResults = new List<SettingResult>();
            SettingResult updatingSettingTypeResult = null;

            foreach (var settingType in settingTypes)
            {
                try
                {
                    updatingSettingTypeResult = UpdateCustomerSettingType(
                        customerId, 
                        customerLevel, 
                        settingType, 
                        productId, 
                        uplineLevel);
                }
                catch (Exception ex)
                {
                    updatingSettingTypeResult = new SettingResult(
                        Result.SystemError.Code, 
                        Result.SystemError.Message, 
                        0, 
                        customerId, 
                        settingType.Id, 
                        settingType.Name);

                    LogError(ex);
                }

                settingTypeResults.Add(updatingSettingTypeResult);
            }

            return new UpdatingSettingResult(settingTypeResults);
        }

        private SettingResult UpdateCustomerSettingType(int customerId, CustomerLevel customerLevel, SettingType settingType, byte productId, CustomerLevel? uplineLevel)
        {
            var preTasksResult = RunPreUpdateCustomerSettingTypeTasks(customerId, settingType, uplineLevel);
            if (!preTasksResult.IsSuccessful)
            {
                return preTasksResult;
            }

            var result = RunUpdateCustomerSettingType(customerId, customerLevel, settingType, productId, uplineLevel);
            var settingResult = new SettingResult(result.Code, result.Message, result.MsgType, customerId, settingType.Id, settingType.Name);

            return RunPostUpdateCustomerSettingTypeTasks(customerId, settingType, uplineLevel, settingResult);
        }

        private SettingResult RunPreUpdateCustomerSettingTypeTasks(int customerId, SettingType settingType, CustomerLevel? uplineLevel)
        {
            var preTasks = _settingResolver.ResolveSettingPreTasks(settingType);
            if (preTasks != null
                && IsOverride(preTasks.GetType().GetMethod("PreUpdateCustomerSettingType")))
            {
                return preTasks.PreUpdateCustomerSettingType(_settingContext, customerId, settingType, uplineLevel);
            }

            return new SettingResult(Result.Success);
        }

        private IResult RunUpdateCustomerSettingType(int customerId, CustomerLevel customerLevel, SettingType settingType, byte productId, CustomerLevel? uplineLevel)
        {
            var settingTypeDataAccess = _settingResolver.ResolveSettingDataAccess(settingType);
            return settingTypeDataAccess.UpdateCustomerSettingType(_settingContext, customerId, customerLevel, settingType, productId, uplineLevel);
        }

        private SettingResult RunPostUpdateCustomerSettingTypeTasks(
            int customerId,
            SettingType settingType,
            CustomerLevel? uplineLevel,
            SettingResult result)
        {
            var postTasks = _settingResolver.ResolveSettingPostTasks(settingType);

            if (postTasks != null
                && IsOverride(postTasks.GetType().GetMethod("PostUpdateCustomerSettingType")))
            {
                return postTasks.PostUpdateCustomerSettingType(_settingContext, customerId, settingType, uplineLevel, result);
            }

            return result;
        }

        public IResult UpdateMultipleCustomerSetting(UpdatingMultipleSettingParam updatingMultipleParam)
        {
            Check.Argument.IsNotNull(updatingMultipleParam, "updatingMultipleParam");
            Check.Argument.IsNotNegativeOrZero(updatingMultipleParam.Setting.SettingTypes.Count(), "updatingMultipleParam.Setting.SettingTypes.Count");

            var validationResult = ValidateUpdatingMultipleCustomerSetting(updatingMultipleParam);
            if (!validationResult.IsSuccessful)
            {
                return validationResult;
            }

            var results = UpdateSettingTypesForMultipleCustomers(updatingMultipleParam);

            return MapUpdatingSettingResultWithCustomerName(results, updatingMultipleParam.CustomerIds, updatingMultipleParam.CustomerNames);
        }

        private IResult ValidateUpdatingMultipleCustomerSetting(UpdatingMultipleSettingParam updatingMultipleParam)
        {
            return _validationService.ValidateUpdatingMultipleSetting(updatingMultipleParam);
        }

        private List<SettingResult> UpdateSettingTypesForMultipleCustomers(UpdatingMultipleSettingParam updatingMultipleParam)
        {
            var customerLevel = updatingMultipleParam.Setting.CustomerLevel;
            var uplineLevel = updatingMultipleParam.UplineLevel;
            var customers = updatingMultipleParam.CustomerIds;
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
                    var errorResult = new SettingResult(Result.SystemError.Code, Result.SystemError.Message, 0, 0, settingType.Id, settingType.Name);
                    results.Add(errorResult);
                    
                    LogError(ex);
                }
            }

            return results;
        }

        private UpdatingSettingResult UpdateSetingTypeForMultipleCustomers(
            IEnumerable<int> customerIds,
            CustomerLevel customerLevel,
            CustomerLevel? uplineLevel,
            SettingType settingType,
            byte productId)
        {
            var preTasksResult = RunPreUpdateMultipleCustomersSettingTypeTasks(customerIds, customerLevel, settingType, uplineLevel);
            if (!preTasksResult.Result.IsSuccessful && preTasksResult.IsStopUpdating)
            {
                return preTasksResult.Result;
            }

            //// We need to GetCustomersNeedForUpdating here, because in RunPreUpdateMultipleSettingTasks above
            //// because we already updated setting for them, so we need to remove them out of customer list.
            var customerIdsForSingleUpdating = GetCustomerIdsNeedForSingleUpdating(preTasksResult.CustomerIdsForSingleUpdating, customerIds);
            var updatingResults = UpdateSettingTypeOfEachCustomer(customerIdsForSingleUpdating, customerLevel, settingType, productId, uplineLevel);

            if (preTasksResult.IsReturnResultToUser
                && preTasksResult.Result.SubResults != null
                && preTasksResult.Result.SubResults.Any())
            {
                updatingResults.AddRange(preTasksResult.Result.SubResults);
            }

            return RunPostUpdateMultipleCustomersSettingTypeTasks(customerIds, settingType, uplineLevel, new UpdatingSettingResult(updatingResults));
        }

        /// <summary>
        /// This is for the case we want to update multiple member has same position setting group
        /// We need to update grouped member on pre tasks and return un-grouped member to update one by one.
        /// </summary>
        /// <param name="customerIdsReturnFromPreTask">Customer list return from pre task, may be different with original customer list.</param>
        /// <param name="originalCustomerIds">Original customer list.</param>
        /// <returns>List customer will be update one by one.</returns>
        private IEnumerable<int> GetCustomerIdsNeedForSingleUpdating(
            IEnumerable<int> customerIdsReturnFromPreTask,
            IEnumerable<int> originalCustomerIds)
        {
            if (customerIdsReturnFromPreTask != null && customerIdsReturnFromPreTask.Any())
            {
                return customerIdsReturnFromPreTask;
            }

            return originalCustomerIds;
        }

        private PreUpdateMultipleCustomersSettingTypeResult RunPreUpdateMultipleCustomersSettingTypeTasks(
            IEnumerable<int> customers,
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
                Result = new UpdatingSettingResult(Result.Success)
            };
        }

        private List<SettingResult> UpdateSettingTypeOfEachCustomer(
            IEnumerable<int> customerIds,
            CustomerLevel customerLevel,
            SettingType settingType,
            byte productId,
            CustomerLevel? uplineLevel)
        {
            var settingTypeResults = new List<SettingResult>();
            foreach (var customerId in customerIds)
            {
                var singleCustomerSetting = new Setting(customerId, customerLevel, productId, new List<SettingType> { settingType });
                var updatingParam = new UpdatingSettingParam(singleCustomerSetting, uplineLevel);
                var updateResult = UpdateSingleCustomerSetting(updatingParam);
                settingTypeResults.AddRange(updateResult.SubResults);
            }

            return settingTypeResults;
        }

        private UpdatingSettingResult RunPostUpdateMultipleCustomersSettingTypeTasks(
            IEnumerable<int> customerIds,
            SettingType settingType,
            CustomerLevel? uplineLevel,
            UpdatingSettingResult settingTypeMultipleResult)
        {
            var postTasks = _settingResolver.ResolveSettingPostTasks(settingType);
            if (postTasks != null
                && IsOverride(postTasks.GetType().GetMethod("PostUpdateMultipleCustomersSettingType")))
            {
                return postTasks.PostUpdateMultipleCustomersSettingType(_settingContext, customerIds, settingType, uplineLevel, settingTypeMultipleResult);
            }

            return settingTypeMultipleResult;
        }

        private void LogError(Exception ex)
        {
            _logger.Error(ex.Message, ex);
        }

        private bool IsOverride(MethodInfo methodInfo)
        {
            return methodInfo.GetBaseDefinition().DeclaringType != methodInfo.DeclaringType;
        }

        private UpdatingSettingResult MapUpdatingSettingResultWithCustomerName(
            IEnumerable<SettingResult> settingResults,
            IEnumerable<int> customerIds,
            IEnumerable<string> customerNames)
        {
            var customerIdNameContainer = GetCustomerIdNameContainer(customerIds, customerNames);

            return MapSettingResultWithCustomerName(customerIdNameContainer, settingResults);
        }

        private UpdatingSettingResult MapSettingResultWithCustomerName(
            Dictionary<int, string> customerIdNameContainer,
            IEnumerable<SettingResult> results)
        {
            foreach (SettingResult result in results)
            {
                if (customerIdNameContainer.ContainsKey(result.CustomerId))
                {
                    result.CustomerName = customerIdNameContainer[result.CustomerId];
                }
            }

            return new UpdatingSettingResult(results);
        }

        private Dictionary<int, string> GetCustomerIdNameContainer(
            IEnumerable<int> customerIds,
            IEnumerable<string> customerNames)
        {
            Dictionary<int, string> customerIdNameContainer = new Dictionary<int, string>();
            var customerLength = customerIds.Count();
            for (int i = 0; i < customerLength; i++)
            {
                customerIdNameContainer.Add(customerIds.ElementAt(i), customerNames.ElementAt(i));
            }

            return customerIdNameContainer;
        }
    }
}