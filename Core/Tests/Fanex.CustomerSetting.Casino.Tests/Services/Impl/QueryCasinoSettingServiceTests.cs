namespace Fanex.CustomerSetting.Casino.Tests.Services.Impl
{
    using System;
    using Casino.Models;
    using Casino.Services.Impl;
    using Criteria;
    using CustomerSetting.Models;
    using CustomerSetting.Models.Enumerations;
    using CustomerSetting.Models.Requests;
    using CustomerSetting.Services;
    using Data.Repository;
    using Logging;
    using Models;
    using NSubstitute;
    using NUnit.Framework;
    [TestFixture]
    public class QueryCasinoSettingServiceTests
    {
        private IQuerySettingService _querySettingService;
        private ISettingContext _settingContext;
        private IValidationService _validationService;
        private IProductService _productService;
        private IResourceService _resourceService;
        private ITaskService _taskService;
        private IDynamicRepository _dymanicRepository;
        private ILogger _logger;
        private ISettingTypeServiceFactory _settingTypeServiceFactory;
        private ISettingTypeService _settingTypeService;

        private CustomerSettingQuery CreateCustomerSettingQuery(byte levelValue)
        {
            Level level = Level.FromValue<Level>(levelValue);
            Level upLineLevel = level.IsMemberLevel() ? Level.FromValue<Level>(++levelValue) : null;
            var query = new CustomerSettingQuery(10000, level, 110, upLineLevel);

            return query;
        }

        [SetUp]
        public void Setup()
        {
            _settingContext = new SettingContextStub();
            _resourceService = Substitute.For<IResourceService>();
            _productService = Substitute.For<IProductService>();
            _validationService = Substitute.For<IValidationService>();
            _taskService = Substitute.For<ITaskService>();
            _logger = Substitute.For<ILogger>();
            _settingTypeServiceFactory = Substitute.For<ISettingTypeServiceFactory>();
            _dymanicRepository = Substitute.For<IDynamicRepository>();

            _settingTypeService = Substitute.For<ISettingTypeService>();

            _querySettingService = new QueryCasinoSettingService(
                _settingContext,
                _validationService,
                _productService,
                _resourceService,
                _taskService,
                _logger,
                _settingTypeServiceFactory,
                _dymanicRepository);
        }

        [Test]
        public void GetCustomerSetting_CustomerCasinoStatusIsNull_ThrowInvalidOperationExceptionn()
        {
            //// Arrange
            var customerQuery = new CustomerSettingQuery(10000, 1, 110, 2);

            CustomerProductStatus customerCasinoStatus = null;
            _dymanicRepository
                .Get<CustomerProductStatus>(new GetCustomerCasinoStatusCriteria
                {
                    ProductId = customerQuery.ProductId,
                    CustId = customerQuery.CustomerId
                })
                .ReturnsForAnyArgs(customerCasinoStatus);

            //// Act & Assert
            Assert.Throws<InvalidOperationException>(
                () => _querySettingService.GetCustomerSetting(customerQuery));
        }

        [Test]
        public void GetCustomerSetting_CustomerCasinoStatusIsHidden_ThrowInvalidOperationException()
        {
            //// Arrange
            var customerQuery = new CustomerSettingQuery(10000, 1, 110, 2);

            var customerCasinoStatus = new CustomerProductStatus { IsHidden = true };
            _dymanicRepository
                .Get<CustomerProductStatus>(new GetCustomerCasinoStatusCriteria
                {
                    ProductId = customerQuery.ProductId,
                    CustId = customerQuery.CustomerId
                })
                .ReturnsForAnyArgs(customerCasinoStatus);

            //// Act & Assert
            Assert.Throws<InvalidOperationException>(
                () => _querySettingService.GetCustomerSetting(customerQuery));
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

            _dymanicRepository
                .Get<CustomerProductStatus>(new GetCustomerCasinoStatusCriteria
                {
                    ProductId = query.ProductId,
                    CustId = query.CustomerId
                })
                .ReturnsForAnyArgs(customerCasinoStatus);

            //// Act
            var setting = _querySettingService.GetCustomerSetting(query) as CasinoSetting;

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

            _dymanicRepository
                .Get<CustomerProductStatus>(new GetCustomerCasinoStatusCriteria
                {
                    ProductId = query.ProductId,
                    CustId = query.CustomerId
                })
                .ReturnsForAnyArgs(customerCasinoStatus);

            //// Act
            var setting = _querySettingService.GetCustomerSetting(query) as CasinoSetting;

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

            _dymanicRepository
                .Get<CustomerProductStatus>(new GetCustomerCasinoStatusCriteria
                {
                    ProductId = query.ProductId,
                    CustId = query.CustomerId
                })
                .ReturnsForAnyArgs(customerCasinoStatus);

            //// Act
            var setting = _querySettingService.GetCustomerSetting(query) as CasinoSetting;

            //// Assert
            Assert.AreEqual(true, setting.IsNormalMode);
        }
    }
}
