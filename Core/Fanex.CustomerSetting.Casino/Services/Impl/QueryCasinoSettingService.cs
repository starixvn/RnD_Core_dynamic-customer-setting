namespace Fanex.CustomerSetting.Casino.Services.Impl
{
    using System;
    using Common.Helper;
    using Criteria;
    using CustomerSetting.Models;
    using CustomerSetting.Models.Requests;
    using CustomerSetting.Services;
    using CustomerSetting.Services.Impl;
    using Data.Repository;
    using Logging;
    using Models;

    /// <summary>
    /// <see cref="https://redmine.nexcel.vn/projects/allbet/wiki/Allbet_Credit_Changes_On_NAP#2.1"/>
    /// </summary>
    public class QueryCasinoSettingService : QuerySettingService
    {
        public QueryCasinoSettingService(
            ISettingContext settingContext,
            IValidationService validationService = null,
            IProductService productService = null,
            IResourceService resourceService = null,
            ITaskService pluggableTaskService = null,
            ILogger logger = null,
            ISettingTypeServiceFactory settingTypeServiceFactory = null,
            IDynamicRepository dynamicRepository = null)
            : base(settingContext,
                 validationService,
                 productService,
                 resourceService,
                 pluggableTaskService,
                 logger,
                 settingTypeServiceFactory,
                 dynamicRepository)
        {
        }

        public QueryCasinoSettingService(ISettingContext settingContext)
            : this(settingContext, null, null, null, null, null, null, null)
        {
        }

        protected QueryCasinoSettingService()
        {
        }

        public override ISetting GetCustomerSetting(CustomerSettingQuery query)
        {
            var customerCasinoStatus = GetCustomerCasinoStatus(query);

            if (customerCasinoStatus != null && customerCasinoStatus.IsHidden)
            {
                throw new InvalidOperationException($"Cannot get setting for customer {query.CustomerId} due to hidden status on product {query.ProductId}");
            }

            ISetting setting = base.GetCustomerSetting(query);

            return setting;
        }

        internal CustomerCasinoStatus GetCustomerCasinoStatus(CustomerSettingQuery query)
        {
            var customerCasinoStatus = _dynamicRepository.Get<CustomerCasinoStatus>(
                 new GetCustomerCasinoStatusCriteria
                 {
                     ProductId = query.ProductId,
                     CustId = query.CustomerId
                 });

            return customerCasinoStatus;
        }
    }
}
