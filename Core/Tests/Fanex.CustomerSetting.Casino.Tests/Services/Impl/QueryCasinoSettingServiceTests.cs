namespace Fanex.CustomerSetting.Casino.Tests.Services.Impl
{
    using System.Collections.Generic;
    using Casino.Models;
    using Casino.Services;
    using Casino.Services.Impl;
    using CustomerSetting.Models;
    using CustomerSetting.Models.Enumerations;
    using CustomerSetting.Models.Requests;
    using CustomerSetting.Services;
    using Models;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class QueryCasinoSettingServiceTests
    {
        private IQuerySettingService _queryCasinoSettingService;
        private IQuerySettingService _querySettingService;
        private ISettingContext _settingContext;
        private ICustomerProductStatusService _customerProductStatusService;
        private ISetting _setting;

        private CustomerSettingQuery CreateCustomerSettingQuery(byte levelValue)
        {
            Level level = Enumeration.FromValue<Level>(levelValue);
            Level uplineLevel = level.IsMemberLevel() ? Enumeration.FromValue<Level>(++levelValue) : null;
            var query = new CustomerSettingQuery(10000, level, 110, uplineLevel);

            return query;
        }

        [SetUp]
        public void Setup()
        {
            _settingContext = new SettingContextStub();
            _setting = Substitute.For<ISetting>();

            _setting.CustomerId = 1;
            _setting.CustomerLevel = Level.Member;
            _setting.ProductId = 1;
            _setting.SettingTypes = new List<SettingType>();
            _customerProductStatusService = Substitute.For<ICustomerProductStatusService>();
            _querySettingService = Substitute.For<IQuerySettingService>();

            _querySettingService.GetCustomerSetting(Arg.Any<CustomerSettingQuery>()).ReturnsForAnyArgs(_setting);

            _queryCasinoSettingService = new QueryCasinoSettingService(
                _settingContext,
                _querySettingService,
                _customerProductStatusService);
        }

        [TestCase(4)]
        [TestCase(3)]
        [TestCase(2)]
        [TestCase(1)]
        public void GetCustomerSetting_CustomerSyncedStatusIsFalse_SettingNormalModeIsFalse(byte levelValue)
        {
            //// Arrange
            var query = CreateCustomerSettingQuery(levelValue);
            var customerCasinoStatus = new CustomerProductStatus { IsHidden = false, IsSynced = false };
            _customerProductStatusService.GetCustomerStatus(query.CustomerId, query.ProductId)
                          .ReturnsForAnyArgs(customerCasinoStatus);

            //// Act
            var setting = _queryCasinoSettingService.GetCustomerSetting(query) as CasinoSetting;

            //// Assert
            Assert.AreEqual(false, setting.IsNormalMode);
        }

        [TestCase(4)]
        [TestCase(3)]
        [TestCase(2)]
        [TestCase(1)]
        public void GetCustomerSetting_HasAtLeastOneDownlineIsUnsynced_SettingNormalModeIsFalse(byte levelValue)
        {
            //// Arrange
            var query = CreateCustomerSettingQuery(levelValue);
            var customerCasinoStatus = new CustomerProductStatus
            {
                IsHidden = false,
                IsSynced = true,
                IsDownlineSynced = false
            };

            _customerProductStatusService.GetCustomerStatus(query.CustomerId, query.ProductId)
                           .ReturnsForAnyArgs(customerCasinoStatus);

            //// Act
            var setting = _queryCasinoSettingService.GetCustomerSetting(query) as CasinoSetting;

            //// Assert
            Assert.AreEqual(false, setting.IsNormalMode);
        }

        [TestCase(4)]
        [TestCase(3)]
        [TestCase(2)]
        [TestCase(1)]
        public void GetCustomerSetting_CustomerAndAllDownlinesAreSynced_SettingNormalModeIsTrue(byte levelValue)
        {
            //// Arrange
            var query = CreateCustomerSettingQuery(levelValue);
            var customerCasinoStatus = new CustomerProductStatus
            {
                IsHidden = false,
                IsSynced = true,
                IsDownlineSynced = true
            };

            _customerProductStatusService.GetCustomerStatus(query.CustomerId, query.ProductId)
                           .ReturnsForAnyArgs(customerCasinoStatus);

            //// Act
            var setting = _queryCasinoSettingService.GetCustomerSetting(query) as CasinoSetting;

            //// Assert
            Assert.AreEqual(true, setting.IsNormalMode);
        }
    }
}