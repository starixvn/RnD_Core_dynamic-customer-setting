namespace Fanex.CustomerSetting.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.CustomerSetting.Criteria.Customer;
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
        private ISettingDataAccessFactory _settingDataAccessFactory;
        private ISettingDataAccess _settingDataAccess;
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
            _settingDataAccessFactory = Substitute.For<ISettingDataAccessFactory>();
            _settingDataAccess = Substitute.For<ISettingDataAccess>();
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
        public void GetCustomerSetting_SettingQueryIsNull_ThrowArgumentNullException()
        {
            //// Arrange
            SettingQuery settingQuery = null;

            //// Act
            Assert.Throws<ArgumentNullException>(
                new TestDelegate(
                    () => _settingService.GetCustomerSetting(settingQuery)));
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void GetCustomerSetting_CustomerIdIsNegativeOrZero_ThrowArgumentOutOfRangeException(int customerId)
        {
            //// Arrange
            SettingQuery settingQuery = new SettingQuery
            {
                CustomerId = customerId
            };

            //// Act
            Assert.Throws<ArgumentOutOfRangeException>(
                new TestDelegate(
                    () => _settingService.GetCustomerSetting(settingQuery)));
        }

        [Test]
        public void GetCustomerSetting_ProductIdIsZero_ThrowArgumentOutOfRangeException()
        {
            //// Arrange
            SettingQuery settingQuery = FakeObjectManager.CreateSettingQueryFakeObject();
            settingQuery.ProductId = 0;

            //// Act
            Assert.Throws<ArgumentOutOfRangeException>(
                new TestDelegate(
                    () => _settingService.GetCustomerSetting(settingQuery)));
        }

        [Test]
        public void GetCustomerSetting_CustomerLevelValueIsZero_ThrowArgumentOutOfRangeException()
        {
            //// Arrange
            SettingQuery settingQuery = FakeObjectManager.CreateSettingQueryFakeObject();
            settingQuery.CustomerLevel = (CustomerLevel)0;

            //// Act
            Assert.Throws<ArgumentOutOfRangeException>(
                new TestDelegate(
                    () => _settingService.GetCustomerSetting(settingQuery)));
        }

        [Test]
        public void GetCustomerSetting_ExecuteValidateSettingQueryMethodOnValidationService()
        {
            //// Arrange
            SettingQuery settingQuery = FakeObjectManager.CreateSettingQueryFakeObject();
            _validationService.ValidateSettingQuery(Arg.Any<SettingQuery>()).Returns(Result.Success);

            //// Act
            _settingService.GetCustomerSetting(settingQuery);

            //// Assert
            _validationService.Received().ValidateSettingQuery(Arg.Is<SettingQuery>(
                actualSettingQuery => actualSettingQuery.CustomerId == settingQuery.CustomerId
                    && actualSettingQuery.CustomerLevel == settingQuery.CustomerLevel
                    && actualSettingQuery.ProductId == settingQuery.ProductId
                    && actualSettingQuery.UplineLevel == settingQuery.UplineLevel));
        }

        [Test]
        public void GetCustomerSetting_QuerySettingIsNotValid_ThrowInvalidOperationException()
        {
            //// Arrange
            SettingQuery settingQuery = FakeObjectManager.CreateSettingQueryFakeObject();
            _validationService.ValidateSettingQuery(Arg.Any<SettingQuery>()).Returns(Result.AccessDenied);

            //// Act
            Assert.Throws<InvalidOperationException>(
                new TestDelegate(
                    () => _settingService.GetCustomerSetting(settingQuery)));
        }

        [Test]
        public void GetCustomerSetting_ExecuteGetCustomerMethodOnDynamicRepository()
        {
            //// Arrange
            SettingQuery settingQuery = FakeObjectManager.CreateSettingQueryFakeObject();
            _validationService.ValidateSettingQuery(Arg.Any<SettingQuery>()).Returns(Result.Success);

            //// Act
            _settingService.GetCustomerSetting(settingQuery);

            //// Assert
            _dymanicRepository.Received().Get<Customer>(Arg.Is<CustomerQueryCriteria>(
                    criteria => criteria.CustId == settingQuery.CustomerId));
        }

        [Test]
        public void GetCustomerSetting_ExecuteGetProductSettingTypesMethodOnProductService()
        {
            //// Arrange
            SettingQuery settingQuery = FakeObjectManager.CreateSettingQueryFakeObject();
            _validationService.ValidateSettingQuery(Arg.Any<SettingQuery>()).Returns(Result.Success);

            //// Act
            _settingService.GetCustomerSetting(settingQuery);

            //// Assert
            _productService.Received().GetProductSettingTypes(Arg.Is<byte>(
                    productId => productId == settingQuery.ProductId));
        }

        [Test]
        public void GetCustomerSetting_ExecuteResolveSettingPreTasksMethodOnSettingResolver()
        {
            //// Arrange
            SettingQuery settingQuery = FakeObjectManager.CreateSettingQueryFakeObject();
            var productSettingTypes = FakeObjectManager.CreateProductFakeObject().SettingTypes;
            _validationService.ValidateSettingQuery(settingQuery).Returns(Result.Success);
            _productService.GetProductSettingTypes(settingQuery.ProductId).Returns(productSettingTypes);
            SetupSettingTypeDataAccessStubForQuery(settingQuery);

            //// Act
            _settingService.GetCustomerSetting(settingQuery);

            //// Assert
            _settingResolver.Received(productSettingTypes.Count()).ResolveSettingPreTasks(Arg.Any<SettingType>());
        }

        private void SetupSettingTypeDataAccessStubForQuery(SettingQuery settingQuery)
        {
            var customerSettingType = FakeObjectManager.CreateSettingTypeFakeObject(1);
            _settingDataAccess.GetCustomerSettingType(Arg.Any<User>(), settingQuery).Returns(customerSettingType);
            _settingDataAccessFactory.Create(Arg.Any<CustomerLevel>()).Returns(_settingDataAccess);
            _settingResolver.ResolveSettingDataAccessFactory(Arg.Any<SettingType>()).Returns(_settingDataAccessFactory);
        }

        [Test]
        public void GetCustomerSetting_ExecuteResolveSettingPostTasksMethodOnSettingResolver()
        {
            //// Arrange
            SettingQuery settingQuery = FakeObjectManager.CreateSettingQueryFakeObject();
            var productSettingTypes = FakeObjectManager.CreateProductFakeObject().SettingTypes;
            _validationService.ValidateSettingQuery(settingQuery).Returns(Result.Success);
            _productService.GetProductSettingTypes(settingQuery.ProductId).Returns(productSettingTypes);
            SetupSettingTypeDataAccessStubForQuery(settingQuery);

            //// Act
            _settingService.GetCustomerSetting(settingQuery);

            //// Assert
            _settingResolver.Received(productSettingTypes.Count()).ResolveSettingPostTasks(Arg.Any<SettingType>());
        }

        [Test]
        public void GetCustomerSetting_ExecuteGetCustomerSettingTypeMethodOnSettingDataAccess()
        {
            //// Arrange
            SettingQuery settingQuery = FakeObjectManager.CreateSettingQueryFakeObject();
            var productSettingTypes = FakeObjectManager.CreateProductFakeObject().SettingTypes;
            _validationService.ValidateSettingQuery(settingQuery).Returns(Result.Success);
            _productService.GetProductSettingTypes(settingQuery.ProductId).Returns(productSettingTypes);
            SetupSettingTypeDataAccessStubForQuery(settingQuery);

            //// Act
            _settingService.GetCustomerSetting(settingQuery);

            //// Assert
            _settingDataAccess.Received(productSettingTypes.Count()).GetCustomerSettingType(Arg.Any<User>(), settingQuery);
        }

        #endregion Get Customer Setting Tests

        #region Update Customer Setting Tests

        [Test]
        public void UpdateCustomerSetting_SettingCommandIsNull_ThrowArgumentNullException()
        {
            //// Arrange
            SettingCommand settingCommand = null;

            //// Act
            Assert.Throws<ArgumentNullException>(
                new TestDelegate(
                    () => _settingService.UpdateCustomerSetting(settingCommand)));
        }

        [Test]
        public void UpdateCustomerSetting_SettingIsNull_ThrowArgumentNullException()
        {
            //// Arrange
            SettingCommand settingCommand = FakeObjectManager.CreateSettingCommandFakeObject();
            settingCommand.Setting = null;

            //// Act
            Assert.Throws<ArgumentNullException>(
                new TestDelegate(
                    () => _settingService.UpdateCustomerSetting(settingCommand)));
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void UpdateCustomerSetting_CustomerIdIsNegativeOrZero_ThrowArgumentOutOfRangeException(int customerId)
        {
            //// Arrange
            SettingCommand settingCommand = FakeObjectManager.CreateSettingCommandFakeObject();
            settingCommand.Setting.Customer.Id = customerId;

            //// Act
            Assert.Throws<ArgumentOutOfRangeException>(
                new TestDelegate(
                    () => _settingService.UpdateCustomerSetting(settingCommand)));
        }

        [Test]
        public void UpdateCustomerSetting_SettingTypesIsEmpty_ThrowArgumentOutOfRangeException()
        {
            //// Arrange
            SettingCommand settingCommand = FakeObjectManager.CreateSettingCommandFakeObject();
            settingCommand.Setting.SettingTypes = new List<SettingType>();

            //// Act
            Assert.Throws<ArgumentOutOfRangeException>(
                new TestDelegate(
                    () => _settingService.UpdateCustomerSetting(settingCommand)));
        }

        [Test]
        public void UpdateCustomerSetting_ExecuteValidateSettingCommandMethodOnValidationService()
        {
            //// Arrange
            SettingCommand settingCommand = FakeObjectManager.CreateSettingCommandFakeObject();

            //// Act
            _settingService.UpdateCustomerSetting(settingCommand);

            //// Assert
            _validationService.Received().ValidateSettingCommand(Arg.Is<SettingCommand>(
                actualSettingCommand => actualSettingCommand == settingCommand));
        }

        [Test]
        public void UpdateCustomerSetting_ExecuteResolveSettingPreTasksMethodOnSettingResolver()
        {
            //// Arrange
            SettingCommand settingCommand = FakeObjectManager.CreateSettingCommandFakeObject();
            _validationService.ValidateSettingCommand(settingCommand).Returns(Result.Success);
            SetupSettingTypeDataAccessStubForCommand(settingCommand);

            //// Act
            _settingService.UpdateCustomerSetting(settingCommand);

            //// Assert
            _settingResolver.Received(settingCommand.Setting.SettingTypes.Count()).ResolveSettingPreTasks(Arg.Any<SettingType>());
        }

        private void SetupSettingTypeDataAccessStubForCommand(SettingCommand settingCommand)
        {
            var customerSettingType = FakeObjectManager.CreateSettingTypeFakeObject(1);
            _settingDataAccess.UpdateCustomerSetting(
                Arg.Any<ISettingContext>(),
                Arg.Any<Customer>(),
                Arg.Any<SettingType>(),
                Arg.Any<CustomerLevel?>()).Returns(Result.Success);
            _settingDataAccessFactory.Create(Arg.Any<CustomerLevel>()).Returns(_settingDataAccess);
            _settingResolver.ResolveSettingDataAccessFactory(Arg.Any<SettingType>()).Returns(_settingDataAccessFactory);
        }

        [Test]
        public void UpdateCustomerSetting_ExecuteResolveSettingPostTasksMethodOnSettingResolver()
        {
            //// Arrange
            SettingCommand settingCommand = FakeObjectManager.CreateSettingCommandFakeObject();
            _validationService.ValidateSettingCommand(settingCommand).Returns(Result.Success);
            SetupSettingTypeDataAccessStubForCommand(settingCommand);

            //// Act
            _settingService.UpdateCustomerSetting(settingCommand);

            //// Assert
            _settingResolver.Received(settingCommand.Setting.SettingTypes.Count()).ResolveSettingPostTasks(Arg.Any<SettingType>());
        }

        [Test]
        public void UpdateCustomerSetting_ExecuteUpdateSettingTypeOnSettingDataAccess()
        {
            //// Arrange
            SettingCommand settingCommand = FakeObjectManager.CreateSettingCommandFakeObject();
            _validationService.ValidateSettingCommand(settingCommand).Returns(Result.Success);
            SetupSettingTypeDataAccessStubForCommand(settingCommand);

            //// Act
            _settingService.UpdateCustomerSetting(settingCommand);

            //// Assert
            _settingDataAccess.Received(settingCommand.Setting.SettingTypes.Count()).UpdateCustomerSetting(
                Arg.Any<ISettingContext>(),
                Arg.Any<Customer>(),
                Arg.Any<SettingType>(),
                Arg.Any<CustomerLevel?>());
        }

        #endregion Update Customer Setting Tests

        #region Update Multiple Customer Setting Tests

        [Test]
        public void UpdateMultipleCustomerSetting_MultipleSettingCommandIsNull_ThrowArgumentNullException()
        {
            //// Arrange
            MultipleSettingCommand multipleSettingCommand = null;

            //// Act
            Assert.Throws<ArgumentNullException>(
                new TestDelegate(
                    () => _settingService.UpdateMultipleCustomerSetting(multipleSettingCommand)));
        }

        [Test]
        public void UpdateMultipleCustomerSetting_SettingIsNull_ThrowArgumentNullException()
        {
            //// Arrange
            MultipleSettingCommand multipleSettingCommand = FakeObjectManager.CreateMultipleSettingCommandFakeObject();
            multipleSettingCommand.Setting = null;

            //// Act
            Assert.Throws<ArgumentNullException>(
                new TestDelegate(
                    () => _settingService.UpdateMultipleCustomerSetting(multipleSettingCommand)));
        }

        [Test]
        public void UpdateMultipleCustomerSetting_NumberOfCustomerIsZero_ThrowArgumentOutOfRangeException()
        {
            //// Arrange
            MultipleSettingCommand multipleSettingCommand = FakeObjectManager.CreateMultipleSettingCommandFakeObject();
            multipleSettingCommand.Customers = new List<Customer>();

            //// Act
            Assert.Throws<ArgumentOutOfRangeException>(
                new TestDelegate(
                    () => _settingService.UpdateMultipleCustomerSetting(multipleSettingCommand)));
        }

        [Test]
        public void UpdateMultipleCustomerSetting_NumberOfSettingTypesIsZero_ThrowArgumentOutOfRangeException()
        {
            //// Arrange
            MultipleSettingCommand multipleSettingCommand = FakeObjectManager.CreateMultipleSettingCommandFakeObject();
            multipleSettingCommand.Setting.SettingTypes = new List<SettingType>();

            //// Act
            Assert.Throws<ArgumentOutOfRangeException>(
                new TestDelegate(
                    () => _settingService.UpdateMultipleCustomerSetting(multipleSettingCommand)));
        }

        [Test]
        public void UpdateMultipleCustomerSetting_ExecuteValidateMultipleSettingCommandMethodOnValidationService()
        {
            //// Arrange
            MultipleSettingCommand multipleSettingCommand = FakeObjectManager.CreateMultipleSettingCommandFakeObject();

            //// Act
            _settingService.UpdateMultipleCustomerSetting(multipleSettingCommand);

            //// Assert
            _validationService.Received().ValidateMultipleSettingCommand(multipleSettingCommand);
        }

        [Test]
        public void UpdateMultipleCustomerSetting_ExecuteResolveSettingPreTasksMethodOnSettingResolver()
        {
            //// Arrange
            MultipleSettingCommand multipleSettingCommand = FakeObjectManager.CreateMultipleSettingCommandFakeObject();
            _validationService.ValidateMultipleSettingCommand(multipleSettingCommand).Returns(Result.Success);
            SetupSettingTypeDataAccessStubForCommand(multipleSettingCommand);

            //// Act
            _settingService.UpdateMultipleCustomerSetting(multipleSettingCommand);

            //// Assert
            var numOfSettingTypes = multipleSettingCommand.Setting.SettingTypes.Count();
            var numOfSettingTypesEachCustomer = numOfSettingTypes * multipleSettingCommand.Customers.Count();
            var numOfResolveSettingPreTasksMethodExecution = numOfSettingTypes + numOfSettingTypesEachCustomer;
            _settingResolver.Received(numOfResolveSettingPreTasksMethodExecution).ResolveSettingPreTasks(Arg.Any<SettingType>());
        }

        [Test]
        public void UpdateMultipleCustomerSetting_ExecuteResolveSettingPostTasksMethodOnSettingResolver()
        {
            //// Arrange
            MultipleSettingCommand multipleSettingCommand = FakeObjectManager.CreateMultipleSettingCommandFakeObject();
            _validationService.ValidateMultipleSettingCommand(multipleSettingCommand).Returns(Result.Success);
            SetupSettingTypeDataAccessStubForCommand(multipleSettingCommand);

            //// Act
            _settingService.UpdateMultipleCustomerSetting(multipleSettingCommand);

            //// Assert
            var numOfSettingTypes = multipleSettingCommand.Setting.SettingTypes.Count();
            var numOfSettingTypesEachCustomer = numOfSettingTypes * multipleSettingCommand.Customers.Count();
            var numOfExecuteResultSettingPostTasksMethod = numOfSettingTypes + numOfSettingTypesEachCustomer;
            _settingResolver.Received(numOfExecuteResultSettingPostTasksMethod).ResolveSettingPostTasks(Arg.Any<SettingType>());
        }

        [Test]
        public void UpdateMultipleCustomerSetting_ExecuteUpdateSettingTypeOnSettingDataAccess()
        {
            //// Arrange
            MultipleSettingCommand multipleSettingCommand = FakeObjectManager.CreateMultipleSettingCommandFakeObject();
            _validationService.ValidateMultipleSettingCommand(multipleSettingCommand).Returns(Result.Success);
            SetupSettingTypeDataAccessStubForCommand(multipleSettingCommand);

            //// Act
            _settingService.UpdateMultipleCustomerSetting(multipleSettingCommand);

            //// Assert
            var numOfSettingTypes = multipleSettingCommand.Setting.SettingTypes.Count();
            var numOfSettingTypesEachCustomer = numOfSettingTypes * multipleSettingCommand.Customers.Count();
            _settingDataAccess.Received(numOfSettingTypesEachCustomer).UpdateCustomerSetting(
                Arg.Any<ISettingContext>(),
                Arg.Any<Customer>(),
                Arg.Any<SettingType>(),
                Arg.Any<CustomerLevel?>());
        }

        #endregion Update Multiple Customer Setting Tests
    }
}