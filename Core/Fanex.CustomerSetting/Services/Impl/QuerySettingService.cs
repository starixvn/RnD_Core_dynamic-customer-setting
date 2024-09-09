namespace Fanex.CustomerSetting.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common.Helper;
    using Criteria;
    using Data.Repository;
    using Exceptions;
    using Fanex.CustomerSetting.Models.Enumerations;
    using Logging;
    using Models;
    using Models.Constants;
    using Models.Requests;

    public class QuerySettingService : IQuerySettingService
    {
        private readonly ISettingContext _settingContext;
        private readonly IValidationService _validationService;
        private readonly IProductService _productService;
        private readonly IResourceService _resourceService;
        private readonly ITaskService _taskService;
        private readonly ILogger _logger;
        private readonly ISettingTypeServiceFactory _settingTypeServiceFactory;
        private readonly IDynamicRepository _dynamicRepository;

        public QuerySettingService(
            ISettingContext settingContext,
            IValidationService validationService,
            IProductService productService,
            IResourceService resourceService,
            ITaskService pluggableTaskService,
            ILogger logger,
            ISettingTypeServiceFactory settingTypeServiceFactory,
            IDynamicRepository dynamicRepository)
        {
            Check.Argument.IsNotNull(settingContext, nameof(settingContext));
            Check.Argument.IsNotNull(settingContext.Actor, nameof(settingContext.Actor));

            _settingContext = settingContext;
            _productService = productService ?? new ProductService();
            _validationService = validationService ?? new ValidationService(_settingContext);
            _resourceService = resourceService ?? new ResourceService();
            _taskService = pluggableTaskService ?? new TaskService(_settingContext);
            _logger = logger ?? Logging.Logger.Log;
            _settingTypeServiceFactory = settingTypeServiceFactory ?? new SettingTypeServiceFactory();
            _dynamicRepository = dynamicRepository ?? new DynamicRepository();
        }

        public QuerySettingService(ISettingContext settingContext)
            : this(settingContext, null, null, null, null, null, null, null)
        {
        }

        internal QuerySettingService()
        {
        }

        protected ISettingContext SettingContext => _settingContext;

        protected IValidationService ValidationService => _validationService;

        protected IProductService ProductService => _productService;

        protected IResourceService ResourceService => _resourceService;

        protected ITaskService TaskService => _taskService;

        protected ILogger Logger => _logger;

        protected ISettingTypeServiceFactory SettingTypeServiceFactory => _settingTypeServiceFactory;

        protected IDynamicRepository DynamicRepository => _dynamicRepository;

        public virtual ISetting GetCustomerSetting(CustomerSettingQuery query)
        {
            Validate(query);

            // Run plugged tasks before querying data.
            TaskService.Run(query.ProductId, query);

            var customerInfo = GetCustomerInfo(query.CustomerId);
            var product = GetProductInfo(query.ProductId, SettingContext.System, query.SettingStatus);
            var customerSettingTypes = GetCustomerSettingTypes(query, product.SettingTypes);

            var setting = new Setting(
                query.CustomerId,
                query.CustomerLevel,
                customerInfo,
                product.Id,
                product.Name,
                customerSettingTypes);

            // Run plugged tasks after querying setting data of a product.
            TaskService.Run(query.ProductId, query, setting);

            return setting;
        }

        protected virtual void Validate(CustomerSettingQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            var validationResult = ValidationService.ValidateQuerySettingAction(query);

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
        protected virtual Product GetProductInfo(byte productId, byte systemId, byte settingStatus)
        {
            var productSettingTypes = ProductService.GetProductSettingTypes(productId, systemId)
                .Where(product => product.DisplayMode != DisplayModes.Hidden);
            var productName = ResourceService.GetProductName(productId, SettingContext.Language);

            return new Product
            {
                Id = productId,
                Name = productName,
                SettingTypes = productSettingTypes
            };
        }

        protected virtual CustomerInfo GetCustomerInfo(int customerId)
        {
            var customerInfo = DynamicRepository.Get<CustomerInfo>(new CustomerInfoQueryCriteria
            {
                CustId = customerId,
                System = SettingContext.System
            });

            return customerInfo;
        }

        /// <summary>
        /// Get {Customer}{SettingType} => means Get SuperPositionTaking, MasterCommission ....
        /// </summary>
        /// <param name="query">The query setting parameters.</param>
        /// <param name="productSettingTypes">The product setting types.</param>
        /// <returns>Customer setting on each setting type.</returns>
        protected virtual IList<SettingType> GetCustomerSettingTypes(CustomerSettingQuery query, IEnumerable<SettingType> productSettingTypes)
        {
            var customerSettingTypes = new List<SettingType>();

            // TODO Should we user parallel here.
            // How to display setting type by order.
            foreach (var productSettingType in productSettingTypes)
            {
                SettingType customerSettingType = GetCustomerSettingType(query, productSettingType);

                if (customerSettingType.SettingItems.Any())
                {
                    customerSettingTypes.Add(customerSettingType);
                }
            }

            return customerSettingTypes;
        }

        protected virtual SettingType GetCustomerSettingType(CustomerSettingQuery query, SettingType productSettingType)
        {
#pragma warning disable S1854 // Unused assignments should be removed
            SettingType customerSettingType = productSettingType;
#pragma warning restore S1854 // Unused assignments should be removed

            try
            {
                var settingTypeService = SettingTypeServiceFactory.GetInstance(productSettingType);

                // Run plugged tasks of setting type before querying setting type's data.
                TaskService.Run(query.ProductId, productSettingType.Id, query);

                customerSettingType = settingTypeService.GetCustomerSettingType(SettingContext, query);
                customerSettingType.Id = productSettingType.Id;
                customerSettingType.SettingLevel = productSettingType.SettingLevel;
                customerSettingType.DisplayOrder = productSettingType.DisplayOrder;

                if (query.CustomerLevel.IsSuperLevel() && SystemType.IsExternalNap(SettingContext.System) && SystemType.IsNotExternalNapBetSetting(SettingContext.System))
                {
                    customerSettingType.DisplayMode = DisplayModes.ReadOnly;
                }

                if (customerSettingType.DisplayMode != DisplayModes.ReadOnly)
                {
                    customerSettingType.DisplayMode = productSettingType.DisplayMode;
                }

                // Run plugged tasks on setting type after querying setting type's data.
                TaskService.Run(query.ProductId, productSettingType.Id, query, customerSettingType);
            }
            catch (Exception ex)
            {
                WriteLog(query, productSettingType, ex);

                customerSettingType.IsError = true;
                customerSettingType.SettingItems = new List<ISettingItem> { new SettingItemError() };
            }

            return customerSettingType;
        }

        protected virtual void WriteLog(CustomerSettingQuery query, SettingType productSettingType, Exception ex)
        {
            var exception = new CustomerSettingException(ex.Message, ex);

            exception.SetSettingContextInfo("Query Setting", SettingContext);
            exception.SetCustomerInfo(query.CustomerId, query.CustomerLevel, query.UplineLevel);
            exception.SetProductInfo(query.ProductId, productSettingType);

            Logger.Error(exception.Message, exception);
        }
    }
}