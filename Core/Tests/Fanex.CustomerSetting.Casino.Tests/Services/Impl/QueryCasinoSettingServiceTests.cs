namespace Fanex.CustomerSetting.Casino.Tests.Services.Impl
{
    using Casino.Models;
    using Casino.Services;
    using CustomerSetting.Models.Enumerations;
    using CustomerSetting.Models.Requests;
    using CustomerSetting.Services;
    using CustomerSetting.LoggingConfig;
    using Models;
    using NSubstitute;
    using NUnit.Framework;
    using Logging.Extensions.Database;

    [TestFixture]
    public class QueryCasinoSettingServiceTests
    {
        private IQuerySettingService _queryCasinoSettingService;
        private ICustomerProductSyncedStatusService _customerProductSyncedStatusService;

        [SetUp]
        public void Setup()
        {
            LoggingConfig.Init(new[] { "pass", "pwd", "security", "Password" }, getDbExceptionInfo: DatabaseInfoGetters.GetDbExceptionInfo);
            var settingContext = new SettingContextStub();
            _customerProductSyncedStatusService = Substitute.For<ICustomerProductSyncedStatusService>();
            _queryCasinoSettingService = new QueryCasinoSettingServiceStub(settingContext, _customerProductSyncedStatusService);
        }

        [TestCase(4)]
        [TestCase(3)]
        [TestCase(2)]
        [TestCase(1)]
        public void GetCustomerSetting_CustomerIsUnsynced_SettingNormalModeIsFalse(byte levelValue)
        {
            // Arrange.
            var query = CreateCustomerSettingQuery(levelValue);
            bool syncedStatus = false;

            _customerProductSyncedStatusService
                .CheckCustomerProductSyncedStatus(query.CustomerId, query.ProductId)
                .ReturnsForAnyArgs(syncedStatus);

            // Act.
            var setting = _queryCasinoSettingService.GetCustomerSetting(query) as CasinoSetting;

            // Assert.
            Assert.AreEqual(false, setting.IsNormalMode);
        }

        [TestCase(4)]
        [TestCase(3)]
        [TestCase(2)]
        [TestCase(1)]
        public void GetCustomerSetting_CustomerIsSynced_SettingNormalModeIsTrue(byte levelValue)
        {
            // Arrange.
            var query = CreateCustomerSettingQuery(levelValue);
            bool syncedStatus = true;

            _customerProductSyncedStatusService
                .CheckCustomerProductSyncedStatus(query.CustomerId, query.ProductId)
                .ReturnsForAnyArgs(syncedStatus);

            // Act.
            var setting = _queryCasinoSettingService.GetCustomerSetting(query) as CasinoSetting;

            // Assert.
            Assert.AreEqual(true, setting.IsNormalMode);
        }

        private CustomerSettingQuery CreateCustomerSettingQuery(byte levelValue)
        {
            Level level = Enumeration.FromValue<Level>(levelValue);
            Level uplineLevel = level.IsMemberLevel() ? Enumeration.FromValue<Level>(++levelValue) : null;
            var query = new CustomerSettingQuery(10000, level, 110, 0, uplineLevel);

            return query;
        }
    }
}