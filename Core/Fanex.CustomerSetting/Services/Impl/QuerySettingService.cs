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
    using Fanex.Data.Repository;
    using Fanex.Logging;

    public class QuerySettingService : IQuerySettingService
    {
        protected readonly ISettingContext SettingContext;
        protected readonly IValidationService ValidationService;
        protected readonly IProductService ProductService;
        protected readonly IResourceService ResourceService;
        protected readonly ITaskService TaskService;
        protected readonly ILogger Logger;
        protected readonly ISettingTypeServiceFactory SettingTypeServiceFactory;
        protected readonly IDynamicRepository DynamicRepository;

        public QuerySettingService(
            ISettingContext settingContext,
            IValidationService validationService = null,
            IProductService productService = null,
            IResourceService resourceService = null,
            ITaskService pluggableTaskService = null,
            ILogger logger = null,
            ISettingTypeServiceFactory settingTypeServiceFactory = null,
            IDynamicRepository dynamicRepository = null)
        {
            Check.Argument.IsNotNull(settingContext, "settingContext");
            Check.Argument.IsNotNull(settingContext.Actor, "settingContext.Actor");

            SettingContext = settingContext;
            ProductService = productService ?? new ProductService();
            ValidationService = validationService ?? new ValidationService(SettingContext);
            ResourceService = resourceService ?? new ResourceService();
            TaskService = pluggableTaskService ?? new TaskService(SettingContext);
            Logger = logger ?? Logging.Logger.Log;
            SettingTypeServiceFactory = settingTypeServiceFactory ?? new SettingTypeServiceFactory();
            DynamicRepository = dynamicRepository ?? new DynamicRepository();
        }

        public QuerySettingService(ISettingContext settingContext)
            : this(settingContext, null, null, null, null, null, null, null)
        {
        }

        protected QuerySettingService()
        {
        }

        public virtual ISetting GetCustomerSetting(CustomerSettingQuery query)
        {
            Validate(query);

            // Run plugged tasks before querying data.
            TaskService.Run(query.ProductId, query);

            var customerInfo = GetCustomerInfo(query.CustomerId);
            var product = GetProductInfo(query.ProductId, SettingContext.System);
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
                throw new ArgumentNullException("query");
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
        protected virtual Product GetProductInfo(byte productId, byte systemId)
        {
            var productSettingTypes = ProductService.GetProductSettingTypes(productId, systemId)
                .Where(product => product.DisplayMode != DisplayMode.Hidden);
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
            SettingType customerSettingType = productSettingType;

            try
            {
                var settingTypeService = SettingTypeServiceFactory.GetInstance(productSettingType);

                // Run plugged tasks of setting type before querying setting type's data.
                TaskService.Run(query.ProductId, productSettingType.Id, query);

                customerSettingType = settingTypeService.GetCustomerSettingType(SettingContext, query);
                customerSettingType.Id = productSettingType.Id;
                customerSettingType.SettingLevel = productSettingType.SettingLevel;
                customerSettingType.DisplayOrder = productSettingType.DisplayOrder;

                if (customerSettingType.DisplayMode != DisplayMode.ReadOnly)
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