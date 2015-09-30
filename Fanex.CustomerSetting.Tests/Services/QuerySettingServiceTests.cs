namespace Fanex.CustomerSetting.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.CustomerSetting.Criteria;
    using Fanex.CustomerSetting.Exceptions;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Models.Requests;
    using Fanex.CustomerSetting.Models.Results;
    using Fanex.CustomerSetting.Services;
    using Fanex.CustomerSetting.Services.Impl;
    using Fanex.CustomerSetting.Tests.Models;
    using Fanex.Data.Repository;
    using Fanex.Logging;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class QuerySettingServiceTests
    {
        private IQuerySettingService _querySettingService;
        private ISettingContext _settingContext;
        private IValidationService _validationService;
        private IProductService _productService;
        private IResourceService _resourceService;
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
            _logger = Substitute.For<ILogger>();
            _settingTypeServiceFactory = Substitute.For<ISettingTypeServiceFactory>();
            _dymanicRepository = Substitute.For<IDynamicRepository>();

            _settingTypeService = Substitute.For<ISettingTypeService>();

            _querySettingService = new QuerySettingService(
                _settingContext,
                _validationService,
                _productService,
                _resourceService,
                _logger,
                _settingTypeServiceFactory,
                _dymanicRepository);
        }

        [Test]
        public void GetCustomerSetting_QuerySettingParamIsNull_ThrowArgumentNullException()
        {
            //// Arrange
            CustomerSettingQuery querySettingParam = null;

            //// Act
            Assert.Throws<ArgumentNullException>(
                new TestDelegate(
                    () => _querySettingService.GetCustomerSetting(querySettingParam)));
        }

        [Test]
        public void GetCustomerSetting_Always_CallValidateQuerySetting()
        {
            //// Arrange
            CustomerSettingQuery querySettingParam = InstanceFactory.CreateQuerySettingParam();
            _validationService.ValidateQuerySettingAction(Arg.Any<CustomerSettingQuery>()).Returns(Result.Success);

            //// Act
            _querySettingService.GetCustomerSetting(querySettingParam);

            //// Assert
            _validationService.Received().ValidateQuerySettingAction(Arg.Is<CustomerSettingQuery>(
                actualQuerySettingParam => actualQuerySettingParam.CustomerId == querySettingParam.CustomerId
                    && actualQuerySettingParam.CustomerLevel == querySettingParam.CustomerLevel
                    && actualQuerySettingParam.ProductId == querySettingParam.ProductId
                    && actualQuerySettingParam.UplineLevel == querySettingParam.UplineLevel));
        }

        [Test]
        public void GetCustomerSetting_QuerySettingActionIsNotValid_ThrowInvalidOperationException()
        {
            //// Arrange
            CustomerSettingQuery querySettingParam = InstanceFactory.CreateQuerySettingParam();
            _validationService.ValidateQuerySettingAction(Arg.Any<CustomerSettingQuery>()).Returns(Result.AccessDenied);

            //// Act
            Assert.Throws<InvalidOperationException>(
                new TestDelegate(
                    () => _querySettingService.GetCustomerSetting(querySettingParam)));
        }

        private void AssumeQuerySettingActionIsValid()
        {
            _validationService.ValidateQuerySettingAction(Arg.Any<CustomerSettingQuery>()).Returns(Result.Success);
        }

        [Test]
        public void GetCustomerSetting_Always_CallGetCustomerInformation()
        {
            //// Arrange
            CustomerSettingQuery querySettingParam = InstanceFactory.CreateQuerySettingParam();
            AssumeQuerySettingActionIsValid();

            //// Act
            _querySettingService.GetCustomerSetting(querySettingParam);

            //// Assert
            _dymanicRepository.Received().Get<CustomerInfo>(Arg.Is<CustomerInfoQueryCriteria>(
                    criteria => criteria.CustId == querySettingParam.CustomerId));
        }

        [Test]
        public void GetCustomerSetting_CustomerIsValid_ReturnCustomerId()
        {
            //// Arrange
            CustomerSettingQuery querySettingParam = InstanceFactory.CreateQuerySettingParam();
            AssumeQuerySettingActionIsValid();

            //// Act
            var actualCustomerSetting = _querySettingService.GetCustomerSetting(querySettingParam);

            //// Assert
            Assert.AreEqual(querySettingParam.CustomerId, actualCustomerSetting.CustomerId);
        }

        [Test]
        public void GetCustomerSetting_CustomerIsValid_ReturnCustomerLevel()
        {
            //// Arrange
            CustomerSettingQuery querySettingParam = InstanceFactory.CreateQuerySettingParam();
            AssumeQuerySettingActionIsValid();

            //// Act
            var actualCustomerSetting = _querySettingService.GetCustomerSetting(querySettingParam);

            //// Assert
            Assert.AreEqual(querySettingParam.CustomerLevel, actualCustomerSetting.CustomerLevel);
        }

        [Test]
        public void GetCustomerSetting_CustomerIsValid_ReturnCustomerInformation()
        {
            //// Arrange
            CustomerSettingQuery querySettingParam = InstanceFactory.CreateQuerySettingParam();
            AssumeQuerySettingActionIsValid();
            var expectedCustomerInfo = new CustomerInfo { UserName = "CustomerUserName" };
            _dymanicRepository.Get<CustomerInfo>(Arg.Any<CustomerInfoQueryCriteria>()).Returns(expectedCustomerInfo);

            //// Act
            var actualCustomerSetting = _querySettingService.GetCustomerSetting(querySettingParam);

            //// Assert
            Assert.AreEqual(expectedCustomerInfo.UserName, actualCustomerSetting.CustomerInfo.UserName);
        }

        [Test]
        public void GetCustomerSetting_Always_CallGetProductSettingTypes()
        {
            //// Arrange
            CustomerSettingQuery querySettingParam = InstanceFactory.CreateQuerySettingParam();
            AssumeQuerySettingActionIsValid();

            //// Act
            _querySettingService.GetCustomerSetting(querySettingParam);

            //// Assert
            _productService.Received().GetProductSettingTypes(
                Arg.Is<byte>(productId => productId == querySettingParam.ProductId),
                Arg.Any<byte>());
        }

        [Test]
        public void GetCustomerSetting_ProductHasSettingTypes_ReturnCustomerSettingTypesMatchedWithProduct()
        {
            //// Arrange
            CustomerSettingQuery querySettingParam = InstanceFactory.CreateQuerySettingParam();
            AssumeQuerySettingActionIsValid();

            var productSettingTypes = InstanceFactory.CreateProduct().SettingTypes;
            _productService.GetProductSettingTypes(Arg.Any<byte>(), Arg.Any<byte>()).Returns(productSettingTypes);

            productSettingTypes.First().SettingItems = new List<SettingItemStub> { new SettingItemStub() };
            SetupSettingTypeServiceStub(productSettingTypes.First());

            //// Act
            var actualCustomerSetting = _querySettingService.GetCustomerSetting(querySettingParam);

            //// Assert
            Assert.AreEqual(productSettingTypes.First().Id, actualCustomerSetting.SettingTypes.First().Id);
        }

        [Test]
        public void GetCustomerSetting_Always_CallGetProductName()
        {
            //// Arrange
            CustomerSettingQuery querySettingParam = InstanceFactory.CreateQuerySettingParam();
            AssumeQuerySettingActionIsValid();

            //// Act
            _querySettingService.GetCustomerSetting(querySettingParam);

            //// Assert
            _resourceService.Received().GetProductName(
                Arg.Is<byte>(productId => productId == querySettingParam.ProductId),
                Arg.Is<string>(languageCode => languageCode == _settingContext.Language));
        }

        [Test]
        public void GetCustomerSetting_Always_ReturnProductName()
        {
            //// Arrange
            CustomerSettingQuery querySettingParam = InstanceFactory.CreateQuerySettingParam();
            AssumeQuerySettingActionIsValid();
            var expectedProductName = "ProductName";
            _resourceService.GetProductName(Arg.Any<byte>(), Arg.Any<string>()).Returns(expectedProductName);

            //// Act
            var actualCustomerSetting = _querySettingService.GetCustomerSetting(querySettingParam);

            //// Assert
            Assert.AreEqual(expectedProductName, actualCustomerSetting.ProductName);
        }

        [Test]
        public void GetCustomerSetting_Always_ReturnProductId()
        {
            //// Arrange
            CustomerSettingQuery querySettingParam = InstanceFactory.CreateQuerySettingParam();
            AssumeQuerySettingActionIsValid();

            //// Act
            var actualCustomerSetting = _querySettingService.GetCustomerSetting(querySettingParam);

            //// Assert
            Assert.AreEqual(querySettingParam.ProductId, actualCustomerSetting.ProductId);
        }

        private IEnumerable<SettingType> SetupProductSettingTypesStub(
            IEnumerable<SettingType> dummySettingTypes = null)
        {
            var productSettingTypes = dummySettingTypes ?? InstanceFactory.CreateProduct().SettingTypes;
            _productService.GetProductSettingTypes(Arg.Any<byte>(), Arg.Any<byte>()).Returns(productSettingTypes);

            return productSettingTypes;
        }

        [Test]
        public void GetCustomerSetting_Always_CallResolveSettingTypeService()
        {
            //// Arrange
            CustomerSettingQuery querySettingParam = InstanceFactory.CreateQuerySettingParam();
            AssumeQuerySettingActionIsValid();
            var dummySettingTypes = SetupProductSettingTypesStub();
            SetupSettingTypeServiceStub();

            //// Act
            _querySettingService.GetCustomerSetting(querySettingParam);

            //// Assert
            _settingTypeServiceFactory.Received(dummySettingTypes.Count()).GetInstance(Arg.Any<SettingType>());
        }

        [Test]
        public void GetCustomerSetting_Always_CallGetCustomerSettingType()
        {
            //// Arrange
            CustomerSettingQuery querySettingParam = InstanceFactory.CreateQuerySettingParam();
            AssumeQuerySettingActionIsValid();
            var productSettingTypes = SetupProductSettingTypesStub();
            SetupSettingTypeServiceStub();

            //// Act
            _querySettingService.GetCustomerSetting(querySettingParam);

            //// Assert
            _settingTypeService.Received(productSettingTypes.Count()).GetCustomerSettingType(_settingContext, querySettingParam);
        }

        private void SetupSettingTypeServiceStub(SettingType dummySettingType = null)
        {
            var customerSettingType = dummySettingType ?? InstanceFactory.CreateCustomerSettingType(1);
            _settingTypeService.GetCustomerSettingType(Arg.Any<ISettingContext>(), Arg.Any<CustomerSettingQuery>()).Returns(customerSettingType);
            _settingTypeServiceFactory.GetInstance(Arg.Any<SettingType>()).Returns(_settingTypeService);
        }

        [Test]
        public void GetCustomerSetting_RaisedException_AlwaysCallLogError()
        {
            //// Arrange
            CustomerSettingQuery querySettingParam = InstanceFactory.CreateQuerySettingParam();
            AssumeQuerySettingActionIsValid();
            SetupProductSettingTypesStub();

            var expectedException = new Exception("Expected Exception");
            _settingTypeServiceFactory.GetInstance(Arg.Any<SettingType>()).Returns(x => { throw expectedException; });

            //// Act
            _querySettingService.GetCustomerSetting(querySettingParam);

            //// Assert
            _logger.Received().Error(
                Arg.Is<string>(message => message.Contains(expectedException.Message)),
                Arg.Any<CustomerSettingException>());
        }
    }
}