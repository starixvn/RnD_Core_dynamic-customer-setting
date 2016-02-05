namespace Fanex.CustomerSetting.Casino.Tests.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using NUnit.Framework;
    using NSubstitute;
    using Models;
    using System.Linq;
    using CustomerSetting.Services;
    using Data.Repository;
    using CustomerSetting.Models;
    using Logging;
    using Casino.Services.Impl;
    using Criteria;
    using CustomerSetting.Models.Requests;
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
        public void GetCustomerSetting_CustomerSettingQueryIsNull_ThrowInvalidOperationExceptionn()
        {
            Assert.Throws<InvalidOperationException>(() => _querySettingService.GetCustomerSetting(null));
        }

        [Test]
        public void GetCustomerSetting_CustomerCasinoStatusIsHidden_ThrowInvalidOperationException()
        {
            //// Arrange
            var customerQuery = new CustomerSettingQuery(10000, 1, 110, 2);

            var customerCasinoStatus = new CustomerCasinoStatus { IsHidden = true };
            _dymanicRepository
                .Get<CustomerCasinoStatus>(new GetCustomerCasinoStatusCriteria
                {
                    ProductId = customerQuery.ProductId,
                    CustId = customerQuery.CustomerId
                })
                .ReturnsForAnyArgs(customerCasinoStatus);

            //// Act & Assert
            Assert.Throws<InvalidOperationException>(
                () => _querySettingService.GetCustomerSetting(customerQuery));
        }
    }
}
