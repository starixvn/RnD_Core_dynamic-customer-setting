namespace Fanex.CustomerSetting.NumberGames2.Tests.Models
{
    using System.Collections.Generic;
    using CustomerSetting.Models;
    using CustomerSetting.Models.Requests;
    using CustomerSetting.NumberGames2.Services.Impl;
    using CustomerSetting.Services;
    using NSubstitute;

    public class QueryNumberGames2SettingServiceStub : QueryNumberGames2SettingService
    {
        private readonly short[] _expectedSettingTypes;

        public QueryNumberGames2SettingServiceStub(
            ISettingContext settingContext,
            IResourceService resourceService,
            ITaskService pluggableTaskService,
            short[] expectedSettingTypes)
            : base(settingContext, resourceService, pluggableTaskService)
        {
            _expectedSettingTypes = expectedSettingTypes ?? new short[] { 11, 3, 9, 1 };
        }

        protected override CustomerInfo GetCustomerInfo(int customerId)
            => new CustomerInfo();

        protected override Product GetProductInfo(byte productId, byte systemId, byte settingStatus)
            => new Product
            {
                Id = productId,
                Name = "NG2"
            };

        protected override IList<SettingType> GetCustomerSettingTypes(CustomerSettingQuery query, IEnumerable<SettingType> productSettingTypes)
        {
            var customerSettingTypes = new List<SettingType>();

            foreach (var settingTypeId in _expectedSettingTypes)
            {
                customerSettingTypes.Add(new SettingType
                {
                    Id = settingTypeId,
                    SettingItems = new List<SettingItemDefault>
                    {
                        Substitute.For<SettingItemDefault>()
                    }
                });
            }

            return customerSettingTypes;
        }

        protected override void Validate(CustomerSettingQuery query)
        {
            // Do nothing.
        }
    }
}