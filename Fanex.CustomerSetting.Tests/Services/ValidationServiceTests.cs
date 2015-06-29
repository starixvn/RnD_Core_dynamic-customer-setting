namespace Fanex.CustomerSetting.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.CustomerSetting.DataAccesses;
    using Fanex.CustomerSetting.Dtos.Params;
    using Fanex.CustomerSetting.Dtos.Results;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Services;
    using Fanex.CustomerSetting.Services.Impl;
    using Fanex.CustomerSetting.Tests.Models;
    using Fanex.Logging;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class ValidationServiceTests
    {
        private ISettingContext _settingContext;
        private IValidationService _validationService;
        private IValidationDataAccess _validationDataAccess;
        private ILogger _logger;
        private IProductService _productService;

        [SetUp]
        public void Setup()
        {
            _validationDataAccess = Substitute.For<IValidationDataAccess>();
            _settingContext = new SettingContextStub();
            _productService = Substitute.For<IProductService>();
            _logger = Substitute.For<ILogger>();

            _validationService = new ValidationService(_settingContext, _validationDataAccess, _productService, _logger);
        }

        #region Validatie Setting Query Tests

        [Test]
        public void ValidateQuerySetting_QuerySettingParamsIsNull_ThrowArgumentNullException()
        {
            //// Arrange
            QuerySettingParams queryParams = null;

            //// Act
            Assert.Throws<ArgumentNullException>(
                new TestDelegate(
                    () => _validationService.ValidateQuerySetting(queryParams)));
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void ValidateQuerySetting_CustomerIdIsNegativeOrZero_ThrowArgumentOutOfRangeException(int customerId)
        {
            //// Arrange
            QuerySettingParams queryParams = new QuerySettingParams
            {
                CustomerId = customerId
            };

            //// Act
            Assert.Throws<ArgumentOutOfRangeException>(
                new TestDelegate(
                    () => _validationService.ValidateQuerySetting(queryParams)));
        }

        [TestCase(CustomerLevel.Super)]
        [TestCase(CustomerLevel.Master)]
        [TestCase(CustomerLevel.Agent)]
        [TestCase(CustomerLevel.Member)]
        public void ValidateQuerySetting_ActorIsCompany_CanViewAllCustomerSetting(CustomerLevel customerLevel)
        {
            //// Arrange
            QuerySettingParams queryParams = new QuerySettingParams
            {
                CustomerId = 1,
                CustomerLevel = customerLevel
            };
            _settingContext.Actor.Level = UserLevel.Company;
            _validationDataAccess.ValidatePermission(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<byte>())
                .Returns(Result.Success);

            //// Act
            var actualResult = _validationService.ValidateQuerySetting(queryParams);

            //// Assert
            Assert.IsTrue(actualResult.IsSuccessful);
        }

        [TestCase(CustomerLevel.Super)]
        [TestCase(CustomerLevel.Agent)]
        public void ValidateQuerySetting_ActorIsSuper_CannotViewOtherSuperAndAgentSetting(CustomerLevel customerLevel)
        {
            //// Arrange
            QuerySettingParams queryParams = new QuerySettingParams
            {
                CustomerId = 1,
                CustomerLevel = customerLevel
            };
            _settingContext.Actor.Level = UserLevel.Super;

            //// Act
            var actualResult = _validationService.ValidateQuerySetting(queryParams);

            //// Assert
            Assert.IsFalse(actualResult.IsSuccessful);
        }

        [TestCase(CustomerLevel.Master)]
        [TestCase(CustomerLevel.Member)]
        public void ValidateQuerySetting_ActorIsSuper_CanViewDownlineSettingExceptAgent(CustomerLevel downlineLevel)
        {
            //// Arrange
            QuerySettingParams queryParams = new QuerySettingParams
            {
                CustomerId = 1,
                CustomerLevel = downlineLevel
            };
            _settingContext.Actor.Level = UserLevel.Super;
            _validationDataAccess.ValidatePermission(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<byte>())
                .Returns(Result.Success);

            //// Act
            var actualResult = _validationService.ValidateQuerySetting(queryParams);

            //// Assert
            Assert.IsTrue(actualResult.IsSuccessful);
        }

        [TestCase(CustomerLevel.Super)]
        [TestCase(CustomerLevel.Master)]
        public void ValidateQuerySetting_ActorIsMaster_CannotViewOtherMasterAndSuperSetting(CustomerLevel customerLevel)
        {
            //// Arrange
            QuerySettingParams queryParams = new QuerySettingParams
            {
                CustomerId = 1,
                CustomerLevel = customerLevel
            };
            _settingContext.Actor.Level = UserLevel.Master;

            //// Act
            var actualResult = _validationService.ValidateQuerySetting(queryParams);

            //// Assert
            Assert.IsFalse(actualResult.IsSuccessful);
        }

        [TestCase(CustomerLevel.Agent)]
        [TestCase(CustomerLevel.Member)]
        public void ValidateQuerySetting_ActorIsMaster_CanViewDownlineSetting(CustomerLevel downlineLevel)
        {
            //// Arrange
            QuerySettingParams queryParams = new QuerySettingParams
            {
                CustomerId = 1,
                CustomerLevel = downlineLevel
            };
            _settingContext.Actor.Level = UserLevel.Master;
            _validationDataAccess.ValidatePermission(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<byte>())
                .Returns(Result.Success);

            //// Act
            var actualResult = _validationService.ValidateQuerySetting(queryParams);

            //// Assert
            Assert.IsTrue(actualResult.IsSuccessful);
        }

        [TestCase(CustomerLevel.Super)]
        [TestCase(CustomerLevel.Master)]
        [TestCase(CustomerLevel.Agent)]
        public void ValidateQuerySetting_ActorIsAgent_CannotViewOtherAgentAndUplineSetting(CustomerLevel customerLevel)
        {
            //// Arrange
            QuerySettingParams queryParams = new QuerySettingParams
            {
                CustomerId = 1,
                CustomerLevel = customerLevel
            };
            _settingContext.Actor.Level = UserLevel.Agent;

            //// Act
            var actualResult = _validationService.ValidateQuerySetting(queryParams);

            //// Assert
            Assert.IsFalse(actualResult.IsSuccessful);
        }

        [Test]
        public void ValidateQuerySetting_ActorIsAgent_CanViewMemberSetting()
        {
            //// Arrange
            QuerySettingParams queryParams = new QuerySettingParams
            {
                CustomerId = 1,
                CustomerLevel = CustomerLevel.Member
            };
            _settingContext.Actor.Level = UserLevel.Agent;
            _validationDataAccess.ValidatePermission(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<byte>()).Returns(Result.Success);

            //// Act
            var actualResult = _validationService.ValidateQuerySetting(queryParams);

            //// Assert
            Assert.IsTrue(actualResult.IsSuccessful);
        }

        [Test]
        public void ValidateQuerySetting_ExecuteValidatePerrmissionMethod()
        {
            //// Arrange
            QuerySettingParams queryParams = new QuerySettingParams
            {
                CustomerId = 1,
                CustomerLevel = CustomerLevel.Member
            };
            _settingContext.Actor.Level = UserLevel.Agent;

            //// Act
            _validationService.ValidateQuerySetting(queryParams);

            //// Assert
            _validationDataAccess.Received().ValidatePermission(_settingContext.Actor.Id, queryParams.CustomerId, (byte)queryParams.CustomerLevel);
        }

        #endregion Validatie Setting Query Tests

        #region validate Setting Command Tests

        [Test]
        public void ValidateUpdatingSetting_UpdatingSettingParamsIsNull_ReturnValidationError()
        {
            //// Arrange
            UpdatingSettingParams updatingParams = null;

            //// Act
            var actualValidationResult = _validationService.ValidateUpdatingSetting(updatingParams);

            //// Assert
            Assert.AreEqual(Result.SystemError.Code, actualValidationResult.Code);
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void ValidateUpdatingSetting_CustomerIdIsNegativeOrZero_ReturnValidationError(int customerId)
        {
            //// Arrange
            UpdatingSettingParams updatingParams = FakeObjectManager.CreateUpdatingSettingParams(customerId);

            //// Act
            var actualValidationResult = _validationService.ValidateUpdatingSetting(updatingParams);

            //// Assert
            Assert.AreEqual(Result.SystemError.Code, actualValidationResult.Code);
        }

        [Test]
        public void ValidateUpdatingSetting_ExecuteValidatePermissionOnDataAccess()
        {
            //// Arrange
            var updatingParams = FakeObjectManager.CreateUpdatingSettingParams();

            //// Act
            _validationService.ValidateUpdatingSetting(updatingParams);

            //// Assert
            _validationDataAccess.Received().ValidatePermission(
                _settingContext.Actor.Id,
                updatingParams.Setting.Customer.Id,
                (byte)updatingParams.Setting.Customer.Level);
        }

        [Test]
        public void ValidateUpdatingSetting_ExecuteGetProductSettingTypesOnProductService()
        {
            //// Arrange
            var updatingParams = FakeObjectManager.CreateUpdatingSettingParams();
            _validationDataAccess.ValidatePermission(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<byte>()).Returns(Result.Success);

            //// Act
            _validationService.ValidateUpdatingSetting(updatingParams);

            //// Assert
            _productService.Received().GetProductSettingTypes(Arg.Is<byte>(
                productId => productId == updatingParams.Setting.Product.Id));
        }

        [TestCase(CustomerLevel.Super)]
        [TestCase(CustomerLevel.Master)]
        [TestCase(CustomerLevel.Agent)]
        [TestCase(CustomerLevel.Member)]
        public void ValidateUpdatingSetting_ActorIsCompany_CanUpdateAllCustomerSetting(CustomerLevel customerLevel)
        {
            //// Arrange
            var updatingParams = FakeObjectManager.CreateUpdatingSettingParams(1, customerLevel);
            _settingContext.Actor.Level = UserLevel.Company;
            _validationDataAccess.ValidatePermission(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<byte>()).Returns(Result.Success);
            _productService.GetProductSettingTypes(updatingParams.Setting.Product.Id).Returns(updatingParams.Setting.SettingTypes);

            //// Act
            var actualResult = _validationService.ValidateUpdatingSetting(updatingParams);

            //// Assert
            Assert.IsTrue(actualResult.IsSuccessful);
        }

        [TestCase(CustomerLevel.Super)]
        [TestCase(CustomerLevel.Agent)]
        public void ValidateUpdatingSetting_ActorIsSuper_CannotUpdateOtherSuperAndAgentSetting(CustomerLevel customerLevel)
        {
            //// Arrange
            var updatingParams = FakeObjectManager.CreateUpdatingSettingParams(1, customerLevel);
            _settingContext.Actor.Level = UserLevel.Super;

            //// Act
            var actualResult = _validationService.ValidateUpdatingSetting(updatingParams);

            //// Assert
            Assert.IsFalse(actualResult.IsSuccessful);
        }

        [TestCase(CustomerLevel.Master)]
        [TestCase(CustomerLevel.Member)]
        public void ValidateUpdatingSetting_ActorIsSuper_CanUpdateDownlineSettingExceptAgent(CustomerLevel downlineLevel)
        {
            //// Arrange
            var updatingParams = FakeObjectManager.CreateUpdatingSettingParams(1, downlineLevel);
            _settingContext.Actor.Level = UserLevel.Super;
            _validationDataAccess.ValidatePermission(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<byte>()).Returns(Result.Success);
            _productService.GetProductSettingTypes(updatingParams.Setting.Product.Id).Returns(updatingParams.Setting.SettingTypes);

            //// Act
            var actualResult = _validationService.ValidateUpdatingSetting(updatingParams);

            //// Assert
            Assert.IsTrue(actualResult.IsSuccessful);
        }

        [TestCase(CustomerLevel.Super)]
        [TestCase(CustomerLevel.Master)]
        public void ValidateUpdatingSetting_ActorIsMaster_CannotUpdateOtherMasterAndSuperSetting(CustomerLevel customerLevel)
        {
            //// Arrange
            var updatingParams = FakeObjectManager.CreateUpdatingSettingParams(1, customerLevel);
            _settingContext.Actor.Level = UserLevel.Master;

            //// Act
            var actualResult = _validationService.ValidateUpdatingSetting(updatingParams);

            //// Assert
            Assert.IsFalse(actualResult.IsSuccessful);
        }

        [TestCase(CustomerLevel.Agent)]
        [TestCase(CustomerLevel.Member)]
        public void ValidateUpdatingSetting_ActorIsMaster_CanUpdateDownlineSetting(CustomerLevel downlineLevel)
        {
            //// Arrange
            var updatingParams = FakeObjectManager.CreateUpdatingSettingParams(1, downlineLevel);
            _settingContext.Actor.Level = UserLevel.Master;
            _validationDataAccess.ValidatePermission(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<byte>()).Returns(Result.Success);
            _productService.GetProductSettingTypes(updatingParams.Setting.Product.Id).Returns(updatingParams.Setting.SettingTypes);

            //// Act
            var actualResult = _validationService.ValidateUpdatingSetting(updatingParams);

            //// Assert
            Assert.IsTrue(actualResult.IsSuccessful);
        }

        [TestCase(CustomerLevel.Super)]
        [TestCase(CustomerLevel.Master)]
        [TestCase(CustomerLevel.Agent)]
        public void ValidateUpdatingSetting_ActorIsAgent_CannotUpdateOtherAgentAndUplineSetting(CustomerLevel customerLevel)
        {
            //// Arrange
            var updatingParams = FakeObjectManager.CreateUpdatingSettingParams(1, customerLevel);
            _settingContext.Actor.Level = UserLevel.Agent;

            //// Act
            var actualResult = _validationService.ValidateUpdatingSetting(updatingParams);

            //// Assert
            Assert.IsFalse(actualResult.IsSuccessful);
        }

        [Test]
        public void ValidateUpdatingSetting_ActorIsAgent_CanUpdateMemberSetting()
        {
            //// Arrange
            var updatingParams = FakeObjectManager.CreateUpdatingSettingParams(1, CustomerLevel.Member);
            _settingContext.Actor.Level = UserLevel.Agent;
            _validationDataAccess.ValidatePermission(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<byte>()).Returns(Result.Success);
            _productService.GetProductSettingTypes(updatingParams.Setting.Product.Id).Returns(updatingParams.Setting.SettingTypes);

            //// Act
            var actualResult = _validationService.ValidateUpdatingSetting(updatingParams);

            //// Assert
            Assert.IsTrue(actualResult.IsSuccessful);
        }

        [Test]
        public void ValidateUpdatingSetting_CustomerSettingTypeDoesNotMatchWithProductSettingType_ReturnFailedResult()
        {
            //// Arrange
            var updatingParams = FakeObjectManager.CreateUpdatingSettingParams();
            _validationDataAccess.ValidatePermission(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<byte>()).Returns(Result.Success);
            updatingParams.Setting.SettingTypes = new List<SettingType>
            {
                FakeObjectManager.CreateSettingType(1)
            };
            _productService.GetProductSettingTypes(updatingParams.Setting.Product.Id)
                .Returns(new List<SettingType>
                {
                    FakeObjectManager.CreateSettingType(2)
                });

            //// Act
            var actualResult = _validationService.ValidateUpdatingSetting(updatingParams);

            //// Assert
            Assert.IsFalse(actualResult.IsSuccessful);
        }

        [Test]
        public void ValidateUpdatingSetting_CustomerSettingTypeMatchWithProductSettingType_ReturnSuccessResult()
        {
            //// Arrange
            var updatingParams = FakeObjectManager.CreateUpdatingSettingParams();
            _validationDataAccess.ValidatePermission(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<byte>()).Returns(Result.Success);
            updatingParams.Setting.SettingTypes = new List<SettingType>
            {
                FakeObjectManager.CreateSettingType(1)
            };
            _productService.GetProductSettingTypes(updatingParams.Setting.Product.Id)
                .Returns(new List<SettingType>
                {
                    FakeObjectManager.CreateSettingType(1)
                });

            //// Act
            var actualResult = _validationService.ValidateUpdatingSetting(updatingParams);

            //// Assert
            Assert.IsTrue(actualResult.IsSuccessful);
        }

        [Test]
        public void ValidateUpdatingSetting_ExecuteValidateMethodOnEachSettingItem()
        {
            //// Arrange
            var updatingParams = FakeObjectManager.CreateUpdatingSettingParams();
            var settingItemMock = Substitute.For<ISettingItem>();
            updatingParams.Setting.SettingTypes = new List<SettingType>
            {
                new SettingType
                {
                    Id = 1,
                    SettingTemplate = 1,
                    SettingItems = new List<ISettingItem>
                    {
                        settingItemMock
                    }
                }
            };
            _validationDataAccess.ValidatePermission(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<byte>()).Returns(Result.Success);
            _productService.GetProductSettingTypes(updatingParams.Setting.Product.Id).Returns(updatingParams.Setting.SettingTypes);

            //// Act
            _validationService.ValidateUpdatingSetting(updatingParams);

            //// Assert
            settingItemMock.Received().Validate();
        }

        #endregion validate Setting Command Tests

        #region Validate Multiple Setting Command Tests

        [Test]
        public void ValidateUpdatingMultipleSetting_UpdatingMultipleSettingParamsIsNull_ReturnValidationError()
        {
            //// Arrange
            UpdatingMultipleSettingParams updatingMultipleSettingParams = null;

            //// Act
            var actualValidationResult = _validationService.ValidateUpdatingMultipleSetting(updatingMultipleSettingParams);

            //// Assert
            Assert.AreEqual(Result.SystemError.Code, actualValidationResult.Code);
        }

        [Test]
        public void ValidateUpdatingMultipleSetting_CustomersIsNull_ReturnValidationError()
        {
            //// Arrange
            UpdatingMultipleSettingParams updatingMultipleParams = FakeObjectManager.CreateUpdatingMultipleSettingParams();
            updatingMultipleParams.Customers = null;

            //// Act
            var actualValidationResult = _validationService.ValidateUpdatingMultipleSetting(updatingMultipleParams);

            //// Assert
            Assert.AreEqual(Result.SystemError.Code, actualValidationResult.Code);
        }

        [Test]
        public void ValidateUpdatingMultipleSetting_ExecuteValidatePermissionOnDataAccess()
        {
            //// Arrange
            var updatingMultipleParams = FakeObjectManager.CreateUpdatingMultipleSettingParams();

            //// Act
            _validationService.ValidateUpdatingMultipleSetting(updatingMultipleParams);

            //// Assert
            _validationDataAccess.Received().ValidatePermission(
                _settingContext.Actor.Id,
                updatingMultipleParams.Setting.Customer.Id,
                (byte)updatingMultipleParams.Setting.Customer.Level);
        }

        [Test]
        public void ValidateUpdatingMultipleSetting_ExecuteGetProductSettingTypesOnProductService()
        {
            //// Arrange
            var updatingMultipleParams = FakeObjectManager.CreateUpdatingMultipleSettingParams();
            _validationDataAccess.ValidatePermission(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<byte>()).Returns(Result.Success);

            //// Act
            _validationService.ValidateUpdatingMultipleSetting(updatingMultipleParams);

            //// Assert
            _productService.Received().GetProductSettingTypes(Arg.Is<byte>(
                productId => productId == updatingMultipleParams.Setting.Product.Id));
        }

        [Test]
        public void ValidateUpdatingMultipleSetting_CustomerLevelIsMember_ExecuteValidatePermissionMultipleOnDataAccess()
        {
            //// Arrange
            var updatingMultipleParams = FakeObjectManager.CreateUpdatingMultipleSettingParams(1, CustomerLevel.Member);
            _validationDataAccess.ValidatePermission(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<byte>()).Returns(Result.Success);
            _productService.GetProductSettingTypes(updatingMultipleParams.Setting.Product.Id)
                .Returns(updatingMultipleParams.Setting.SettingTypes);

            //// Act
            _validationService.ValidateUpdatingMultipleSetting(updatingMultipleParams);

            //// Assert
            _validationDataAccess.Received().ValidatePermissionMultiple(
                Arg.Is<int>(actorId => actorId == _settingContext.Actor.Id),
                Arg.Is<string>(memberIds => memberIds == string.Join(",", updatingMultipleParams.Customers.Select(cust => cust.Id))));
        }

        [TestCase(CustomerLevel.Super)]
        [TestCase(CustomerLevel.Master)]
        [TestCase(CustomerLevel.Agent)]
        [TestCase(CustomerLevel.Member)]
        public void ValidateUpdatingMultipleSetting_ActorIsCompany_CanUpdateAllMultipleCustomerSetting(CustomerLevel customerLevel)
        {
            //// Arrange
            var updatingMultipleSettingParams = FakeObjectManager.CreateUpdatingMultipleSettingParams(1, customerLevel);
            _settingContext.Actor.Level = UserLevel.Company;
            _validationDataAccess.ValidatePermission(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<byte>()).Returns(Result.Success);
            _validationDataAccess.ValidatePermissionMultiple(Arg.Any<int>(), Arg.Any<string>()).Returns(Result.Success);
            _productService.GetProductSettingTypes(updatingMultipleSettingParams.Setting.Product.Id).Returns(updatingMultipleSettingParams.Setting.SettingTypes);

            //// Act
            var actualResult = _validationService.ValidateUpdatingMultipleSetting(updatingMultipleSettingParams);

            //// Assert
            Assert.IsTrue(actualResult.IsSuccessful);
        }

        [TestCase(CustomerLevel.Super)]
        [TestCase(CustomerLevel.Agent)]
        public void ValidateUpdatingMultipleSetting_ActorIsSuper_CannotUpdateMultipleOtherSuperAndAgentSetting(CustomerLevel customerLevel)
        {
            //// Arrange
            var updatingMultipleSettingParams = FakeObjectManager.CreateUpdatingMultipleSettingParams(1, customerLevel);
            _settingContext.Actor.Level = UserLevel.Super;

            //// Act
            var actualResult = _validationService.ValidateUpdatingMultipleSetting(updatingMultipleSettingParams);

            //// Assert
            Assert.IsFalse(actualResult.IsSuccessful);
        }

        [TestCase(CustomerLevel.Master)]
        [TestCase(CustomerLevel.Member)]
        public void ValidateUpdatingMultipleSetting_ActorIsSuper_CanUpdateMultipleDownlineSettingExceptAgent(CustomerLevel downlineLevel)
        {
            //// Arrange
            var updatingMultipleSettingParams = FakeObjectManager.CreateUpdatingMultipleSettingParams(1, downlineLevel);
            _settingContext.Actor.Level = UserLevel.Super;
            _validationDataAccess.ValidatePermission(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<byte>()).Returns(Result.Success);
            _validationDataAccess.ValidatePermissionMultiple(Arg.Any<int>(), Arg.Any<string>()).Returns(Result.Success);
            _productService.GetProductSettingTypes(updatingMultipleSettingParams.Setting.Product.Id).Returns(updatingMultipleSettingParams.Setting.SettingTypes);

            //// Act
            var actualResult = _validationService.ValidateUpdatingMultipleSetting(updatingMultipleSettingParams);

            //// Assert
            Assert.IsTrue(actualResult.IsSuccessful);
        }

        [TestCase(CustomerLevel.Super)]
        [TestCase(CustomerLevel.Master)]
        public void ValidateUpdatingMultipleSetting_ActorIsMaster_CannotUpdateMultipleOtherMasterAndSuperSetting(CustomerLevel customerLevel)
        {
            //// Arrange
            var updatingMultipleParams = FakeObjectManager.CreateUpdatingMultipleSettingParams(1, customerLevel);
            _settingContext.Actor.Level = UserLevel.Master;

            //// Act
            var actualResult = _validationService.ValidateUpdatingMultipleSetting(updatingMultipleParams);

            //// Assert
            Assert.IsFalse(actualResult.IsSuccessful);
        }

        [TestCase(CustomerLevel.Agent)]
        [TestCase(CustomerLevel.Member)]
        public void ValidateUpdatingMultipleSetting_ActorIsMaster_CanUpdateMultipleDownlineSetting(CustomerLevel downlineLevel)
        {
            //// Arrange
            var updatingMultipleParams = FakeObjectManager.CreateUpdatingMultipleSettingParams(1, downlineLevel);
            _settingContext.Actor.Level = UserLevel.Master;
            _validationDataAccess.ValidatePermission(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<byte>()).Returns(Result.Success);
            _validationDataAccess.ValidatePermissionMultiple(Arg.Any<int>(), Arg.Any<string>()).Returns(Result.Success);
            _productService.GetProductSettingTypes(updatingMultipleParams.Setting.Product.Id).Returns(updatingMultipleParams.Setting.SettingTypes);

            //// Act
            var actualResult = _validationService.ValidateUpdatingMultipleSetting(updatingMultipleParams);

            //// Assert
            Assert.IsTrue(actualResult.IsSuccessful);
        }

        [TestCase(CustomerLevel.Super)]
        [TestCase(CustomerLevel.Master)]
        [TestCase(CustomerLevel.Agent)]
        public void ValidateUpdatingMultipleSetting_ActorIsAgent_CannotUpdateMultipleOtherAgentAndUplineSetting(CustomerLevel customerLevel)
        {
            //// Arrange
            var updatingMultipleParams = FakeObjectManager.CreateUpdatingMultipleSettingParams(1, customerLevel);
            _settingContext.Actor.Level = UserLevel.Agent;

            //// Act
            var actualResult = _validationService.ValidateUpdatingMultipleSetting(updatingMultipleParams);

            //// Assert
            Assert.IsFalse(actualResult.IsSuccessful);
        }

        [Test]
        public void ValidateUpdatingMultipleSetting_ActorIsAgent_CanUpdateMemberSetting()
        {
            //// Arrange
            var updatingMultipleParams = FakeObjectManager.CreateUpdatingMultipleSettingParams(1, CustomerLevel.Member);
            _settingContext.Actor.Level = UserLevel.Agent;
            _validationDataAccess.ValidatePermission(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<byte>()).Returns(Result.Success);
            _validationDataAccess.ValidatePermissionMultiple(Arg.Any<int>(), Arg.Any<string>()).Returns(Result.Success);
            _productService.GetProductSettingTypes(updatingMultipleParams.Setting.Product.Id).Returns(updatingMultipleParams.Setting.SettingTypes);

            //// Act
            var actualResult = _validationService.ValidateUpdatingMultipleSetting(updatingMultipleParams);

            //// Assert
            Assert.IsTrue(actualResult.IsSuccessful);
        }

        [Test]
        public void ValidateUpdatingMultipleSetting_CustomerSettingTypeDoesNotMatchWithProductSettingType_ReturnFailedResult()
        {
            //// Arrange
            var updatingMultipleParams = FakeObjectManager.CreateUpdatingMultipleSettingParams();
            _validationDataAccess.ValidatePermission(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<byte>()).Returns(Result.Success);
            _validationDataAccess.ValidatePermissionMultiple(Arg.Any<int>(), Arg.Any<string>()).Returns(Result.Success);
            updatingMultipleParams.Setting.SettingTypes = new List<SettingType>
            {
                FakeObjectManager.CreateSettingType(1)
            };
            _productService.GetProductSettingTypes(updatingMultipleParams.Setting.Product.Id)
                .Returns(new List<SettingType>
                {
                    FakeObjectManager.CreateSettingType(2)
                });

            //// Act
            var actualResult = _validationService.ValidateUpdatingMultipleSetting(updatingMultipleParams);

            //// Assert
            Assert.IsFalse(actualResult.IsSuccessful);
        }

        [Test]
        public void ValidateUpdatingMultipleSetting_CustomerSettingTypeMatchWithProductSettingType_ReturnSuccessResult()
        {
            //// Arrange
            var updatingMultipleParams = FakeObjectManager.CreateUpdatingMultipleSettingParams();
            _validationDataAccess.ValidatePermission(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<byte>()).Returns(Result.Success);
            _validationDataAccess.ValidatePermissionMultiple(Arg.Any<int>(), Arg.Any<string>()).Returns(Result.Success);
            updatingMultipleParams.Setting.SettingTypes = new List<SettingType>
            {
                FakeObjectManager.CreateSettingType(1)
            };
            _productService.GetProductSettingTypes(updatingMultipleParams.Setting.Product.Id)
                .Returns(new List<SettingType>
                {
                    FakeObjectManager.CreateSettingType(1)
                });

            //// Act
            var actualResult = _validationService.ValidateUpdatingMultipleSetting(updatingMultipleParams);

            //// Assert
            Assert.IsTrue(actualResult.IsSuccessful);
        }

        [Test]
        public void ValidateUpdatingMultipleSetting_ExecuteValidateMethodOnEachSettingItem()
        {
            //// Arrange
            var updatingMultipleParams = FakeObjectManager.CreateUpdatingMultipleSettingParams();
            var settingItemMock = Substitute.For<ISettingItem>();
            updatingMultipleParams.Setting.SettingTypes = new List<SettingType>
            {
                new SettingType
                {
                    Id = 1,
                    SettingTemplate = 1,
                    SettingItems = new List<ISettingItem>
                    {
                        settingItemMock
                    }
                }
            };
            _validationDataAccess.ValidatePermission(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<byte>()).Returns(Result.Success);
            _validationDataAccess.ValidatePermissionMultiple(Arg.Any<int>(), Arg.Any<string>()).Returns(Result.Success);
            _productService.GetProductSettingTypes(updatingMultipleParams.Setting.Product.Id).Returns(updatingMultipleParams.Setting.SettingTypes);

            //// Act
            _validationService.ValidateUpdatingMultipleSetting(updatingMultipleParams);

            //// Assert
            settingItemMock.Received().Validate();
        }

        #endregion Validate Multiple Setting Command Tests
    }
}