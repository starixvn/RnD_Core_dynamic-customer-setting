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
    public class SettingServiceTests
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
            _logger = Substitute.For<ILogger>();

            _settingService = new SettingService(
                _settingContext,
                _validationService,
                _productService,
                _settingResolver,
                _dymanicRepository,
                _logger);
        }

        #region Get Customer Setting Tests

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
            QuerySettingParam querySettingParam = FakeObjectManager.CreateQuerySettingParam();
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
            QuerySettingParam querySettingParam = FakeObjectManager.CreateQuerySettingParam();
            querySettingParam.CustomerLevel = (CustomerLevel)0;

            //// Act
            Assert.Throws<ArgumentOutOfRangeException>(
                new TestDelegate(
                    () => _settingService.GetCustomerSetting(querySettingParam)));
        }

        [Test]
        public void GetCustomerSetting_ExecuteValidateQuerySettingMethodOnValidationService()
        {
            //// Arrange
            QuerySettingParam querySettingParam = FakeObjectManager.CreateQuerySettingParam();
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
        public void GetCustomerSetting_QuerySettingIsNotValid_ThrowInvalidOperationException()
        {
            //// Arrange
            QuerySettingParam querySettingParam = FakeObjectManager.CreateQuerySettingParam();
            _validationService.ValidateQuerySetting(Arg.Any<QuerySettingParam>()).Returns(Result.AccessDenied);

            //// Act
            Assert.Throws<InvalidOperationException>(
                new TestDelegate(
                    () => _settingService.GetCustomerSetting(querySettingParam)));
        }

        [Test]
        public void GetCustomerSetting_ExecuteGetCustomerMethodOnDynamicRepository()
        {
            //// Arrange
            QuerySettingParam querySettingParam = FakeObjectManager.CreateQuerySettingParam();
            _validationService.ValidateQuerySetting(Arg.Any<QuerySettingParam>()).Returns(Result.Success);

            //// Act
            _settingService.GetCustomerSetting(querySettingParam);

            //// Assert
            _dymanicRepository.Received().Get<Customer>(Arg.Is<CustomerQueryCriteria>(
                    criteria => criteria.CustId == querySettingParam.CustomerId));
        }

        [Test]
        public void GetCustomerSetting_ExecuteGetProductSettingTypesMethodOnProductService()
        {
            //// Arrange
            QuerySettingParam querySettingParam = FakeObjectManager.CreateQuerySettingParam();
            _validationService.ValidateQuerySetting(Arg.Any<QuerySettingParam>()).Returns(Result.Success);

            //// Act
            _settingService.GetCustomerSetting(querySettingParam);

            //// Assert
            _productService.Received().GetProductSettingTypes(Arg.Is<byte>(
                    productId => productId == querySettingParam.ProductId));
        }

        [Test]
        public void GetCustomerSetting_ExecuteResolveSettingPreTasksMethodOnSettingResolver()
        {
            //// Arrange
            QuerySettingParam querySettingParam = FakeObjectManager.CreateQuerySettingParam();
            var productSettingTypes = FakeObjectManager.CreateProduct().SettingTypes;
            _validationService.ValidateQuerySetting(querySettingParam).Returns(Result.Success);
            _productService.GetProductSettingTypes(querySettingParam.ProductId).Returns(productSettingTypes);
            SetupSettingTypeDataAccessStubForQuery(querySettingParam);

            //// Act
            _settingService.GetCustomerSetting(querySettingParam);

            //// Assert
            _settingResolver.Received(productSettingTypes.Count()).ResolveSettingPreTasks(Arg.Any<SettingType>());
        }

        private void SetupSettingTypeDataAccessStubForQuery(QuerySettingParam querySettingParam)
        {
            var customerSettingType = FakeObjectManager.CreateSettingType(1);
            _settingDataAccess.GetCustomerSettingType(Arg.Any<User>(), querySettingParam).Returns(customerSettingType);
            _settingResolver.ResolveSettingDataAccess(Arg.Any<SettingType>()).Returns(_settingDataAccess);
        }

        [Test]
        public void GetCustomerSetting_ExecuteResolveSettingPostTasksMethodOnSettingResolver()
        {
            //// Arrange
            QuerySettingParam querySettingParam = FakeObjectManager.CreateQuerySettingParam();
            var productSettingTypes = FakeObjectManager.CreateProduct().SettingTypes;
            _validationService.ValidateQuerySetting(querySettingParam).Returns(Result.Success);
            _productService.GetProductSettingTypes(querySettingParam.ProductId).Returns(productSettingTypes);
            SetupSettingTypeDataAccessStubForQuery(querySettingParam);

            //// Act
            _settingService.GetCustomerSetting(querySettingParam);

            //// Assert
            _settingResolver.Received(productSettingTypes.Count()).ResolveSettingPostTasks(Arg.Any<SettingType>());
        }

        [Test]
        public void GetCustomerSetting_ExecuteGetCustomerSettingTypeMethodOnSettingDataAccess()
        {
            //// Arrange
            QuerySettingParam querySettingParam = FakeObjectManager.CreateQuerySettingParam();
            var productSettingTypes = FakeObjectManager.CreateProduct().SettingTypes;
            _validationService.ValidateQuerySetting(querySettingParam).Returns(Result.Success);
            _productService.GetProductSettingTypes(querySettingParam.ProductId).Returns(productSettingTypes);
            SetupSettingTypeDataAccessStubForQuery(querySettingParam);

            //// Act
            _settingService.GetCustomerSetting(querySettingParam);

            //// Assert
            _settingDataAccess.Received(productSettingTypes.Count()).GetCustomerSettingType(Arg.Any<User>(), querySettingParam);
        }

        [Test]
        public void GetCustomerSetting_GotExceptionWhenResolveSettingDataAccess_ExecuteErrorOnLogger()
        {
            //// Arrange
            QuerySettingParam querySettingParam = FakeObjectManager.CreateQuerySettingParam();
            var productSettingTypes = FakeObjectManager.CreateProduct().SettingTypes;
            _validationService.ValidateQuerySetting(querySettingParam).Returns(Result.Success);
            _productService.GetProductSettingTypes(querySettingParam.ProductId).Returns(productSettingTypes);
            _settingResolver.ResolveSettingDataAccess(Arg.Any<SettingType>()).Returns(x => { throw new Exception(); });

            //// Act
            _settingService.GetCustomerSetting(querySettingParam);

            //// Assert
            _logger.Received().Error(Arg.Any<string>(), Arg.Any<Exception>());
        }

        #endregion Get Customer Setting Tests

        #region Update Customer Setting Tests

        [Test]
        public void UpdateCustomerSetting_UpdatingSettingParamIsNull_ThrowArgumentNullException()
        {
            //// Arrange
            UpdatingSettingParam updatingParam = null;

            //// Act
            Assert.Throws<ArgumentNullException>(
                new TestDelegate(
                    () => _settingService.UpdateCustomerSetting(updatingParam)));
        }

        [Test]
        public void UpdateCustomerSetting_SettingIsNull_ThrowArgumentNullException()
        {
            //// Arrange
            UpdatingSettingParam updatingParam = FakeObjectManager.CreateUpdatingSettingParam();
            updatingParam.Setting = null;

            //// Act
            Assert.Throws<ArgumentNullException>(
                new TestDelegate(
                    () => _settingService.UpdateCustomerSetting(updatingParam)));
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void UpdateCustomerSetting_CustomerIdIsNegativeOrZero_ThrowArgumentOutOfRangeException(int customerId)
        {
            //// Arrange
            UpdatingSettingParam updatingParam = FakeObjectManager.CreateUpdatingSettingParam();
            updatingParam.Setting.Customer.Id = customerId;

            //// Act
            Assert.Throws<ArgumentOutOfRangeException>(
                new TestDelegate(
                    () => _settingService.UpdateCustomerSetting(updatingParam)));
        }

        [Test]
        public void UpdateCustomerSetting_SettingTypesIsEmpty_ThrowArgumentOutOfRangeException()
        {
            //// Arrange
            UpdatingSettingParam updatingParam = FakeObjectManager.CreateUpdatingSettingParam();
            updatingParam.Setting.SettingTypes = new List<SettingType>();

            //// Act
            Assert.Throws<ArgumentOutOfRangeException>(
                new TestDelegate(
                    () => _settingService.UpdateCustomerSetting(updatingParam)));
        }

        [Test]
        public void UpdateCustomerSetting_ExecuteValidateUpdatingSettingMethodOnValidationService()
        {
            //// Arrange
            UpdatingSettingParam updatingParam = FakeObjectManager.CreateUpdatingSettingParam();

            //// Act
            _settingService.UpdateCustomerSetting(updatingParam);

            //// Assert
            _validationService.Received().ValidateUpdatingSetting(Arg.Is<UpdatingSettingParam>(
                actualSettingCommand => actualSettingCommand == updatingParam));
        }

        [Test]
        public void UpdateCustomerSetting_ExecuteResolveSettingPreTasksMethodOnSettingResolver()
        {
            //// Arrange
            UpdatingSettingParam updatingParam = FakeObjectManager.CreateUpdatingSettingParam();
            _validationService.ValidateUpdatingSetting(updatingParam).Returns(Result.Success);
            SetupSettingTypeDataAccessStubForCommand(updatingParam);

            //// Act
            _settingService.UpdateCustomerSetting(updatingParam);

            //// Assert
            _settingResolver.Received(updatingParam.Setting.SettingTypes.Count()).ResolveSettingPreTasks(Arg.Any<SettingType>());
        }

        private void SetupSettingTypeDataAccessStubForCommand(UpdatingSettingParam updatingParam)
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
            UpdatingSettingParam updatingParam = FakeObjectManager.CreateUpdatingSettingParam();
            _validationService.ValidateUpdatingSetting(updatingParam).Returns(Result.Success);
            SetupSettingTypeDataAccessStubForCommand(updatingParam);

            //// Act
            _settingService.UpdateCustomerSetting(updatingParam);

            //// Assert
            _settingResolver.Received(updatingParam.Setting.SettingTypes.Count()).ResolveSettingPostTasks(Arg.Any<SettingType>());
        }

        [Test]
        public void UpdateCustomerSetting_ExecuteUpdateCustomerSettingTypeOnSettingDataAccess()
        {
            //// Arrange
            UpdatingSettingParam updatingParam = FakeObjectManager.CreateUpdatingSettingParam();
            _validationService.ValidateUpdatingSetting(updatingParam).Returns(Result.Success);
            SetupSettingTypeDataAccessStubForCommand(updatingParam);

            //// Act
            _settingService.UpdateCustomerSetting(updatingParam);

            //// Assert
            _settingDataAccess.Received(updatingParam.Setting.SettingTypes.Count()).UpdateCustomerSettingType(
                Arg.Any<ISettingContext>(),
                Arg.Any<Customer>(),
                Arg.Any<SettingType>(),
                Arg.Any<CustomerLevel?>());
        }

        #endregion Update Customer Setting Tests

        #region Update Multiple Customer Setting Tests

        [Test]
        public void UpdateMultipleCustomerSetting_UpdateMultipleSettingParamIsNull_ThrowArgumentNullException()
        {
            //// Arrange
            UpdatingMultipleSettingParam updatingMultipleSettingParam = null;

            //// Act
            Assert.Throws<ArgumentNullException>(
                new TestDelegate(
                    () => _settingService.UpdateMultipleCustomerSetting(updatingMultipleSettingParam)));
        }

        [Test]
        public void UpdateMultipleCustomerSetting_SettingIsNull_ThrowArgumentNullException()
        {
            //// Arrange
            UpdatingMultipleSettingParam updatingMultipleSettingParam = FakeObjectManager.CreateUpdatingMultipleSettingParam();
            updatingMultipleSettingParam.Setting = null;

            //// Act
            Assert.Throws<ArgumentNullException>(
                new TestDelegate(
                    () => _settingService.UpdateMultipleCustomerSetting(updatingMultipleSettingParam)));
        }

        [Test]
        public void UpdateMultipleCustomerSetting_NumberOfCustomerIsZero_ThrowArgumentOutOfRangeException()
        {
            //// Arrange
            UpdatingMultipleSettingParam updatingMultipleSettingParam = FakeObjectManager.CreateUpdatingMultipleSettingParam();
            updatingMultipleSettingParam.Customers = new List<Customer>();

            //// Act
            Assert.Throws<ArgumentOutOfRangeException>(
                new TestDelegate(
                    () => _settingService.UpdateMultipleCustomerSetting(updatingMultipleSettingParam)));
        }

        [Test]
        public void UpdateMultipleCustomerSetting_NumberOfSettingTypesIsZero_ThrowArgumentOutOfRangeException()
        {
            //// Arrange
            UpdatingMultipleSettingParam updatingMultipleSettingParam = FakeObjectManager.CreateUpdatingMultipleSettingParam();
            updatingMultipleSettingParam.Setting.SettingTypes = new List<SettingType>();

            //// Act
            Assert.Throws<ArgumentOutOfRangeException>(
                new TestDelegate(
                    () => _settingService.UpdateMultipleCustomerSetting(updatingMultipleSettingParam)));
        }

        [Test]
        public void UpdateMultipleCustomerSetting_ExecuteValidateUpdatingMultipleSettingMethodOnValidationService()
        {
            //// Arrange
            UpdatingMultipleSettingParam updatingMultipleSettingParam = FakeObjectManager.CreateUpdatingMultipleSettingParam();

            //// Act
            _settingService.UpdateMultipleCustomerSetting(updatingMultipleSettingParam);

            //// Assert
            _validationService.Received().ValidateUpdatingMultipleSetting(updatingMultipleSettingParam);
        }

        [Test]
        public void UpdateMultipleCustomerSetting_ExecuteResolveSettingPreTasksMethodOnSettingResolver()
        {
            //// Arrange
            UpdatingMultipleSettingParam updatingMultipleSettingParam = FakeObjectManager.CreateUpdatingMultipleSettingParam();
            _validationService.ValidateUpdatingMultipleSetting(updatingMultipleSettingParam).Returns(Result.Success);
            SetupSettingTypeDataAccessStubForCommand(updatingMultipleSettingParam);

            //// Act
            _settingService.UpdateMultipleCustomerSetting(updatingMultipleSettingParam);

            //// Assert
            var numOfSettingTypes = updatingMultipleSettingParam.Setting.SettingTypes.Count();
            var numOfSettingTypesEachCustomer = numOfSettingTypes * updatingMultipleSettingParam.Customers.Count();
            var numOfResolveSettingPreTasksMethodExecution = numOfSettingTypes + numOfSettingTypesEachCustomer;
            _settingResolver.Received(numOfResolveSettingPreTasksMethodExecution).ResolveSettingPreTasks(Arg.Any<SettingType>());
        }

        [Test]
        public void UpdateMultipleCustomerSetting_ExecuteResolveSettingPostTasksMethodOnSettingResolver()
        {
            //// Arrange
            UpdatingMultipleSettingParam updatingMultipleSettingParam = FakeObjectManager.CreateUpdatingMultipleSettingParam();
            _validationService.ValidateUpdatingMultipleSetting(updatingMultipleSettingParam).Returns(Result.Success);
            SetupSettingTypeDataAccessStubForCommand(updatingMultipleSettingParam);

            //// Act
            _settingService.UpdateMultipleCustomerSetting(updatingMultipleSettingParam);

            //// Assert
            var numOfSettingTypes = updatingMultipleSettingParam.Setting.SettingTypes.Count();
            var numOfSettingTypesEachCustomer = numOfSettingTypes * updatingMultipleSettingParam.Customers.Count();
            var numOfExecuteResultSettingPostTasksMethod = numOfSettingTypes + numOfSettingTypesEachCustomer;
            _settingResolver.Received(numOfExecuteResultSettingPostTasksMethod).ResolveSettingPostTasks(Arg.Any<SettingType>());
        }

        [Test]
        public void UpdateMultipleCustomerSetting_ExecuteUpdateCustomerSettingTypeOnSettingDataAccess()
        {
            //// Arrange
            UpdatingMultipleSettingParam updatingMultipleSettingParam = FakeObjectManager.CreateUpdatingMultipleSettingParam();
            _validationService.ValidateUpdatingMultipleSetting(updatingMultipleSettingParam).Returns(Result.Success);
            SetupSettingTypeDataAccessStubForCommand(updatingMultipleSettingParam);

            //// Act
            _settingService.UpdateMultipleCustomerSetting(updatingMultipleSettingParam);

            //// Assert
            var numOfSettingTypes = updatingMultipleSettingParam.Setting.SettingTypes.Count();
            var numOfSettingTypesEachCustomer = numOfSettingTypes * updatingMultipleSettingParam.Customers.Count();
            _settingDataAccess.Received(numOfSettingTypesEachCustomer).UpdateCustomerSettingType(
                Arg.Any<ISettingContext>(),
                Arg.Any<Customer>(),
                Arg.Any<SettingType>(),
                Arg.Any<CustomerLevel?>());
        }

        #endregion Update Multiple Customer Setting Tests
    }
}