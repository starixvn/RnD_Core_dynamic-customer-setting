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
    public class SettingServiceTests
    {
        private ISettingService _settingService;
        private IValidationService _validationService;
        private IProductService _productService;
        private ISettingContext _settingContext;
        private IDynamicRepository _dymanicRepository;
        private ISettingResolver _settingResolver;
        private ISettingTypeDataAccess _settingDataAccess;
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
            _settingPreTasksHandler = Substitute.ForPartsOf<SettingPreTasks>(_settingContext);
            _settingPostTasksHandler = Substitute.ForPartsOf<SettingPostTasks>(_settingContext);

            _settingService = new SettingService(
                _settingContext,
                _validationService,
                _productService,
                _settingResolver,
                _dymanicRepository);
        }

        #region Get Customer Setting Tests

        [Test]
        public void GetCustomerSetting_QuerySettingParamsIsNull_ThrowArgumentNullException()
        {
            //// Arrange
            QuerySettingParams querySettingParams = null;

            //// Act
            Assert.Throws<ArgumentNullException>(
                new TestDelegate(
                    () => _settingService.GetCustomerSetting(querySettingParams)));
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void GetCustomerSetting_CustomerIdIsNegativeOrZero_ThrowArgumentOutOfRangeException(int customerId)
        {
            //// Arrange
            QuerySettingParams querySettingParams = new QuerySettingParams
            {
                CustomerId = customerId
            };

            //// Act
            Assert.Throws<ArgumentOutOfRangeException>(
                new TestDelegate(
                    () => _settingService.GetCustomerSetting(querySettingParams)));
        }

        [Test]
        public void GetCustomerSetting_ProductIdIsZero_ThrowArgumentOutOfRangeException()
        {
            //// Arrange
            QuerySettingParams querySettingParams = FakeObjectManager.CreateQuerySettingParams();
            querySettingParams.ProductId = 0;

            //// Act
            Assert.Throws<ArgumentOutOfRangeException>(
                new TestDelegate(
                    () => _settingService.GetCustomerSetting(querySettingParams)));
        }

        [Test]
        public void GetCustomerSetting_CustomerLevelValueIsZero_ThrowArgumentOutOfRangeException()
        {
            //// Arrange
            QuerySettingParams querySettingParams = FakeObjectManager.CreateQuerySettingParams();
            querySettingParams.CustomerLevel = (CustomerLevel)0;

            //// Act
            Assert.Throws<ArgumentOutOfRangeException>(
                new TestDelegate(
                    () => _settingService.GetCustomerSetting(querySettingParams)));
        }

        [Test]
        public void GetCustomerSetting_ExecuteValidateQuerySettingMethodOnValidationService()
        {
            //// Arrange
            QuerySettingParams querySettingParams = FakeObjectManager.CreateQuerySettingParams();
            _validationService.ValidateQuerySetting(Arg.Any<QuerySettingParams>()).Returns(Result.Success);

            //// Act
            _settingService.GetCustomerSetting(querySettingParams);

            //// Assert
            _validationService.Received().ValidateQuerySetting(Arg.Is<QuerySettingParams>(
                actualQuerySettingParams => actualQuerySettingParams.CustomerId == querySettingParams.CustomerId
                    && actualQuerySettingParams.CustomerLevel == querySettingParams.CustomerLevel
                    && actualQuerySettingParams.ProductId == querySettingParams.ProductId
                    && actualQuerySettingParams.UplineLevel == querySettingParams.UplineLevel));
        }

        [Test]
        public void GetCustomerSetting_QuerySettingIsNotValid_ThrowInvalidOperationException()
        {
            //// Arrange
            QuerySettingParams querySettingParams = FakeObjectManager.CreateQuerySettingParams();
            _validationService.ValidateQuerySetting(Arg.Any<QuerySettingParams>()).Returns(Result.AccessDenied);

            //// Act
            Assert.Throws<InvalidOperationException>(
                new TestDelegate(
                    () => _settingService.GetCustomerSetting(querySettingParams)));
        }

        [Test]
        public void GetCustomerSetting_ExecuteGetCustomerMethodOnDynamicRepository()
        {
            //// Arrange
            QuerySettingParams querySettingParams = FakeObjectManager.CreateQuerySettingParams();
            _validationService.ValidateQuerySetting(Arg.Any<QuerySettingParams>()).Returns(Result.Success);

            //// Act
            _settingService.GetCustomerSetting(querySettingParams);

            //// Assert
            _dymanicRepository.Received().Get<Customer>(Arg.Is<CustomerQueryCriteria>(
                    criteria => criteria.CustId == querySettingParams.CustomerId));
        }

        [Test]
        public void GetCustomerSetting_ExecuteGetProductSettingTypesMethodOnProductService()
        {
            //// Arrange
            QuerySettingParams querySettingParams = FakeObjectManager.CreateQuerySettingParams();
            _validationService.ValidateQuerySetting(Arg.Any<QuerySettingParams>()).Returns(Result.Success);

            //// Act
            _settingService.GetCustomerSetting(querySettingParams);

            //// Assert
            _productService.Received().GetProductSettingTypes(Arg.Is<byte>(
                    productId => productId == querySettingParams.ProductId));
        }

        [Test]
        public void GetCustomerSetting_ExecuteResolveSettingPreTasksMethodOnSettingResolver()
        {
            //// Arrange
            QuerySettingParams querySettingParams = FakeObjectManager.CreateQuerySettingParams();
            var productSettingTypes = FakeObjectManager.CreateProduct().SettingTypes;
            _validationService.ValidateQuerySetting(querySettingParams).Returns(Result.Success);
            _productService.GetProductSettingTypes(querySettingParams.ProductId).Returns(productSettingTypes);
            SetupSettingTypeDataAccessStubForQuery(querySettingParams);

            //// Act
            _settingService.GetCustomerSetting(querySettingParams);

            //// Assert
            _settingResolver.Received(productSettingTypes.Count()).ResolveSettingPreTasks(Arg.Any<SettingType>());
        }

        private void SetupSettingTypeDataAccessStubForQuery(QuerySettingParams querySettingParams)
        {
            var customerSettingType = FakeObjectManager.CreateSettingType(1);
            _settingDataAccess.GetCustomerSettingType(Arg.Any<User>(), querySettingParams).Returns(customerSettingType);
            _settingResolver.ResolveSettingDataAccess(Arg.Any<SettingType>()).Returns(_settingDataAccess);
        }

        [Test]
        public void GetCustomerSetting_ExecuteResolveSettingPostTasksMethodOnSettingResolver()
        {
            //// Arrange
            QuerySettingParams querySettingParams = FakeObjectManager.CreateQuerySettingParams();
            var productSettingTypes = FakeObjectManager.CreateProduct().SettingTypes;
            _validationService.ValidateQuerySetting(querySettingParams).Returns(Result.Success);
            _productService.GetProductSettingTypes(querySettingParams.ProductId).Returns(productSettingTypes);
            SetupSettingTypeDataAccessStubForQuery(querySettingParams);

            //// Act
            _settingService.GetCustomerSetting(querySettingParams);

            //// Assert
            _settingResolver.Received(productSettingTypes.Count()).ResolveSettingPostTasks(Arg.Any<SettingType>());
        }

        [Test]
        public void GetCustomerSetting_ExecuteGetCustomerSettingTypeMethodOnSettingDataAccess()
        {
            //// Arrange
            QuerySettingParams querySettingParams = FakeObjectManager.CreateQuerySettingParams();
            var productSettingTypes = FakeObjectManager.CreateProduct().SettingTypes;
            _validationService.ValidateQuerySetting(querySettingParams).Returns(Result.Success);
            _productService.GetProductSettingTypes(querySettingParams.ProductId).Returns(productSettingTypes);
            SetupSettingTypeDataAccessStubForQuery(querySettingParams);

            //// Act
            _settingService.GetCustomerSetting(querySettingParams);

            //// Assert
            _settingDataAccess.Received(productSettingTypes.Count()).GetCustomerSettingType(Arg.Any<User>(), querySettingParams);
        }

        #endregion Get Customer Setting Tests

        #region Update Customer Setting Tests

        [Test]
        public void UpdateCustomerSetting_UpdatingSettingParamsIsNull_ThrowArgumentNullException()
        {
            //// Arrange
            UpdatingSettingParams updatingParams = null;

            //// Act
            Assert.Throws<ArgumentNullException>(
                new TestDelegate(
                    () => _settingService.UpdateCustomerSetting(updatingParams)));
        }

        [Test]
        public void UpdateCustomerSetting_SettingIsNull_ThrowArgumentNullException()
        {
            //// Arrange
            UpdatingSettingParams updatingParams = FakeObjectManager.CreateUpdatingSettingParams();
            updatingParams.Setting = null;

            //// Act
            Assert.Throws<ArgumentNullException>(
                new TestDelegate(
                    () => _settingService.UpdateCustomerSetting(updatingParams)));
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void UpdateCustomerSetting_CustomerIdIsNegativeOrZero_ThrowArgumentOutOfRangeException(int customerId)
        {
            //// Arrange
            UpdatingSettingParams updatingParams = FakeObjectManager.CreateUpdatingSettingParams();
            updatingParams.Setting.Customer.Id = customerId;

            //// Act
            Assert.Throws<ArgumentOutOfRangeException>(
                new TestDelegate(
                    () => _settingService.UpdateCustomerSetting(updatingParams)));
        }

        [Test]
        public void UpdateCustomerSetting_SettingTypesIsEmpty_ThrowArgumentOutOfRangeException()
        {
            //// Arrange
            UpdatingSettingParams updatingParams = FakeObjectManager.CreateUpdatingSettingParams();
            updatingParams.Setting.SettingTypes = new List<SettingType>();

            //// Act
            Assert.Throws<ArgumentOutOfRangeException>(
                new TestDelegate(
                    () => _settingService.UpdateCustomerSetting(updatingParams)));
        }

        [Test]
        public void UpdateCustomerSetting_ExecuteValidateUpdatingSettingMethodOnValidationService()
        {
            //// Arrange
            UpdatingSettingParams updatingParams = FakeObjectManager.CreateUpdatingSettingParams();

            //// Act
            _settingService.UpdateCustomerSetting(updatingParams);

            //// Assert
            _validationService.Received().ValidateUpdatingSetting(Arg.Is<UpdatingSettingParams>(
                actualSettingCommand => actualSettingCommand == updatingParams));
        }

        [Test]
        public void UpdateCustomerSetting_ExecuteResolveSettingPreTasksMethodOnSettingResolver()
        {
            //// Arrange
            UpdatingSettingParams updatingParams = FakeObjectManager.CreateUpdatingSettingParams();
            _validationService.ValidateUpdatingSetting(updatingParams).Returns(Result.Success);
            SetupSettingTypeDataAccessStubForCommand(updatingParams);

            //// Act
            _settingService.UpdateCustomerSetting(updatingParams);

            //// Assert
            _settingResolver.Received(updatingParams.Setting.SettingTypes.Count()).ResolveSettingPreTasks(Arg.Any<SettingType>());
        }

        private void SetupSettingTypeDataAccessStubForCommand(UpdatingSettingParams updatingParams)
        {
            var customerSettingType = FakeObjectManager.CreateSettingType(1);
            _settingDataAccess.UpdateCustomerSettingType(
                Arg.Any<ISettingContext>(),
                Arg.Any<Customer>(),
                Arg.Any<SettingType>(),
                Arg.Any<CustomerLevel?>()).Returns(Result.Success);
            _settingResolver.ResolveSettingDataAccess(Arg.Any<SettingType>()).Returns(_settingDataAccess);
        }

        [Test]
        public void UpdateCustomerSetting_ExecuteResolveSettingPostTasksMethodOnSettingResolver()
        {
            //// Arrange
            UpdatingSettingParams updatingParams = FakeObjectManager.CreateUpdatingSettingParams();
            _validationService.ValidateUpdatingSetting(updatingParams).Returns(Result.Success);
            SetupSettingTypeDataAccessStubForCommand(updatingParams);

            //// Act
            _settingService.UpdateCustomerSetting(updatingParams);

            //// Assert
            _settingResolver.Received(updatingParams.Setting.SettingTypes.Count()).ResolveSettingPostTasks(Arg.Any<SettingType>());
        }

        [Test]
        public void UpdateCustomerSetting_ExecuteUpdateCustomerSettingTypeOnSettingDataAccess()
        {
            //// Arrange
            UpdatingSettingParams updatingParams = FakeObjectManager.CreateUpdatingSettingParams();
            _validationService.ValidateUpdatingSetting(updatingParams).Returns(Result.Success);
            SetupSettingTypeDataAccessStubForCommand(updatingParams);

            //// Act
            _settingService.UpdateCustomerSetting(updatingParams);

            //// Assert
            _settingDataAccess.Received(updatingParams.Setting.SettingTypes.Count()).UpdateCustomerSettingType(
                Arg.Any<ISettingContext>(),
                Arg.Any<Customer>(),
                Arg.Any<SettingType>(),
                Arg.Any<CustomerLevel?>());
        }

        #endregion Update Customer Setting Tests

        #region Update Multiple Customer Setting Tests

        [Test]
        public void UpdateMultipleCustomerSetting_UpdateMultipleSettingParamsIsNull_ThrowArgumentNullException()
        {
            //// Arrange
            UpdatingMultipleSettingParams updatingMultipleSettingParams = null;

            //// Act
            Assert.Throws<ArgumentNullException>(
                new TestDelegate(
                    () => _settingService.UpdateMultipleCustomerSetting(updatingMultipleSettingParams)));
        }

        [Test]
        public void UpdateMultipleCustomerSetting_SettingIsNull_ThrowArgumentNullException()
        {
            //// Arrange
            UpdatingMultipleSettingParams updatingMultipleSettingParams = FakeObjectManager.CreateUpdatingMultipleSettingParams();
            updatingMultipleSettingParams.Setting = null;

            //// Act
            Assert.Throws<ArgumentNullException>(
                new TestDelegate(
                    () => _settingService.UpdateMultipleCustomerSetting(updatingMultipleSettingParams)));
        }

        [Test]
        public void UpdateMultipleCustomerSetting_NumberOfCustomerIsZero_ThrowArgumentOutOfRangeException()
        {
            //// Arrange
            UpdatingMultipleSettingParams updatingMultipleSettingParams = FakeObjectManager.CreateUpdatingMultipleSettingParams();
            updatingMultipleSettingParams.Customers = new List<Customer>();

            //// Act
            Assert.Throws<ArgumentOutOfRangeException>(
                new TestDelegate(
                    () => _settingService.UpdateMultipleCustomerSetting(updatingMultipleSettingParams)));
        }

        [Test]
        public void UpdateMultipleCustomerSetting_NumberOfSettingTypesIsZero_ThrowArgumentOutOfRangeException()
        {
            //// Arrange
            UpdatingMultipleSettingParams updatingMultipleSettingParams = FakeObjectManager.CreateUpdatingMultipleSettingParams();
            updatingMultipleSettingParams.Setting.SettingTypes = new List<SettingType>();

            //// Act
            Assert.Throws<ArgumentOutOfRangeException>(
                new TestDelegate(
                    () => _settingService.UpdateMultipleCustomerSetting(updatingMultipleSettingParams)));
        }

        [Test]
        public void UpdateMultipleCustomerSetting_ExecuteValidateUpdatingMultipleSettingMethodOnValidationService()
        {
            //// Arrange
            UpdatingMultipleSettingParams updatingMultipleSettingParams = FakeObjectManager.CreateUpdatingMultipleSettingParams();

            //// Act
            _settingService.UpdateMultipleCustomerSetting(updatingMultipleSettingParams);

            //// Assert
            _validationService.Received().ValidateUpdatingMultipleSetting(updatingMultipleSettingParams);
        }

        [Test]
        public void UpdateMultipleCustomerSetting_ExecuteResolveSettingPreTasksMethodOnSettingResolver()
        {
            //// Arrange
            UpdatingMultipleSettingParams updatingMultipleSettingParams = FakeObjectManager.CreateUpdatingMultipleSettingParams();
            _validationService.ValidateUpdatingMultipleSetting(updatingMultipleSettingParams).Returns(Result.Success);
            SetupSettingTypeDataAccessStubForCommand(updatingMultipleSettingParams);

            //// Act
            _settingService.UpdateMultipleCustomerSetting(updatingMultipleSettingParams);

            //// Assert
            var numOfSettingTypes = updatingMultipleSettingParams.Setting.SettingTypes.Count();
            var numOfSettingTypesEachCustomer = numOfSettingTypes * updatingMultipleSettingParams.Customers.Count();
            var numOfResolveSettingPreTasksMethodExecution = numOfSettingTypes + numOfSettingTypesEachCustomer;
            _settingResolver.Received(numOfResolveSettingPreTasksMethodExecution).ResolveSettingPreTasks(Arg.Any<SettingType>());
        }

        [Test]
        public void UpdateMultipleCustomerSetting_ExecuteResolveSettingPostTasksMethodOnSettingResolver()
        {
            //// Arrange
            UpdatingMultipleSettingParams updatingMultipleSettingParams = FakeObjectManager.CreateUpdatingMultipleSettingParams();
            _validationService.ValidateUpdatingMultipleSetting(updatingMultipleSettingParams).Returns(Result.Success);
            SetupSettingTypeDataAccessStubForCommand(updatingMultipleSettingParams);

            //// Act
            _settingService.UpdateMultipleCustomerSetting(updatingMultipleSettingParams);

            //// Assert
            var numOfSettingTypes = updatingMultipleSettingParams.Setting.SettingTypes.Count();
            var numOfSettingTypesEachCustomer = numOfSettingTypes * updatingMultipleSettingParams.Customers.Count();
            var numOfExecuteResultSettingPostTasksMethod = numOfSettingTypes + numOfSettingTypesEachCustomer;
            _settingResolver.Received(numOfExecuteResultSettingPostTasksMethod).ResolveSettingPostTasks(Arg.Any<SettingType>());
        }

        [Test]
        public void UpdateMultipleCustomerSetting_ExecuteUpdateCustomerSettingTypeOnSettingDataAccess()
        {
            //// Arrange
            UpdatingMultipleSettingParams updatingMultipleSettingParams = FakeObjectManager.CreateUpdatingMultipleSettingParams();
            _validationService.ValidateUpdatingMultipleSetting(updatingMultipleSettingParams).Returns(Result.Success);
            SetupSettingTypeDataAccessStubForCommand(updatingMultipleSettingParams);

            //// Act
            _settingService.UpdateMultipleCustomerSetting(updatingMultipleSettingParams);

            //// Assert
            var numOfSettingTypes = updatingMultipleSettingParams.Setting.SettingTypes.Count();
            var numOfSettingTypesEachCustomer = numOfSettingTypes * updatingMultipleSettingParams.Customers.Count();
            _settingDataAccess.Received(numOfSettingTypesEachCustomer).UpdateCustomerSettingType(
                Arg.Any<ISettingContext>(),
                Arg.Any<Customer>(),
                Arg.Any<SettingType>(),
                Arg.Any<CustomerLevel?>());
        }

        #endregion Update Multiple Customer Setting Tests
    }
}