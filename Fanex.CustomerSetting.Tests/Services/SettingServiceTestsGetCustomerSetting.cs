namespace Fanex.CustomerSetting.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.CustomerSetting.Criteria;
    using Fanex.CustomerSetting.Dtos.Params;
    using Fanex.CustomerSetting.Dtos.Results;
    using Fanex.CustomerSetting.Handlers;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Services;
    using Fanex.CustomerSetting.Services.Impl;
    using Fanex.CustomerSetting.Tests.Models;
    using Fanex.Logging;
    using Fanex.NAP.Framework.Data;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class SettingServiceTestsGetCustomerSetting
    {
        private IQuerySettingService _querySettingService;
        private ISettingContext _settingContext;
        private IValidationService _validationService;
        private IProductService _productService;
        private IResourceService _resourceService;
        private IDynamicRepository _dymanicRepository;
        private ILogger _logger;
        private ISettingTypeServiceFactory _settingResolver;

        private ISettingTypeService _settingTypeService;

        [SetUp]
        public void Setup()
        {
            _settingContext = new SettingContextStub();
            _resourceService = Substitute.For<IResourceService>();
            _productService = Substitute.For<IProductService>();
            _validationService = Substitute.For<IValidationService>();
            _logger = Substitute.For<ILogger>();
            _settingResolver = Substitute.For<ISettingTypeServiceFactory>();
            _dymanicRepository = Substitute.For<IDynamicRepository>();

            _settingTypeService = Substitute.For<ISettingTypeService>();

            _querySettingService = new QuerySettingService(
                _settingContext,
                _validationService,
                _productService,
                _resourceService,
                _logger,
                _settingResolver,
                _dymanicRepository);
        }

        [Test]
        public void GetCustomerSetting_QuerySettingParamIsNull_ThrowArgumentNullException()
        {
            //// Arrange
            QuerySettingParam querySettingParam = null;

            //// Act
            Assert.Throws<ArgumentNullException>(
                new TestDelegate(
                    () => _querySettingService.GetCustomerSetting(querySettingParam)));
        }

        [Test]
        public void GetCustomerSetting_Always_CallValidateQuerySetting()
        {
            //// Arrange
            QuerySettingParam querySettingParam = InstanceFactory.CreateQuerySettingParam();
            _validationService.ValidateQuerySettingAction(Arg.Any<QuerySettingParam>()).Returns(Result.Success);

            //// Act
            _querySettingService.GetCustomerSetting(querySettingParam);

            //// Assert
            _validationService.Received().ValidateQuerySettingAction(Arg.Is<QuerySettingParam>(
                actualQuerySettingParam => actualQuerySettingParam.CustomerId == querySettingParam.CustomerId
                    && actualQuerySettingParam.CustomerLevel == querySettingParam.CustomerLevel
                    && actualQuerySettingParam.ProductId == querySettingParam.ProductId
                    && actualQuerySettingParam.UplineLevel == querySettingParam.UplineLevel));
        }

        [Test]
        public void GetCustomerSetting_QuerySettingActionIsNotValid_ThrowInvalidOperationException()
        {
            //// Arrange
            QuerySettingParam querySettingParam = InstanceFactory.CreateQuerySettingParam();
            _validationService.ValidateQuerySettingAction(Arg.Any<QuerySettingParam>()).Returns(Result.AccessDenied);

            //// Act
            Assert.Throws<InvalidOperationException>(
                new TestDelegate(
                    () => _querySettingService.GetCustomerSetting(querySettingParam)));
        }

        private void AssumeQuerySettingActionIsValid()
        {
            _validationService.ValidateQuerySettingAction(Arg.Any<QuerySettingParam>()).Returns(Result.Success);
        }

        [Test]
        public void GetCustomerSetting_Always_CallGetCustomerInformation()
        {
            //// Arrange
            QuerySettingParam querySettingParam = InstanceFactory.CreateQuerySettingParam();
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
            QuerySettingParam querySettingParam = InstanceFactory.CreateQuerySettingParam();
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
            QuerySettingParam querySettingParam = InstanceFactory.CreateQuerySettingParam();
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
            QuerySettingParam querySettingParam = InstanceFactory.CreateQuerySettingParam();
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
            QuerySettingParam querySettingParam = InstanceFactory.CreateQuerySettingParam();
            AssumeQuerySettingActionIsValid();

            //// Act
            _querySettingService.GetCustomerSetting(querySettingParam);

            //// Assert
            _productService.Received().GetProductSettingTypes(
                Arg.Is<byte>(productId => productId == querySettingParam.ProductId));
        }

        [Test]
        public void GetCustomerSetting_ProductHasSettingTypes_ReturnCustomerSettingTypesMatchedWithProduct()
        {
            //// Arrange
            QuerySettingParam querySettingParam = InstanceFactory.CreateQuerySettingParam();
            AssumeQuerySettingActionIsValid();

            var productSettingTypes = InstanceFactory.CreateProduct().SettingTypes;
            _productService.GetProductSettingTypes(Arg.Any<byte>()).Returns(productSettingTypes);

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
            QuerySettingParam querySettingParam = InstanceFactory.CreateQuerySettingParam();
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
            QuerySettingParam querySettingParam = InstanceFactory.CreateQuerySettingParam();
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
            QuerySettingParam querySettingParam = InstanceFactory.CreateQuerySettingParam();
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
            _productService.GetProductSettingTypes(Arg.Any<byte>()).Returns(productSettingTypes);

            return productSettingTypes;
        }

        [Test]
        public void GetCustomerSetting_Always_CallResolveSettingTypeService()
        {
            //// Arrange
            QuerySettingParam querySettingParam = InstanceFactory.CreateQuerySettingParam();
            AssumeQuerySettingActionIsValid();
            var dummySettingTypes = SetupProductSettingTypesStub();
            SetupSettingTypeServiceStub();

            //// Act
            _querySettingService.GetCustomerSetting(querySettingParam);

            //// Assert
            _settingResolver.Received(dummySettingTypes.Count()).CreateInstance(Arg.Any<SettingType>());
        }

        [Test]
        public void GetCustomerSetting_Always_CallGetCustomerSettingType()
        {
            //// Arrange
            QuerySettingParam querySettingParam = InstanceFactory.CreateQuerySettingParam();
            AssumeQuerySettingActionIsValid();
            var productSettingTypes = SetupProductSettingTypesStub();
            SetupSettingTypeServiceStub();

            //// Act
            _querySettingService.GetCustomerSetting(querySettingParam);

            //// Assert
            _settingTypeService.Received(productSettingTypes.Count()).GetCustomerSettingType(_settingContext, querySettingParam, Arg.Any<SettingType>());
        }

        private void SetupSettingTypeServiceStub(SettingType dummySettingType = null)
        {
            var customerSettingType = dummySettingType ?? InstanceFactory.CreateCustomerSettingType(1);
            _settingTypeService.GetCustomerSettingType(Arg.Any<ISettingContext>(), Arg.Any<QuerySettingParam>(), Arg.Any<SettingType>()).Returns(customerSettingType);
            _settingResolver.CreateInstance(Arg.Any<SettingType>()).Returns(_settingTypeService);
        }

        ////[Test]
        ////public void GetCustomerSetting_RaisedException_ExecuteLogQueryCustomerSettingTypeError()
        ////{
        ////    //// Arrange
        ////    QuerySettingParam querySettingParam = InstanceFactory.CreateQuerySettingParam();
        ////    AssumeQuerySettingActionIsValid();
        ////    SetupProductSettingTypesStub();
        ////    _settingResolver.ResolveSettingDataAccess(Arg.Any<SettingType>()).Returns(x => { throw new Exception(); });

        ////    //// Act
        ////   _querySettingService.GetCustomerSetting(querySettingParam);

        ////    //// Assert
        ////    _loggingService.Received().LogQueryCustomerSettingTypeError(
        ////        Arg.Is<int>(customerId => customerId == querySettingParam.CustomerId),
        ////        Arg.Is<UserLevel>(uplineLevel => uplineLevel == querySettingParam.UplineLevel),
        ////        Arg.Is<byte>(productId => productId == querySettingParam.ProductId),
        ////        Arg.Any<SettingType>(),
        ////        Arg.Any<Exception>());
        ////}
    }
}