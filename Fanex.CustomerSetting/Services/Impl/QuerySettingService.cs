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

    public class QuerySettingService : IQuerySettingService
    {
        private readonly ISettingContext _settingContext;
        private readonly IValidationService _validationService;
        private readonly IProductService _productService;
        private readonly IResourceService _resourceService;
        private readonly ILoggingService _loggingService;
        private readonly ISettingResolver _settingResolver;
        private readonly IDynamicRepository _dynamicRepository;

        public QuerySettingService(
            ISettingContext settingContext,
            IValidationService validationService = null,
            IProductService productService = null,
            IResourceService resourceService = null,
            ILoggingService loggingService = null,
            ISettingResolver settingResolver = null,
            IDynamicRepository dynamicRepository = null)
        {
            Check.Argument.IsNotNull(settingContext, "settingContext");
            Check.Argument.IsNotNull(settingContext.Actor, "settingContext.Actor");
            Check.Argument.IsNotNegativeOrZero(settingContext.Actor.Id, "settingContext.Actor.Id");
            Check.Argument.IsNotNegativeOrZero(settingContext.Actor.Level.Value, "settingContext.Actor.Level");

            _settingContext = settingContext;
            _productService = productService ?? new ProductService();
            _validationService = validationService ?? new ValidationService(_settingContext);
            _resourceService = resourceService ?? new ResourceService();
            _loggingService = loggingService ?? new LoggingService(_settingContext);
            _settingResolver = settingResolver ?? new SettingResolver();
            _dynamicRepository = dynamicRepository ?? new DynamicRepository();
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
            var customerInfo = _dynamicRepository.Get<CustomerInfo>(new CustomerInfoQueryCriteria
            {
                CustId = customerId
            });

            return customerInfo;
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
                SettingType customerSettingType = new SettingType();

                try
                {
                    RunPreGetCustomerSettingTypeTasks(queryParam, productSettingType);

                    customerSettingType = GetCustomerSettingType(queryParam, productSettingType);

                    customerSettingType = RunPostGetCustomerSettingTypeTasks(queryParam, customerSettingType);
                }
                catch (Exception ex)
                {
                    _loggingService.LogQueryCustomerSettingTypeError(
                        queryParam.CustomerId,
                        queryParam.UplineLevel,
                        queryParam.ProductId,
                        productSettingType,
                        ex);
                }

                if (customerSettingType.SettingItems.Any())
                {
                    customerSettingTypes.Add(customerSettingType);
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

        private SettingType RunPostGetCustomerSettingTypeTasks(QuerySettingParam queryParam, SettingType customerSettingType)
        {
            var postTasks = _settingResolver.ResolveSettingPostTasks(customerSettingType);

            if (postTasks != null
                && IsOverride(postTasks.GetType().GetMethod("PostGetCustomerSettingType")))
            {
                customerSettingType = postTasks.PostGetCustomerSettingType(_settingContext, queryParam, customerSettingType);
            }

            return customerSettingType;
        }

        private bool IsOverride(MethodInfo methodInfo)
        {
            return methodInfo.GetBaseDefinition().DeclaringType != methodInfo.DeclaringType;
        }
    }
}