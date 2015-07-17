namespace Fanex.CustomerSetting.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Fanex.Common.Helper;
    using Fanex.CustomerSetting.Dtos.Params;
    using Fanex.CustomerSetting.Dtos.Results;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Exceptions;
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
    public class UpdateSettingService : IUpdateSettingService
    {
        private readonly ISettingContext _settingContext;
        private readonly IValidationService _validationService;
        private readonly IProductService _productService;
        private readonly ILogger _logger;
        private readonly ISettingResolver _settingResolver;
        private readonly IDynamicRepository _dynamicRepository;

        public UpdateSettingService(
            ISettingContext settingContext,
            IValidationService validationService = null,
            IProductService productService = null,
            ILogger logger = null,
            ISettingResolver settingResolver = null,
            IDynamicRepository dynamicRepository = null)
        {
            Check.Argument.IsNotNull(settingContext, "settingContext");
            Check.Argument.IsNotNull(settingContext.Actor, "settingContext.Actor");

            _settingContext = settingContext;
            _productService = productService ?? new ProductService();
            _validationService = validationService ?? new ValidationService(_settingContext);
            _logger = logger ?? Logger.Log;
            _settingResolver = settingResolver ?? new SettingResolver();
            _dynamicRepository = dynamicRepository ?? new DynamicRepository();
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
                    LogUpdateCustomerSettingTypeError(customerId, customerLevel, uplineLevel, productId, settingType, ex);
                }

                settingTypeResults.Add(updatingSettingTypeResult);
            }

            return new UpdatingSettingResult(settingTypeResults);
        }

        private SettingResult UpdateCustomerSettingType(int customerId, Level customerLevel, SettingType settingType, byte productId, Level uplineLevel)
        {
            var preTasksResult = RunPreUpdateCustomerSettingTypeTasks(customerId, customerLevel, settingType, uplineLevel);

            if (!preTasksResult.IsSuccessful)
            {
                return preTasksResult;
            }

            var result = RunUpdateCustomerSettingType(customerId, customerLevel, settingType, productId, uplineLevel);
            var settingResult = new SettingResult(result.Code, result.Message, result.MsgType, customerId, settingType.Id, settingType.Name);

            return RunPostUpdateCustomerSettingTypeTasks(customerId, customerLevel, settingType, uplineLevel, settingResult);
        }

        private SettingResult RunPreUpdateCustomerSettingTypeTasks(
            int customerId,
            Level customerLevel,
            SettingType settingType,
            Level uplineLevel)
        {
            var preTasks = _settingResolver.ResolveSettingPreTasks(settingType);

            if (preTasks != null
                && IsOverride(preTasks.GetType().GetMethod("PreUpdateCustomerSettingType")))
            {
                return preTasks.PreUpdateCustomerSettingType(_settingContext, customerId, customerLevel, settingType, uplineLevel);
            }

            return new SettingResult(Result.Success);
        }

        private IResult RunUpdateCustomerSettingType(int customerId, Level customerLevel, SettingType settingType, byte productId, Level uplineLevel)
        {
            var settingTypeDataAccess = _settingResolver.ResolveSettingDataAccess(settingType);
            return settingTypeDataAccess.UpdateCustomerSettingType(_settingContext, customerId, customerLevel, settingType, productId, uplineLevel);
        }

        private SettingResult RunPostUpdateCustomerSettingTypeTasks(
            int customerId,
            Level customerLevel,
            SettingType settingType,
            Level uplineLevel,
            SettingResult result)
        {
            var postTasks = _settingResolver.ResolveSettingPostTasks(settingType);

            if (postTasks != null
                && IsOverride(postTasks.GetType().GetMethod("PostUpdateCustomerSettingType")))
            {
                return postTasks.PostUpdateCustomerSettingType(_settingContext, customerId, customerLevel, settingType, uplineLevel, result);
            }

            return result;
        }

        private void LogUpdateCustomerSettingTypeError(
            int customerId, 
            Level customerLevel, 
            Level uplineLevel, 
            byte productId,
            SettingType customerSettingType, 
            Exception ex)
        {
            var customerSettingException = new CustomerSettingException(ex.Message, ex);
            customerSettingException.SetSettingContextInfo("Update Setting", _settingContext);
            customerSettingException.SetCustomerInfo(customerId, customerLevel, uplineLevel);
            customerSettingException.SetProductInfo(productId, customerSettingType);

            _logger.Error(ex.Message, ex);
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
            var customerIds = updatingMultipleParam.CustomerIds;
            var settingTypes = updatingMultipleParam.Setting.SettingTypes;
            var productId = updatingMultipleParam.Setting.ProductId;
            var results = new List<SettingResult>();

            foreach (var settingType in settingTypes)
            {
                try
                {
                    var result = UpdateSetingTypeForMultipleCustomers(customerIds, customerLevel, uplineLevel, settingType, productId);
                    results.AddRange(result.SubResults);
                }
                catch (Exception ex)
                {
                    var errorResult = new SettingResult(
                        Result.SystemError.Code,
                        Result.SystemError.Message,
                        msgType: 0,
                        customerId: 0,
                        settingTypeId: settingType.Id,
                        settingTypeName: settingType.Name);

                    LogUpdateMultipleCustomersSettingTypeError(customerIds, customerLevel, uplineLevel, productId, settingType, ex);

                    results.Add(errorResult);
                }
            }

            return results;
        }

        private UpdatingSettingResult UpdateSetingTypeForMultipleCustomers(
            IEnumerable<int> customerIds,
            Level customerLevel,
            Level uplineLevel,
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
            var customerIdsForSingleUpdating = GetCustomerIdsForSingleUpdating(preTasksResult.CustomerIdsForSingleUpdating, customerIds);
            var updatingResults = UpdateSettingTypeOfEachCustomer(customerIdsForSingleUpdating, customerLevel, settingType, productId, uplineLevel);

            if (preTasksResult.IsReturnResultToUser
                && preTasksResult.Result.SubResults != null
                && preTasksResult.Result.SubResults.Any())
            {
                updatingResults.AddRange(preTasksResult.Result.SubResults);
            }

            return RunPostUpdateMultipleCustomersSettingTypeTasks(customerIds, customerLevel, settingType, uplineLevel, new UpdatingSettingResult(updatingResults));
        }

        /// <summary>
        /// This is for the case we want to update multiple member has same position setting group
        /// We need to update grouped member on pre tasks and return un-grouped member to update one by one.
        /// </summary>
        /// <param name="customerIdsReturnFromPreTask">Customer list return from pre task, may be different with original customer list.</param>
        /// <param name="originalCustomerIds">Original customer list.</param>
        /// <returns>List customer will be update one by one.</returns>
        private IEnumerable<int> GetCustomerIdsForSingleUpdating(
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
            Level customerLevel,
            SettingType settingType,
            Level uplineLevel)
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
            Level customerLevel,
            SettingType settingType,
            byte productId,
            Level uplineLevel)
        {
            var settingTypeResults = new List<SettingResult>();
            foreach (var customerId in customerIds)
            {
                var singleCustomerSetting = new Setting(customerId, customerLevel, productId, new List<SettingType> { settingType });
                var updatingParam = new UpdatingSettingParam(singleCustomerSetting, uplineLevel);
                var updatingResult = UpdateSingleCustomerSetting(updatingParam);

                settingTypeResults.AddRange(updatingResult.SubResults);
            }

            return settingTypeResults;
        }

        private UpdatingSettingResult RunPostUpdateMultipleCustomersSettingTypeTasks(
            IEnumerable<int> customerIds,
            Level customerLevel,
            SettingType settingType,
            Level uplineLevel,
            UpdatingSettingResult settingTypeMultipleResult)
        {
            var postTasks = _settingResolver.ResolveSettingPostTasks(settingType);
            if (postTasks != null
                && IsOverride(postTasks.GetType().GetMethod("PostUpdateMultipleCustomersSettingType")))
            {
                return postTasks.PostUpdateMultipleCustomersSettingType(_settingContext, customerIds, customerLevel, settingType, uplineLevel, settingTypeMultipleResult);
            }

            return settingTypeMultipleResult;
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
            var customerIdNameContainer = new Dictionary<int, string>();
            var customerLength = customerIds.Count();

            for (int i = 0; i < customerLength; i++)
            {
                customerIdNameContainer.Add(customerIds.ElementAt(i), customerNames.ElementAt(i));
            }

            return customerIdNameContainer;
        }
        private void LogUpdateMultipleCustomersSettingTypeError(
            IEnumerable<int> customerIds,
            Level customerLevel,
            Level uplineLevel,
            byte productId,
            SettingType customerSettingType,
            Exception ex)
        {
            var customerSettingException = new CustomerSettingException(ex.Message, ex);
            customerSettingException.SetSettingContextInfo("Update Setting", _settingContext);
            customerSettingException.SetCustomerInfo(string.Join(",", customerIds), customerLevel, uplineLevel);
            customerSettingException.SetProductInfo(productId, customerSettingType);

            _logger.Error(ex.Message, ex);
        }
    }
}