namespace Fanex.CustomerSetting.Casino.Tests.Models
{
    using System.Collections.Generic;
    using Casino.Services;
    using Casino.Services.Impl;
    using CustomerSetting.Models;
    using CustomerSetting.Models.Requests;
    using CustomerSetting.Services;
    using NSubstitute;

    public class QueryCasinoSettingServiceStub : QueryCasinoSettingService
    {
        public QueryCasinoSettingServiceStub(ISettingContext settingContext, ICustomerProductSyncedStatusService customerProductSyncedStatusService)
            : base(settingContext, Substitute.For<ITaskService>(), customerProductSyncedStatusService)
        {
        }

        protected override CustomerInfo GetCustomerInfo(int customerId)
            => new CustomerInfo();

        protected override Product GetProductInfo(byte productId, byte systemId, byte settingStatus)
            => new Product
            {
                Id = productId
            };

        protected override IList<SettingType> GetCustomerSettingTypes(CustomerSettingQuery query, IEnumerable<SettingType> productSettingTypes)
            => Substitute.For<IList<SettingType>>();

        protected override void Validate(CustomerSettingQuery query)
        {
            // Do nothing.
        }
    }
}