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
        public void ValidateSettingQuery_SettingQueryIsNull_ThrowArgumentNullException()
        {
            //// Arrange
            QuerySettingParams settingQuery = null;

            //// Act
            Assert.Throws<ArgumentNullException>(
                new TestDelegate(
                    () => _validationService.ValidateQuerySetting(settingQuery)));
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void ValidateSettingQuery_CustomerIdIsNegativeOrZero_ThrowArgumentOutOfRangeException(int customerId)
        {
            //// Arrange
            QuerySettingParams settingQuery = new QuerySettingParams
            {
                CustomerId = customerId
            };

            //// Act
            Assert.Throws<ArgumentOutOfRangeException>(
                new TestDelegate(
                    () => _validationService.ValidateQuerySetting(settingQuery)));
        }

        [TestCase(CustomerLevel.Super)]
        [TestCase(CustomerLevel.Master)]
        [TestCase(CustomerLevel.Agent)]
        [TestCase(CustomerLevel.Member)]
        public void ValidateSettingQuery_ActorIsCompany_CanViewAllCustomerSetting(CustomerLevel customerLevel)
        {
            //// Arrange
            QuerySettingParams settingQuery = new QuerySettingParams
            {
                CustomerId = 1,
                CustomerLevel = customerLevel
            };
            _settingContext.Actor.Level = UserLevel.Company;
            _validationDataAccess.ValidatePermission(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<byte>())
                .Returns(Result.Success);

            //// Act
            var actualResult = _validationService.ValidateQuerySetting(settingQuery);

            //// Assert
            Assert.IsTrue(actualResult.IsSuccessful);
        }

        [TestCase(CustomerLevel.Super)]
        [TestCase(CustomerLevel.Agent)]
        public void ValidateSettingQuery_ActorIsSuper_CannotViewOtherSuperAndAgentSetting(CustomerLevel customerLevel)
        {
            //// Arrange
            QuerySettingParams settingQuery = new QuerySettingParams
            {
                CustomerId = 1,
                CustomerLevel = customerLevel
            };
            _settingContext.Actor.Level = UserLevel.Super;

            //// Act
            var actualResult = _validationService.ValidateQuerySetting(settingQuery);

            //// Assert
            Assert.IsFalse(actualResult.IsSuccessful);
        }

        [TestCase(CustomerLevel.Master)]
        [TestCase(CustomerLevel.Member)]
        public void ValidateSettingQuery_ActorIsSuper_CanViewDownlineSettingExceptAgent(CustomerLevel downlineLevel)
        {
            //// Arrange
            QuerySettingParams settingQuery = new QuerySettingParams
            {
                CustomerId = 1,
                CustomerLevel = downlineLevel
            };
            _settingContext.Actor.Level = UserLevel.Super;
            _validationDataAccess.ValidatePermission(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<byte>())
                .Returns(Result.Success);

            //// Act
            var actualResult = _validationService.ValidateQuerySetting(settingQuery);

            //// Assert
            Assert.IsTrue(actualResult.IsSuccessful);
        }

        [TestCase(CustomerLevel.Super)]
        [TestCase(CustomerLevel.Master)]
        public void ValidateSettingQuery_ActorIsMaster_CannotViewOtherMasterAndSuperSetting(CustomerLevel customerLevel)
        {
            //// Arrange
            QuerySettingParams settingQuery = new QuerySettingParams
            {
                CustomerId = 1,
                CustomerLevel = customerLevel
            };
            _settingContext.Actor.Level = UserLevel.Master;

            //// Act
            var actualResult = _validationService.ValidateQuerySetting(settingQuery);

            //// Assert
            Assert.IsFalse(actualResult.IsSuccessful);
        }

        [TestCase(CustomerLevel.Agent)]
        [TestCase(CustomerLevel.Member)]
        public void ValidateSettingQuery_ActorIsMaster_CanViewDownlineSetting(CustomerLevel downlineLevel)
        {
            //// Arrange
            QuerySettingParams settingQuery = new QuerySettingParams
            {
                CustomerId = 1,
                CustomerLevel = downlineLevel
            };
            _settingContext.Actor.Level = UserLevel.Master;
            _validationDataAccess.ValidatePermission(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<byte>())
                .Returns(Result.Success);

            //// Act
            var actualResult = _validationService.ValidateQuerySetting(settingQuery);

            //// Assert
            Assert.IsTrue(actualResult.IsSuccessful);
        }

        [TestCase(CustomerLevel.Super)]
        [TestCase(CustomerLevel.Master)]
        [TestCase(CustomerLevel.Agent)]
        public void ValidateSettingQuery_ActorIsAgent_CannotViewOtherAgentAndUplineSetting(CustomerLevel customerLevel)
        {
            //// Arrange
            QuerySettingParams settingQuery = new QuerySettingParams
            {
                CustomerId = 1,
                CustomerLevel = customerLevel
            };
            _settingContext.Actor.Level = UserLevel.Agent;

            //// Act
            var actualResult = _validationService.ValidateQuerySetting(settingQuery);

            //// Assert
            Assert.IsFalse(actualResult.IsSuccessful);
        }

        [Test]
        public void ValidateSettingQuery_ActorIsAgent_CanViewMemberSetting()
        {
            //// Arrange
            QuerySettingParams settingQuery = new QuerySettingParams
            {
                CustomerId = 1,
                CustomerLevel = CustomerLevel.Member
            };
            _settingContext.Actor.Level = UserLevel.Agent;
            _validationDataAccess.ValidatePermission(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<byte>()).Returns(Result.Success);

            //// Act
            var actualResult = _validationService.ValidateQuerySetting(settingQuery);

            //// Assert
            Assert.IsTrue(actualResult.IsSuccessful);
        }

        [Test]
        public void ValidateSettingQuery_ExecuteValidatePerrmissionMethod()
        {
            //// Arrange
            QuerySettingParams settingQuery = new QuerySettingParams
            {
                CustomerId = 1,
                CustomerLevel = CustomerLevel.Member
            };
            _settingContext.Actor.Level = UserLevel.Agent;

            //// Act
            _validationService.ValidateQuerySetting(settingQuery);

            //// Assert
            _validationDataAccess.Received().ValidatePermission(_settingContext.Actor.Id, settingQuery.CustomerId, (byte)settingQuery.CustomerLevel);
        }

        #endregion Validatie Setting Query Tests

        #region validate Setting Command Tests

        [Test]
        public void ValidateSettingCommand_UpdatingSettingParamsIsNull_ReturnValidationError()
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
        public void ValidateSettingCommand_CustomerIdIsNegativeOrZero_ReturnValidationError(int customerId)
        {
            //// Arrange
            UpdatingSettingParams updatingParams = FakeObjectManager.CreateUpdatingSettingParams(customerId);

            //// Act
            var actualValidationResult = _validationService.ValidateUpdatingSetting(updatingParams);

            //// Assert
            Assert.AreEqual(Result.SystemError.Code, actualValidationResult.Code);
        }

        [Test]
        public void ValidateSettingCommand_ExecuteValidatePermissionOnDataAccess()
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
        public void ValidateSettingCommand_ExecuteGetProductSettingTypesOnProductService()
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
        public void ValidateSettingCommand_ActorIsCompany_CanUpdateAllCustomerSetting(CustomerLevel customerLevel)
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
        public void ValidateSettingCommand_ActorIsSuper_CannotUpdateOtherSuperAndAgentSetting(CustomerLevel customerLevel)
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
        public void ValidateSettingCommand_ActorIsSuper_CanUpdateDownlineSettingExceptAgent(CustomerLevel downlineLevel)
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
        public void ValidateSettingCommand_ActorIsMaster_CannotUpdateOtherMasterAndSuperSetting(CustomerLevel customerLevel)
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
        public void ValidateSettingCommand_ActorIsMaster_CanUpdateDownlineSetting(CustomerLevel downlineLevel)
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
        public void ValidateSettingCommand_ActorIsAgent_CannotUpdateOtherAgentAndUplineSetting(CustomerLevel customerLevel)
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
        public void ValidateSettingCommand_ActorIsAgent_CanUpdateMemberSetting()
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
        public void ValidateSettingCommand_CustomerSettingTypeDoesNotMatchWithProductSettingType_ReturnFailedResult()
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
        public void ValidateSettingCommand_CustomerSettingTypeMatchWithProductSettingType_ReturnSuccessResult()
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
        public void ValidateSettingCommand_ExecuteValidateMethodOnEachSettingItem()
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
        public void ValidateupdatingMultipleSettingParams_updatingMultipleSettingParamsIsNull_ReturnValidationError()
        {
            //// Arrange
            UpdatingMultipleSettingParams updatingMultipleSettingParams = null;

            //// Act
            var actualValidationResult = _validationService.ValidateUpdatingMultipleSetting(updatingMultipleSettingParams);

            //// Assert
            Assert.AreEqual(Result.SystemError.Code, actualValidationResult.Code);
        }

        [Test]
        public void ValidateupdatingMultipleSettingParams_CustomersIsNull_ReturnValidationError()
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
        public void ValidateupdatingMultipleSettingParams_ExecuteValidatePermissionOnDataAccess()
        {
            //// Arrange
            var settingCommand = FakeObjectManager.CreateUpdatingMultipleSettingParams();

            //// Act
            _validationService.ValidateUpdatingMultipleSetting(settingCommand);

            //// Assert
            _validationDataAccess.Received().ValidatePermission(
                _settingContext.Actor.Id,
                settingCommand.Setting.Customer.Id,
                (byte)settingCommand.Setting.Customer.Level);
        }

        [Test]
        public void ValidateupdatingMultipleSettingParams_ExecuteGetProductSettingTypesOnProductService()
        {
            //// Arrange
            var settingCommand = FakeObjectManager.CreateUpdatingMultipleSettingParams();
            _validationDataAccess.ValidatePermission(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<byte>()).Returns(Result.Success);

            //// Act
            _validationService.ValidateUpdatingMultipleSetting(settingCommand);

            //// Assert
            _productService.Received().GetProductSettingTypes(Arg.Is<byte>(
                productId => productId == settingCommand.Setting.Product.Id));
        }

        [Test]
        public void ValidateupdatingMultipleSettingParams_CustomerLevelIsMember_ExecuteValidatePermissionMultipleOnDataAccess()
        {
            //// Arrange
            var settingCommand = FakeObjectManager.CreateUpdatingMultipleSettingParams(1, CustomerLevel.Member);
            _validationDataAccess.ValidatePermission(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<byte>()).Returns(Result.Success);
            _productService.GetProductSettingTypes(settingCommand.Setting.Product.Id).Returns(settingCommand.Setting.SettingTypes);

            //// Act
            _validationService.ValidateUpdatingMultipleSetting(settingCommand);

            //// Assert
            _validationDataAccess.Received().ValidatePermissionMultiple(
                Arg.Is<int>(actorId => actorId == _settingContext.Actor.Id),
                Arg.Is<string>(memberIds => memberIds == string.Join(",", settingCommand.Customers.Select(cust => cust.Id))));
        }

        [TestCase(CustomerLevel.Super)]
        [TestCase(CustomerLevel.Master)]
        [TestCase(CustomerLevel.Agent)]
        [TestCase(CustomerLevel.Member)]
        public void ValidateupdatingMultipleSettingParams_ActorIsCompany_CanUpdateAllMultipleCustomerSetting(CustomerLevel customerLevel)
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
        public void ValidateupdatingMultipleSettingParams_ActorIsSuper_CannotUpdateMultipleOtherSuperAndAgentSetting(CustomerLevel customerLevel)
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
        public void ValidateupdatingMultipleSettingParams_ActorIsSuper_CanUpdateMultipleDownlineSettingExceptAgent(CustomerLevel downlineLevel)
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
        public void ValidateupdatingMultipleSettingParams_ActorIsMaster_CannotUpdateMultipleOtherMasterAndSuperSetting(CustomerLevel customerLevel)
        {
            //// Arrange
            var settingCommand = FakeObjectManager.CreateUpdatingMultipleSettingParams(1, customerLevel);
            _settingContext.Actor.Level = UserLevel.Master;

            //// Act
            var actualResult = _validationService.ValidateUpdatingMultipleSetting(settingCommand);

            //// Assert
            Assert.IsFalse(actualResult.IsSuccessful);
        }

        [TestCase(CustomerLevel.Agent)]
        [TestCase(CustomerLevel.Member)]
        public void ValidateupdatingMultipleSettingParams_ActorIsMaster_CanUpdateMultipleDownlineSetting(CustomerLevel downlineLevel)
        {
            //// Arrange
            var settingCommand = FakeObjectManager.CreateUpdatingMultipleSettingParams(1, downlineLevel);
            _settingContext.Actor.Level = UserLevel.Master;
            _validationDataAccess.ValidatePermission(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<byte>()).Returns(Result.Success);
            _validationDataAccess.ValidatePermissionMultiple(Arg.Any<int>(), Arg.Any<string>()).Returns(Result.Success);
            _productService.GetProductSettingTypes(settingCommand.Setting.Product.Id).Returns(settingCommand.Setting.SettingTypes);

            //// Act
            var actualResult = _validationService.ValidateUpdatingMultipleSetting(settingCommand);

            //// Assert
            Assert.IsTrue(actualResult.IsSuccessful);
        }

        [TestCase(CustomerLevel.Super)]
        [TestCase(CustomerLevel.Master)]
        [TestCase(CustomerLevel.Agent)]
        public void ValidateupdatingMultipleSettingParams_ActorIsAgent_CannotUpdateMultipleOtherAgentAndUplineSetting(CustomerLevel customerLevel)
        {
            //// Arrange
            var settingCommand = FakeObjectManager.CreateUpdatingMultipleSettingParams(1, customerLevel);
            _settingContext.Actor.Level = UserLevel.Agent;

            //// Act
            var actualResult = _validationService.ValidateUpdatingMultipleSetting(settingCommand);

            //// Assert
            Assert.IsFalse(actualResult.IsSuccessful);
        }

        [Test]
        public void ValidateupdatingMultipleSettingParams_ActorIsAgent_CanUpdateMemberSetting()
        {
            //// Arrange
            var settingCommand = FakeObjectManager.CreateUpdatingMultipleSettingParams(1, CustomerLevel.Member);
            _settingContext.Actor.Level = UserLevel.Agent;
            _validationDataAccess.ValidatePermission(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<byte>()).Returns(Result.Success);
            _validationDataAccess.ValidatePermissionMultiple(Arg.Any<int>(), Arg.Any<string>()).Returns(Result.Success);
            _productService.GetProductSettingTypes(settingCommand.Setting.Product.Id).Returns(settingCommand.Setting.SettingTypes);

            //// Act
            var actualResult = _validationService.ValidateUpdatingMultipleSetting(settingCommand);

            //// Assert
            Assert.IsTrue(actualResult.IsSuccessful);
        }

        [Test]
        public void ValidateupdatingMultipleSettingParams_CustomerSettingTypeDoesNotMatchWithProductSettingType_ReturnFailedResult()
        {
            //// Arrange
            var settingCommand = FakeObjectManager.CreateUpdatingMultipleSettingParams();
            _validationDataAccess.ValidatePermission(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<byte>()).Returns(Result.Success);
            _validationDataAccess.ValidatePermissionMultiple(Arg.Any<int>(), Arg.Any<string>()).Returns(Result.Success);
            settingCommand.Setting.SettingTypes = new List<SettingType>
            {
                FakeObjectManager.CreateSettingType(1)
            };
            _productService.GetProductSettingTypes(settingCommand.Setting.Product.Id)
                .Returns(new List<SettingType>
                {
                    FakeObjectManager.CreateSettingType(2)
                });

            //// Act
            var actualResult = _validationService.ValidateUpdatingMultipleSetting(settingCommand);

            //// Assert
            Assert.IsFalse(actualResult.IsSuccessful);
        }

        [Test]
        public void ValidateupdatingMultipleSettingParams_CustomerSettingTypeMatchWithProductSettingType_ReturnSuccessResult()
        {
            //// Arrange
            var settingCommand = FakeObjectManager.CreateUpdatingMultipleSettingParams();
            _validationDataAccess.ValidatePermission(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<byte>()).Returns(Result.Success);
            _validationDataAccess.ValidatePermissionMultiple(Arg.Any<int>(), Arg.Any<string>()).Returns(Result.Success);
            settingCommand.Setting.SettingTypes = new List<SettingType>
            {
                FakeObjectManager.CreateSettingType(1)
            };
            _productService.GetProductSettingTypes(settingCommand.Setting.Product.Id)
                .Returns(new List<SettingType>
                {
                    FakeObjectManager.CreateSettingType(1)
                });

            //// Act
            var actualResult = _validationService.ValidateUpdatingMultipleSetting(settingCommand);

            //// Assert
            Assert.IsTrue(actualResult.IsSuccessful);
        }

        [Test]
        public void ValidateupdatingMultipleSettingParams_ExecuteValidateMethodOnEachSettingItem()
        {
            //// Arrange
            var settingCommand = FakeObjectManager.CreateUpdatingMultipleSettingParams();
            var settingItemMock = Substitute.For<ISettingItem>();
            settingCommand.Setting.SettingTypes = new List<SettingType>
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
            _productService.GetProductSettingTypes(settingCommand.Setting.Product.Id).Returns(settingCommand.Setting.SettingTypes);

            //// Act
            _validationService.ValidateUpdatingMultipleSetting(settingCommand);

            //// Assert
            settingItemMock.Received().Validate();
        }

        #endregion Validate Multiple Setting Command Tests
    }
}