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
    using Fanex.Logging;
    using Fanex.NAP.Framework.Data;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class SettingServiceTestsGetCustomerSetting
    {
        private ISettingService _settingService;
        private IValidationService _validationService;
        private IProductService _productService;
        private ISettingContext _settingContext;
        private IDynamicRepository _dymanicRepository;
        private ISettingResolver _settingResolver;
        private ISettingTypeDataAccess _settingDataAccess;
        private IResourceService _resourceService;
        private ILogger _logger;
        private SettingPreTasks _settingPreTasksHandler;
        private SettingPostTasks _settingPostTasksHandler;

        [SetUp]
        public void Setup()
        {
            _dymanicRepository = Substitute.For<IDynamicRepository>();
            _settingContext = new SettingContextStub();
            _productService = Substitute.For<IProductService>();
            _validationService = Substitute.For<IValidationService>();
            _settingResolver = Substitute.For<ISettingResolver>();
            _settingDataAccess = Substitute.For<ISettingTypeDataAccess>();
            _resourceService = Substitute.For<IResourceService>();
            _settingPreTasksHandler = Substitute.ForPartsOf<SettingPreTasks>();
            _settingPostTasksHandler = Substitute.ForPartsOf<SettingPostTasks>(_resourceService);
            _logger = Substitute.For<ILogger>();

            _settingService = new SettingService(
                _settingContext,
                _validationService,
                _productService,
                _settingResolver,
                _dymanicRepository,
                _resourceService,
                _logger);
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

        [TestCase(0)]
        [TestCase(-1)]
        public void GetCustomerSetting_CustomerIdIsNegativeOrZero_ThrowArgumentOutOfRangeException(int customerId)
        {
            //// Arrange
            QuerySettingParam querySettingParam = new QuerySettingParam
            {
                CustomerId = customerId
            };

            //// Act
            Assert.Throws<ArgumentOutOfRangeException>(
                new TestDelegate(
                    () => _settingService.GetCustomerSetting(querySettingParam)));
        }

        [Test]
        public void GetCustomerSetting_ProductIdIsZero_ThrowArgumentOutOfRangeException()
        {
            //// Arrange
            QuerySettingParam querySettingParam = InstanceCreator.CreateQuerySettingParam();
            querySettingParam.ProductId = 0;

            //// Act
            Assert.Throws<ArgumentOutOfRangeException>(
                new TestDelegate(
                    () => _settingService.GetCustomerSetting(querySettingParam)));
        }

        [Test]
        public void GetCustomerSetting_CustomerLevelValueIsZero_ThrowArgumentOutOfRangeException()
        {
            //// Arrange
            QuerySettingParam querySettingParam = InstanceCreator.CreateQuerySettingParam();
            querySettingParam.CustomerLevel = (CustomerLevel)0;

            //// Act
            Assert.Throws<ArgumentOutOfRangeException>(
                new TestDelegate(
                    () => _settingService.GetCustomerSetting(querySettingParam)));
        }

        [Test]
        public void GetCustomerSetting_Always_ExecuteValidateQuerySettingMethodOnValidationService()
        {
            //// Arrange
            QuerySettingParam querySettingParam = InstanceCreator.CreateQuerySettingParam();
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
            QuerySettingParam querySettingParam = InstanceCreator.CreateQuerySettingParam();
            _validationService.ValidateQuerySetting(Arg.Any<QuerySettingParam>()).Returns(Result.AccessDenied);

            //// Act
            Assert.Throws<InvalidOperationException>(
                new TestDelegate(
                    () => _settingService.GetCustomerSetting(querySettingParam)));
        }

        private void AssumeQuerySettingActionPassedValidation()
        {
            _validationService.ValidateQuerySetting(Arg.Any<QuerySettingParam>()).Returns(Result.Success);
        }

        [Test]
        public void GetCustomerSetting_Always_ExecuteGetCustomerMethodOnDynamicRepository()
        {
            //// Arrange
            QuerySettingParam querySettingParam = InstanceCreator.CreateQuerySettingParam();
            AssumeQuerySettingActionPassedValidation();

            //// Act
            _settingService.GetCustomerSetting(querySettingParam);

            //// Assert
            _dymanicRepository.Received().Get<CustomerInfo>(Arg.Is<CustomerQueryCriteria>(
                    criteria => criteria.CustId == querySettingParam.CustomerId));
        }

        [Test]
        public void GetCustomerSetting_CustomerIsValid_ReturnCustomerId()
        {
            //// Arrange
            QuerySettingParam querySettingParam = InstanceCreator.CreateQuerySettingParam();
            AssumeQuerySettingActionPassedValidation();

            //// Act
            var actualCustomerSetting = _settingService.GetCustomerSetting(querySettingParam);

            //// Assert
            Assert.AreEqual(querySettingParam.CustomerId, actualCustomerSetting.CustomerId);
        }

        [Test]
        public void GetCustomerSetting_CustomerIsValid_ReturnCustomerLevel()
        {
            //// Arrange
            QuerySettingParam querySettingParam = InstanceCreator.CreateQuerySettingParam();
            AssumeQuerySettingActionPassedValidation();

            //// Act
            var actualCustomerSetting = _settingService.GetCustomerSetting(querySettingParam);

            //// Assert
            Assert.AreEqual(querySettingParam.CustomerLevel, actualCustomerSetting.CustomerLevel);
        }

        [Test]
        public void GetCustomerSetting_CustomerIsValid_ReturnCustomerInformation()
        {
            //// Arrange
            QuerySettingParam querySettingParam = InstanceCreator.CreateQuerySettingParam();
            AssumeQuerySettingActionPassedValidation();
            var expectedCustomerInfo = new CustomerInfo { UserName = "CustomerUserName" };
            _dymanicRepository.Get<CustomerInfo>(Arg.Any<CustomerQueryCriteria>()).Returns(expectedCustomerInfo);

            //// Act
            var actualCustomerSetting = _settingService.GetCustomerSetting(querySettingParam);

            //// Assert
            Assert.AreEqual(expectedCustomerInfo.UserName, actualCustomerSetting.CustomerInfo.UserName);
        }

        [Test]
        public void GetCustomerSetting_Always_ExecuteGetProductSettingTypesMethodOnProductService()
        {
            //// Arrange
            QuerySettingParam querySettingParam = InstanceCreator.CreateQuerySettingParam();
            AssumeQuerySettingActionPassedValidation();

            //// Act
            _settingService.GetCustomerSetting(querySettingParam);

            //// Assert
            _productService.Received().GetProductSettingTypes(
                Arg.Is<byte>(productId => productId == querySettingParam.ProductId));
        }

        [Test]
        public void GetCustomerSetting_ExecuteGetProductSettingTypesMethodOnProductService()
        {
            //// Arrange
            QuerySettingParam querySettingParam = InstanceCreator.CreateQuerySettingParam();
            AssumeQuerySettingActionPassedValidation();

            //// Act
            _settingService.GetCustomerSetting(querySettingParam);

            //// Assert
            _productService.Received().GetProductSettingTypes(Arg.Is<byte>(
                    productId => productId == querySettingParam.ProductId));
        }

        [Test]
        public void GetCustomerSetting_ProductIsValid_ReturnCustomerSettingTypesMatchedWithProduct()
        {
            //// Arrange
            QuerySettingParam querySettingParam = InstanceCreator.CreateQuerySettingParam();
            AssumeQuerySettingActionPassedValidation();

            var productSettingTypes = InstanceCreator.CreateProduct().SettingTypes;
            _productService.GetProductSettingTypes(Arg.Any<byte>()).Returns(productSettingTypes);

            productSettingTypes.First().SettingItems = new List<SettingItemStub> { new SettingItemStub() };
            SetupSettingTypeDataAccessStubForReturningDummySettingTypes(productSettingTypes.First());

            //// Act
            var actualCustomerSetting = _settingService.GetCustomerSetting(querySettingParam);

            //// Assert
            Assert.AreEqual(productSettingTypes.First().Id, actualCustomerSetting.SettingTypes.First().Id);
        }

        [Test]
        public void GetCustomerSetting_ExecuteGetProductNameMethodOnResourceService()
        {
            //// Arrange
            QuerySettingParam querySettingParam = InstanceCreator.CreateQuerySettingParam();
            AssumeQuerySettingActionPassedValidation();

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
            QuerySettingParam querySettingParam = InstanceCreator.CreateQuerySettingParam();
            AssumeQuerySettingActionPassedValidation();
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
            QuerySettingParam querySettingParam = InstanceCreator.CreateQuerySettingParam();
            AssumeQuerySettingActionPassedValidation();

            //// Act
            var actualCustomerSetting = _settingService.GetCustomerSetting(querySettingParam);

            //// Assert
            Assert.AreEqual(querySettingParam.ProductId, actualCustomerSetting.ProductId);
        }

        private IEnumerable<SettingType> SetupProductServiceStubForReturningDummySettingTypes(
            IEnumerable<SettingType> dummySettingTypes = null)
        {
            var productSettingTypes = dummySettingTypes ?? InstanceCreator.CreateProduct().SettingTypes;
            _productService.GetProductSettingTypes(Arg.Any<byte>()).Returns(productSettingTypes);

            return productSettingTypes;
        }

        [Test]
        public void GetCustomerSetting_SettingResolverExecutesResolveSettingPreTasks()
        {
            //// Arrange
            QuerySettingParam querySettingParam = InstanceCreator.CreateQuerySettingParam();
            AssumeQuerySettingActionPassedValidation();
            var dummySettingTypes = SetupProductServiceStubForReturningDummySettingTypes();
            SetupSettingTypeDataAccessStubForReturningDummySettingTypes();

            //// Act
            _settingService.GetCustomerSetting(querySettingParam);

            //// Assert
            _settingResolver.Received(dummySettingTypes.Count()).ResolveSettingPreTasks(Arg.Any<SettingType>());
        }

        [Test]
        public void GetCustomerSetting_SettingTypeHasPreGetCustomerSettingTypeTasks_ExecutePreGetCustomerSettingTypeTasks()
        {
            //// Arrange
            QuerySettingParam querySettingParam = InstanceCreator.CreateQuerySettingParam();
            AssumeQuerySettingActionPassedValidation();
            SetupProductServiceStubForReturningDummySettingTypes();
            SetupSettingTypeDataAccessStubForReturningDummySettingTypes();

            //// Mock PreTasks Handler.
            var settingTypePreTasksMock = Substitute.ForPartsOf<SettingPreTasks>();
            _settingResolver.ResolveSettingPreTasks(Arg.Any<SettingType>()).Returns(settingTypePreTasksMock);

            //// Act
            _settingService.GetCustomerSetting(querySettingParam);

            //// Assert
            settingTypePreTasksMock.Received().PreGetCustomerSettingType(_settingContext, querySettingParam);
        }

        private void SetupSettingTypeDataAccessStubForReturningDummySettingTypes(SettingType dummySettingType = null)
        {
            var customerSettingType = dummySettingType ?? InstanceCreator.CreateCustomerSettingType(1);
            _settingDataAccess.GetCustomerSettingType(Arg.Any<User>(), Arg.Any<QuerySettingParam>()).Returns(customerSettingType);
            _settingResolver.ResolveSettingDataAccess(Arg.Any<SettingType>()).Returns(_settingDataAccess);
        }

        [Test]
        public void GetCustomerSetting_SettingResolverExecutesResolveSettingPostTasksMethod()
        {
            //// Arrange
            QuerySettingParam querySettingParam = InstanceCreator.CreateQuerySettingParam();
            AssumeQuerySettingActionPassedValidation();
            var productSettingTypes = SetupProductServiceStubForReturningDummySettingTypes();
            SetupSettingTypeDataAccessStubForReturningDummySettingTypes();

            //// Act
            _settingService.GetCustomerSetting(querySettingParam);

            //// Assert
            _settingResolver.Received(productSettingTypes.Count()).ResolveSettingPostTasks(Arg.Any<SettingType>());
        }

        [Test]
        public void GetCustomerSetting_SettingDataAccessExecutesGetCustomerSettingTypeMethod()
        {
            //// Arrange
            QuerySettingParam querySettingParam = InstanceCreator.CreateQuerySettingParam();
            AssumeQuerySettingActionPassedValidation();
            var productSettingTypes = SetupProductServiceStubForReturningDummySettingTypes();
            SetupSettingTypeDataAccessStubForReturningDummySettingTypes();

            //// Act
            _settingService.GetCustomerSetting(querySettingParam);

            //// Assert
            _settingDataAccess.Received(productSettingTypes.Count()).GetCustomerSettingType(Arg.Any<User>(), querySettingParam);
        }

        [Test]
        public void GetCustomerSetting_SettingTypeHasPostGetCustomerSettingTypeTasks_ExecutePostGetCustomerSettingTypeTasks()
        {
            //// Arrange
            QuerySettingParam querySettingParam = InstanceCreator.CreateQuerySettingParam();
            AssumeQuerySettingActionPassedValidation();
            SetupProductServiceStubForReturningDummySettingTypes();
            SetupSettingTypeDataAccessStubForReturningDummySettingTypes();

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
        public void GetCustomerSetting_RaisedException_ExecuteErrorOnLogger()
        {
            //// Arrange
            QuerySettingParam querySettingParam = InstanceCreator.CreateQuerySettingParam();
            AssumeQuerySettingActionPassedValidation();
            SetupProductServiceStubForReturningDummySettingTypes();
            _settingResolver.ResolveSettingDataAccess(Arg.Any<SettingType>()).Returns(x => { throw new Exception(); });

            //// Act
            _settingService.GetCustomerSetting(querySettingParam);

            //// Assert
            _logger.Received().Error(Arg.Any<string>(), Arg.Any<Exception>());
        }
    }
}