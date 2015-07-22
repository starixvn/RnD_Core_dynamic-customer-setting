namespace Fanex.CustomerSetting.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Common.Helper;
    using Fanex.CustomerSetting.Criteria;
    using Fanex.CustomerSetting.Dtos.Params;
    using Fanex.CustomerSetting.Exceptions;
    using Fanex.CustomerSetting.Models;
    using Fanex.Logging;
    using Fanex.NAP.Framework.Data;

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

        public ISetting GetCustomerSetting(QuerySettingParam queryParam)
        {
            Validate(queryParam);

            var customerInfo = GetCustomerInfo(queryParam.CustomerId);
            var product = GetProductInfo(queryParam.ProductId);
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

        private void Validate(QuerySettingParam queryParam)
        {
            Check.Argument.IsNotNull(queryParam, "queryParam");

            var validationResult = _validationService.ValidateQuerySettingAction(queryParam);

            if (validationResult.IsNotOk)
            {
                throw new InvalidOperationException(validationResult.Message);
            }
        }

        /// <summary>
        /// Gets product information, includes list of product setting types.
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <returns></returns>
        private Product GetProductInfo(byte productId)
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

            // TODO Should we user parallel here.
            // How to display setting type by order.
            foreach (var productSettingType in productSettingTypes)
            {
                SettingType customerSettingType = GetCustomerSettingType(queryParam, productSettingType);

                if (customerSettingType.SettingItems.Any())
                {
                    customerSettingTypes.Add(customerSettingType);
                }
            }

            return customerSettingTypes;
        }

        private SettingType GetCustomerSettingType(QuerySettingParam queryParam, SettingType productSettingType)
        {
            SettingType customerSettingType = productSettingType;

            try
            {
                var settingTypeService = _settingTypeServiceFactory.GetInstance(productSettingType);

                customerSettingType = settingTypeService.GetCustomerSettingType(_settingContext, queryParam, productSettingType);
                customerSettingType.Id = productSettingType.Id;
                customerSettingType.SettingTemplate = productSettingType.SettingTemplate;
                customerSettingType.SettingLevel = productSettingType.SettingLevel;
            }
            catch (Exception ex)
            {
                WriteLog(queryParam, productSettingType, ex);
                //// TODO Should we create ErrorSettingType here.

                customerSettingType.SettingItems = new List<ISettingItem> { new SettingItemError() };
            }

            return customerSettingType;
        }

        private void WriteLog(QuerySettingParam queryParam, SettingType productSettingType, Exception ex)
        {
            var exception = new CustomerSettingException(ex.Message, ex);
            exception.SetSettingContextInfo("Query Setting", _settingContext);
            exception.SetCustomerInfo(queryParam.CustomerId, queryParam.CustomerLevel, queryParam.UplineLevel);
            exception.SetProductInfo(queryParam.ProductId, productSettingType);

            _logger.Error(exception.Message, exception);
        }
    }
}