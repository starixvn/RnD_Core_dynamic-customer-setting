namespace Fanex.CustomerSetting.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Common.Helper;
    using Fanex.CustomerSetting.Criteria;
    using Fanex.CustomerSetting.Exceptions;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Models.Constants;
    using Fanex.CustomerSetting.Models.Requests;
    using Fanex.CustomerSetting.Resource;
    using Fanex.Data.Repository;
    using Fanex.Logging;

    public class QuerySettingService : IQuerySettingService
    {
        private readonly ISettingContext _settingContext;
        private readonly IValidationService _validationService;
        private readonly IProductService _productService;
        private readonly IResourceService _resourceService;
        private readonly ILogger _logger;
        private readonly ISettingTypeServiceFactory _settingTypeServiceFactory;
        private readonly IDynamicRepository _dynamicRepository;

        public QuerySettingService(
            ISettingContext settingContext,
            IValidationService validationService = null,
            IProductService productService = null,
            IResourceService resourceService = null,
            ILogger logger = null,
            ISettingTypeServiceFactory settingTypeServiceFactory = null,
            IDynamicRepository dynamicRepository = null)
        {
            Check.Argument.IsNotNull(settingContext, "settingContext");
            Check.Argument.IsNotNull(settingContext.Actor, "settingContext.Actor");

            _settingContext = settingContext;
            _productService = productService ?? new ProductService();
            _validationService = validationService ?? new ValidationService(_settingContext);
            _resourceService = resourceService ?? new ResourceService();
            _logger = logger ?? Logger.Log;
            _settingTypeServiceFactory = settingTypeServiceFactory ?? new SettingTypeServiceFactory();
            _dynamicRepository = dynamicRepository ?? new DynamicRepository();
        }

        public Setting GetCustomerSetting(CustomerSettingQuery query)
        {
            Validate(query);

            var customerInfo = GetCustomerInfo(query.CustomerId);
            var product = GetProductInfo(query.ProductId, _settingContext.System);
            var customerSettingTypes = GetCustomerSettingTypes(query, product.SettingTypes);

            var setting = new Setting(
                query.CustomerId,
                query.CustomerLevel,
                customerInfo,
                product.Id,
                product.Name,
                customerSettingTypes);

            return setting;
        }

        private void Validate(CustomerSettingQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }

            var validationResult = _validationService.ValidateQuerySettingAction(query);

            if (validationResult.IsNotOk)
            {
                throw new AccessViolationException(validationResult.Message);
            }
        }

        /// <summary>
        /// Gets product information, includes list of product setting types.
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <returns></returns>
        private Product GetProductInfo(byte productId, byte systemId)
        {
            var productSettingTypes = _productService.GetProductSettingTypes(productId, systemId)
                .Where(product => product.DisplayMode != DisplayMode.Hidden);
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
        /// <param name="query">The query setting parameters.</param>
        /// <param name="productSettingTypes">The product setting types.</param>
        /// <returns>Customer setting on each setting type.</returns>
        private IList<SettingType> GetCustomerSettingTypes(CustomerSettingQuery query, IEnumerable<SettingType> productSettingTypes)
        {
            var customerSettingTypes = new List<SettingType>();

            // TODO Should we user parallel here.
            // How to display setting type by order.
            foreach (var productSettingType in productSettingTypes)
            {
                SettingType customerSettingType = GetCustomerSettingType(query, productSettingType);

                if (!customerSettingType.SettingItems.Any())
                {
                    customerSettingType.IsError = true;
                    customerSettingType.SettingItems = new List<ISettingItem> { new SettingItemError() };
                    WriteLog(query, productSettingType, new Exception(string.Format(Message.SettingTypeHasNoSettingItem, customerSettingType.Name)));
                }

                customerSettingTypes.Add(customerSettingType);
            }

            return customerSettingTypes;
        }

        private SettingType GetCustomerSettingType(CustomerSettingQuery query, SettingType productSettingType)
        {
            SettingType customerSettingType = productSettingType;

            try
            {
                var settingTypeService = _settingTypeServiceFactory.GetInstance(productSettingType);

                customerSettingType = settingTypeService.GetCustomerSettingType(_settingContext, query);
                customerSettingType.Id = productSettingType.Id;
                customerSettingType.SettingLevel = productSettingType.SettingLevel;
                customerSettingType.DisplayOrder = productSettingType.DisplayOrder;
                if (customerSettingType.DisplayMode != DisplayMode.ReadOnly)
                {
                    customerSettingType.DisplayMode = productSettingType.DisplayMode;
                }
            }
            catch (Exception ex)
            {
                WriteLog(query, productSettingType, ex);

                customerSettingType.IsError = true;
                customerSettingType.SettingItems = new List<ISettingItem> { new SettingItemError() };
            }

            return customerSettingType;
        }

        private void WriteLog(CustomerSettingQuery query, SettingType productSettingType, Exception ex)
        {
            var exception = new CustomerSettingException(ex.Message, ex);
            exception.SetSettingContextInfo("Query Setting", _settingContext);
            exception.SetCustomerInfo(query.CustomerId, query.CustomerLevel, query.UplineLevel);
            exception.SetProductInfo(query.ProductId, productSettingType);

            _logger.Error(exception.Message, exception);
        }
    }
}