namespace Fanex.CustomerSetting.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.CustomerSetting.Criteria;
    using Fanex.CustomerSetting.DataAccesses;
    using Fanex.CustomerSetting.Dtos.Params;
    using Fanex.CustomerSetting.Dtos.Results;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Handlers;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Services;
    using Fanex.CustomerSetting.Services.Impl;
    using Fanex.CustomerSetting.Tests.Models;
    using Fanex.NAP.Framework.Data;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class SettingServiceTestsGetCustomerSetting
    {
        private ISettingService _settingService;
        private ISettingContext _settingContext;
        private IValidationService _validationService;
        private IProductService _productService;
        private IResourceService _resourceService;
        private ILoggingService _loggingService;
        private IDynamicRepository _dymanicRepository;
        private ISettingResolver _settingResolver;
        private ISettingTypeDataAccess _settingDataAccess;

        [SetUp]
        public void Setup()
        {
            _settingContext = new SettingContextStub();
            _resourceService = Substitute.For<IResourceService>();
            _loggingService = Substitute.For<ILoggingService>();
            _productService = Substitute.For<IProductService>();
            _validationService = Substitute.For<IValidationService>();
            _settingResolver = Substitute.For<ISettingResolver>();
            _dymanicRepository = Substitute.For<IDynamicRepository>();

            _settingDataAccess = Substitute.For<ISettingTypeDataAccess>();

            _settingService = new SettingService(
                _settingContext,
                _validationService,
                _productService,
                _resourceService,
                _loggingService,
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
                    () => _settingService.GetCustomerSetting(querySettingParam)));
        }

        [Test]
        public void GetCustomerSetting_Always_CallValidateQuerySetting()
        {
            //// Arrange
            QuerySettingParam querySettingParam = InstanceFactory.CreateQuerySettingParam();
            _validationService.ValidateQuerySetting(Arg.Any<QuerySettingParam>()).Returns(Result.Success);

            //// Act
            _settingService.GetCustomerSetting(querySettingParam);

            //// Assert
            _validationService.Received().ValidateQuerySetting(Arg.Is<QuerySettingParam>(
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
            _validationService.ValidateQuerySetting(Arg.Any<QuerySettingParam>()).Returns(Result.AccessDenied);

            //// Act
            Assert.Throws<InvalidOperationException>(
                new TestDelegate(
                    () => _settingService.GetCustomerSetting(querySettingParam)));
        }

        private void AssumeQuerySettingActionIsValid()
        {
            _validationService.ValidateQuerySetting(Arg.Any<QuerySettingParam>()).Returns(Result.Success);
        }

        [Test]
        public void GetCustomerSetting_Always_CallGetCustomerInformation()
        {
            //// Arrange
            QuerySettingParam querySettingParam = InstanceFactory.CreateQuerySettingParam();
            AssumeQuerySettingActionIsValid();

            //// Act
            _settingService.GetCustomerSetting(querySettingParam);

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
            var actualCustomerSetting = _settingService.GetCustomerSetting(querySettingParam);

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
            var actualCustomerSetting = _settingService.GetCustomerSetting(querySettingParam);

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
            var actualCustomerSetting = _settingService.GetCustomerSetting(querySettingParam);

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
            _settingService.GetCustomerSetting(querySettingParam);

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
            SetupSettingTypeDataAccessStub(productSettingTypes.First());

            //// Act
            var actualCustomerSetting = _settingService.GetCustomerSetting(querySettingParam);

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
            _settingService.GetCustomerSetting(querySettingParam);

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
            var actualCustomerSetting = _settingService.GetCustomerSetting(querySettingParam);

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
            var actualCustomerSetting = _settingService.GetCustomerSetting(querySettingParam);

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
        public void GetCustomerSetting_Always_CallResolveSettingPreTasks()
        {
            //// Arrange
            QuerySettingParam querySettingParam = InstanceFactory.CreateQuerySettingParam();
            AssumeQuerySettingActionIsValid();
            var dummySettingTypes = SetupProductSettingTypesStub();
            SetupSettingTypeDataAccessStub();

            //// Act
            _settingService.GetCustomerSetting(querySettingParam);

            //// Assert
            _settingResolver.Received(dummySettingTypes.Count()).ResolveSettingPreTasks(Arg.Any<SettingType>());
        }

        [Test]
        public void GetCustomerSetting_SettingTypeHasPreGetCustomerSettingTypeTasks_RunTasks()
        {
            //// Arrange
            QuerySettingParam querySettingParam = InstanceFactory.CreateQuerySettingParam();
            AssumeQuerySettingActionIsValid();
            SetupProductSettingTypesStub();
            SetupSettingTypeDataAccessStub();

            //// Mock PreTasks Handler.
            var settingTypePreTasksMock = Substitute.ForPartsOf<SettingPreTasks>();
            _settingResolver.ResolveSettingPreTasks(Arg.Any<SettingType>()).Returns(settingTypePreTasksMock);

            //// Act
            _settingService.GetCustomerSetting(querySettingParam);

            //// Assert
            settingTypePreTasksMock.Received().PreGetCustomerSettingType(_settingContext, querySettingParam);
        }

        private void SetupSettingTypeDataAccessStub(SettingType dummySettingType = null)
        {
            var customerSettingType = dummySettingType ?? InstanceFactory.CreateCustomerSettingType(1);
            _settingDataAccess.GetCustomerSettingType(Arg.Any<User>(), Arg.Any<QuerySettingParam>()).Returns(customerSettingType);
            _settingResolver.ResolveSettingDataAccess(Arg.Any<SettingType>()).Returns(_settingDataAccess);
        }

        [Test]
        public void GetCustomerSetting_Always_CallResolveSettingPostTasksMethod()
        {
            //// Arrange
            QuerySettingParam querySettingParam = InstanceFactory.CreateQuerySettingParam();
            AssumeQuerySettingActionIsValid();
            var productSettingTypes = SetupProductSettingTypesStub();
            SetupSettingTypeDataAccessStub();

            //// Act
            _settingService.GetCustomerSetting(querySettingParam);

            //// Assert
            _settingResolver.Received(productSettingTypes.Count()).ResolveSettingPostTasks(Arg.Any<SettingType>());
        }

        [Test]
        public void GetCustomerSetting_Always_CallGetCustomerSettingType()
        {
            //// Arrange
            QuerySettingParam querySettingParam = InstanceFactory.CreateQuerySettingParam();
            AssumeQuerySettingActionIsValid();
            var productSettingTypes = SetupProductSettingTypesStub();
            SetupSettingTypeDataAccessStub();

            //// Act
            _settingService.GetCustomerSetting(querySettingParam);

            //// Assert
            _settingDataAccess.Received(productSettingTypes.Count()).GetCustomerSettingType(Arg.Any<User>(), querySettingParam);
        }

        [Test]
        public void GetCustomerSetting_SettingTypeHasPostGetCustomerSettingTypeTasks_RunTasks()
        {
            //// Arrange
            QuerySettingParam querySettingParam = InstanceFactory.CreateQuerySettingParam();
            AssumeQuerySettingActionIsValid();
            SetupProductSettingTypesStub();
            SetupSettingTypeDataAccessStub();

            //// Mock PreTasks Handler.
            var settingTypePostTasksMock = Substitute.ForPartsOf<SettingPostTasks>(_resourceService);
            _settingResolver.ResolveSettingPostTasks(Arg.Any<SettingType>()).Returns(settingTypePostTasksMock);

            //// Act
            _settingService.GetCustomerSetting(querySettingParam);

            //// Assert
            settingTypePostTasksMock.Received().PostGetCustomerSettingType(
                _settingContext,
                querySettingParam,
                Arg.Any<SettingType>());
        }

        [Test]
        public void GetCustomerSetting_RaisedException_ExecuteLogQueryCustomerSettingTypeError()
        {
            //// Arrange
            QuerySettingParam querySettingParam = InstanceFactory.CreateQuerySettingParam();
            AssumeQuerySettingActionIsValid();
            SetupProductSettingTypesStub();
            _settingResolver.ResolveSettingDataAccess(Arg.Any<SettingType>()).Returns(x => { throw new Exception(); });

            //// Act
            _settingService.GetCustomerSetting(querySettingParam);

            //// Assert
            _loggingService.Received().LogQueryCustomerSettingTypeError(
                Arg.Is<int>(customerId => customerId == querySettingParam.CustomerId),
                Arg.Is<CustomerLevel?>(uplineLevel => uplineLevel == querySettingParam.UplineLevel),
                Arg.Is<byte>(productId => productId == querySettingParam.ProductId),
                Arg.Any<SettingType>(),
                Arg.Any<Exception>());
        }
    }
}