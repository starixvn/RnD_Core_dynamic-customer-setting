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
        public void ValidateQuerySetting_QuerySettingParamIsNull_ThrowArgumentNullException()
        {
            //// Arrange
            QuerySettingParam queryParam = null;

            //// Act
            Assert.Throws<ArgumentNullException>(
                new TestDelegate(
                    () => _validationService.ValidateQuerySetting(queryParam)));
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void ValidateQuerySetting_CustomerIdIsNegativeOrZero_ThrowArgumentOutOfRangeException(int customerId)
        {
            //// Arrange
            QuerySettingParam queryParam = new QuerySettingParam
            {
                CustomerId = customerId
            };

            //// Act
            Assert.Throws<ArgumentOutOfRangeException>(
                new TestDelegate(
                    () => _validationService.ValidateQuerySetting(queryParam)));
        }

        [TestCase(CustomerLevel.Super, null)]
        [TestCase(CustomerLevel.Master, null)]
        [TestCase(CustomerLevel.Agent, null)]
        [TestCase(CustomerLevel.Member, CustomerLevel.Agent)]
        public void ValidateQuerySetting_ActorIsCompany_CanViewAllCustomerSetting(CustomerLevel customerLevel, CustomerLevel? uplineLevel)
        {
            //// Arrange
            QuerySettingParam queryParam = new QuerySettingParam
            {
                CustomerId = 1,
                CustomerLevel = customerLevel,
                UplineLevel = uplineLevel
            };
            _settingContext.Actor.Level = UserLevel.Company;
            _validationDataAccess.ValidatePermission(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<byte>())
                .Returns(Result.Success);

            //// Act
            var actualResult = _validationService.ValidateQuerySetting(queryParam);

            //// Assert
            Assert.IsTrue(actualResult.IsSuccessful);
        }

        [TestCase(CustomerLevel.Super)]
        [TestCase(CustomerLevel.Agent)]
        public void ValidateQuerySetting_ActorIsSuper_CannotViewOtherSuperAndAgentSetting(CustomerLevel customerLevel)
        {
            //// Arrange
            QuerySettingParam queryParam = new QuerySettingParam
            {
                CustomerId = 1,
                CustomerLevel = customerLevel
            };
            _settingContext.Actor.Level = UserLevel.Super;

            //// Act
            var actualResult = _validationService.ValidateQuerySetting(queryParam);

            //// Assert
            Assert.IsFalse(actualResult.IsSuccessful);
        }

        [TestCase(CustomerLevel.Master, null)]
        [TestCase(CustomerLevel.Member, CustomerLevel.Super)]
        public void ValidateQuerySetting_ActorIsSuper_CanViewDownlineSettingExceptAgent(CustomerLevel downlineLevel, CustomerLevel? uplineLevel)
        {
            //// Arrange
            QuerySettingParam queryParam = new QuerySettingParam
            {
                CustomerId = 1,
                CustomerLevel = downlineLevel,
                UplineLevel = uplineLevel
            };
            _settingContext.Actor.Level = UserLevel.Super;
            _validationDataAccess.ValidatePermission(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<byte>())
                .Returns(Result.Success);

            //// Act
            var actualResult = _validationService.ValidateQuerySetting(queryParam);

            //// Assert
            Assert.IsTrue(actualResult.IsSuccessful);
        }

        [TestCase(CustomerLevel.Super)]
        [TestCase(CustomerLevel.Master)]
        public void ValidateQuerySetting_ActorIsMaster_CannotViewOtherMasterAndSuperSetting(CustomerLevel customerLevel)
        {
            //// Arrange
            QuerySettingParam queryParam = new QuerySettingParam
            {
                CustomerId = 1,
                CustomerLevel = customerLevel
            };
            _settingContext.Actor.Level = UserLevel.Master;

            //// Act
            var actualResult = _validationService.ValidateQuerySetting(queryParam);

            //// Assert
            Assert.IsFalse(actualResult.IsSuccessful);
        }

        [TestCase(CustomerLevel.Agent, null)]
        [TestCase(CustomerLevel.Member, CustomerLevel.Master)]
        public void ValidateQuerySetting_ActorIsMaster_CanViewDownlineSetting(CustomerLevel downlineLevel, CustomerLevel? uplineLevel)
        {
            //// Arrange
            QuerySettingParam queryParam = new QuerySettingParam
            {
                CustomerId = 1,
                CustomerLevel = downlineLevel,
                UplineLevel = uplineLevel
            };
            _settingContext.Actor.Level = UserLevel.Master;
            _validationDataAccess.ValidatePermission(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<byte>())
                .Returns(Result.Success);

            //// Act
            var actualResult = _validationService.ValidateQuerySetting(queryParam);

            //// Assert
            Assert.IsTrue(actualResult.IsSuccessful);
        }

        [TestCase(CustomerLevel.Super)]
        [TestCase(CustomerLevel.Master)]
        [TestCase(CustomerLevel.Agent)]
        public void ValidateQuerySetting_ActorIsAgent_CannotViewOtherAgentAndUplineSetting(CustomerLevel customerLevel)
        {
            //// Arrange
            QuerySettingParam queryParam = new QuerySettingParam
            {
                CustomerId = 1,
                CustomerLevel = customerLevel
            };
            _settingContext.Actor.Level = UserLevel.Agent;

            //// Act
            var actualResult = _validationService.ValidateQuerySetting(queryParam);

            //// Assert
            Assert.IsFalse(actualResult.IsSuccessful);
        }

        [Test]
        public void ValidateQuerySetting_ActorIsAgent_CanViewMemberSetting()
        {
            //// Arrange
            QuerySettingParam queryParam = new QuerySettingParam
            {
                CustomerId = 1,
                CustomerLevel = CustomerLevel.Member,
                UplineLevel = CustomerLevel.Agent
            };
            _settingContext.Actor.Level = UserLevel.Agent;
            _validationDataAccess.ValidatePermission(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<byte>()).Returns(Result.Success);

            //// Act
            var actualResult = _validationService.ValidateQuerySetting(queryParam);

            //// Assert
            Assert.IsTrue(actualResult.IsSuccessful);
        }

        [Test]
        public void ValidateQuerySetting_ExecuteValidatePerrmissionMethod()
        {
            //// Arrange
            QuerySettingParam queryParam = new QuerySettingParam
            {
                CustomerId = 1,
                CustomerLevel = CustomerLevel.Member
            };
            _settingContext.Actor.Level = UserLevel.Agent;

            //// Act
            _validationService.ValidateQuerySetting(queryParam);

            //// Assert
            _validationDataAccess.Received().ValidatePermission(_settingContext.Actor.Id, queryParam.CustomerId, (byte)queryParam.CustomerLevel);
        }

        [Test]
        public void ValidateQuerySetting_CustomerIsMemberAndUplineLevelIsNull_ReturnAccessDeniedResult()
        {
            //// Arrange
            QuerySettingParam queryParam = new QuerySettingParam
            {
                CustomerId = 1,
                CustomerLevel = CustomerLevel.Member,
                UplineLevel = null
            };
            _settingContext.Actor.Level = UserLevel.Agent;
            _validationDataAccess.ValidatePermission(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<byte>()).Returns(Result.Success);

            //// Act
            var actualResult = _validationService.ValidateQuerySetting(queryParam);

            //// Assert
            Assert.AreEqual(Result.AccessDenied.Code, actualResult.Code);
        }

        [Test]
        public void ValidateQuerySetting_CustomerIsMemberAndActorIsNotCompany_ReturnAccessDeniedResultWhenActorLevelDifferenceWithUplineLevel()
        {
            //// Arrange
            QuerySettingParam queryParam = new QuerySettingParam
            {
                CustomerId = 1,
                CustomerLevel = CustomerLevel.Member,
                UplineLevel = CustomerLevel.Master
            };
            _settingContext.Actor.Level = UserLevel.Agent;
            _validationDataAccess.ValidatePermission(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<byte>()).Returns(Result.Success);

            //// Act
            var actualResult = _validationService.ValidateQuerySetting(queryParam);

            //// Assert
            Assert.AreEqual(Result.AccessDenied.Code, actualResult.Code);
        }

        [TestCase(CustomerLevel.Super)]
        [TestCase(CustomerLevel.Master)]
        [TestCase(CustomerLevel.Agent)]
        public void ValidateQuerySetting_CustomerIsMemberAndUplineLevelHasValue_ReturnSuccessValidationResult(CustomerLevel uplineLevel)
        {
            //// Arrange
            QuerySettingParam queryParam = new QuerySettingParam
            {
                CustomerId = 1,
                CustomerLevel = CustomerLevel.Member,
                UplineLevel = uplineLevel
            };
            _settingContext.Actor.Level = UserLevel.Company;
            _validationDataAccess.ValidatePermission(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<byte>()).Returns(Result.Success);

            //// Act
            var actualResult = _validationService.ValidateQuerySetting(queryParam);

            //// Assert
            Assert.IsTrue(actualResult.IsSuccessful);
        }

        [TestCase(CustomerLevel.Super)]
        [TestCase(CustomerLevel.Master)]
        [TestCase(CustomerLevel.Agent)]
        public void ValidateQuerySetting_CustomerIsNotMemberAndUplineLevelHasValue_ReturnAccessDeniedResult(CustomerLevel customerLevel)
        {
            //// Arrange
            QuerySettingParam queryParam = new QuerySettingParam
            {
                CustomerId = 1,
                CustomerLevel = customerLevel,
                UplineLevel = customerLevel
            };
            _settingContext.Actor.Level = UserLevel.Company;
            _validationDataAccess.ValidatePermission(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<byte>()).Returns(Result.Success);

            //// Act
            var actualResult = _validationService.ValidateQuerySetting(queryParam);

            //// Assert
            Assert.AreEqual(Result.AccessDenied.Code, actualResult.Code);
        }

        [TestCase(CustomerLevel.Super)]
        [TestCase(CustomerLevel.Master)]
        [TestCase(CustomerLevel.Agent)]
        public void ValidateQuerySetting_CustomerIsNotMemberAndUplineLevelIsNull_ReturnSuccessValidationResult(CustomerLevel customerLevel)
        {
            //// Arrange
            QuerySettingParam queryParam = new QuerySettingParam
            {
                CustomerId = 1,
                CustomerLevel = customerLevel,
                UplineLevel = null
            };
            _settingContext.Actor.Level = UserLevel.Company;
            _validationDataAccess.ValidatePermission(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<byte>()).Returns(Result.Success);

            //// Act
            var actualResult = _validationService.ValidateQuerySetting(queryParam);

            //// Assert
            Assert.IsTrue(actualResult.IsSuccessful);
        }

        #endregion Validatie Setting Query Tests

        #region validate Setting Command Tests

        [Test]
        public void ValidateUpdatingSetting_UpdatingSettingParamIsNull_ReturnValidationError()
        {
            //// Arrange
            UpdatingSettingParam updatingParam = null;

            //// Act
            var actualValidationResult = _validationService.ValidateUpdatingSetting(updatingParam);

            //// Assert
            Assert.AreEqual(Result.SystemError.Code, actualValidationResult.Code);
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void ValidateUpdatingSetting_CustomerIdIsNegativeOrZero_ReturnValidationError(int customerId)
        {
            //// Arrange
            UpdatingSettingParam updatingParam = FakeObjectManager.CreateUpdatingSettingParam(customerId);

            //// Act
            var actualValidationResult = _validationService.ValidateUpdatingSetting(updatingParam);

            //// Assert
            Assert.AreEqual(Result.SystemError.Code, actualValidationResult.Code);
        }

        [Test]
        public void ValidateUpdatingSetting_ExecuteValidatePermissionOnDataAccess()
        {
            //// Arrange
            var updatingParam = FakeObjectManager.CreateUpdatingSettingParam();

            //// Act
            _validationService.ValidateUpdatingSetting(updatingParam);

            //// Assert
            _validationDataAccess.Received().ValidatePermission(
                _settingContext.Actor.Id,
                updatingParam.Setting.Customer.Id,
                (byte)updatingParam.Setting.Customer.Level);
        }

        [Test]
        public void ValidateUpdatingSetting_ExecuteGetProductSettingTypesOnProductService()
        {
            //// Arrange
            var updatingParam = FakeObjectManager.CreateUpdatingSettingParam();
            _validationDataAccess.ValidatePermission(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<byte>()).Returns(Result.Success);

            //// Act
            _validationService.ValidateUpdatingSetting(updatingParam);

            //// Assert
            _productService.Received().GetProductSettingTypes(Arg.Is<byte>(
                productId => productId == updatingParam.Setting.ProductId));
        }

        [TestCase(CustomerLevel.Super, null)]
        [TestCase(CustomerLevel.Master, null)]
        [TestCase(CustomerLevel.Agent, null)]
        [TestCase(CustomerLevel.Member, CustomerLevel.Agent)]
        public void ValidateUpdatingSetting_ActorIsCompany_CanUpdateAllCustomerSetting(CustomerLevel customerLevel, CustomerLevel? uplineLevel)
        {
            //// Arrange
            var updatingParam = FakeObjectManager.CreateUpdatingSettingParam(1, customerLevel, 1, uplineLevel);
            _settingContext.Actor.Level = UserLevel.Company;
            _validationDataAccess.ValidatePermission(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<byte>()).Returns(Result.Success);
            _productService.GetProductSettingTypes(updatingParam.Setting.ProductId).Returns(updatingParam.Setting.SettingTypes);

            //// Act
            var actualResult = _validationService.ValidateUpdatingSetting(updatingParam);

            //// Assert
            Assert.IsTrue(actualResult.IsSuccessful);
        }

        [TestCase(CustomerLevel.Super)]
        [TestCase(CustomerLevel.Agent)]
        public void ValidateUpdatingSetting_ActorIsSuper_CannotUpdateOtherSuperAndAgentSetting(CustomerLevel customerLevel)
        {
            //// Arrange
            var updatingParam = FakeObjectManager.CreateUpdatingSettingParam(1, customerLevel);
            _settingContext.Actor.Level = UserLevel.Super;

            //// Act
            var actualResult = _validationService.ValidateUpdatingSetting(updatingParam);

            //// Assert
            Assert.IsFalse(actualResult.IsSuccessful);
        }

        [TestCase(CustomerLevel.Master, null)]
        [TestCase(CustomerLevel.Member, CustomerLevel.Super)]
        public void ValidateUpdatingSetting_ActorIsSuper_CanUpdateDownlineSettingExceptAgent(CustomerLevel downlineLevel, CustomerLevel? uplineLevel)
        {
            //// Arrange
            var updatingParam = FakeObjectManager.CreateUpdatingSettingParam(1, downlineLevel, 1, uplineLevel);
            _settingContext.Actor.Level = UserLevel.Super;
            _validationDataAccess.ValidatePermission(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<byte>()).Returns(Result.Success);
            _productService.GetProductSettingTypes(updatingParam.Setting.ProductId).Returns(updatingParam.Setting.SettingTypes);

            //// Act
            var actualResult = _validationService.ValidateUpdatingSetting(updatingParam);

            //// Assert
            Assert.IsTrue(actualResult.IsSuccessful);
        }

        [TestCase(CustomerLevel.Super)]
        [TestCase(CustomerLevel.Master)]
        public void ValidateUpdatingSetting_ActorIsMaster_CannotUpdateOtherMasterAndSuperSetting(CustomerLevel customerLevel)
        {
            //// Arrange
            var updatingParam = FakeObjectManager.CreateUpdatingSettingParam(1, customerLevel);
            _settingContext.Actor.Level = UserLevel.Master;

            //// Act
            var actualResult = _validationService.ValidateUpdatingSetting(updatingParam);

            //// Assert
            Assert.IsFalse(actualResult.IsSuccessful);
        }

        [TestCase(CustomerLevel.Agent, null)]
        [TestCase(CustomerLevel.Member, CustomerLevel.Master)]
        public void ValidateUpdatingSetting_ActorIsMaster_CanUpdateDownlineSetting(CustomerLevel downlineLevel, CustomerLevel? uplineLevel)
        {
            //// Arrange
            var updatingParam = FakeObjectManager.CreateUpdatingSettingParam(1, downlineLevel, 1, uplineLevel);
            _settingContext.Actor.Level = UserLevel.Master;
            _validationDataAccess.ValidatePermission(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<byte>()).Returns(Result.Success);
            _productService.GetProductSettingTypes(updatingParam.Setting.ProductId).Returns(updatingParam.Setting.SettingTypes);

            //// Act
            var actualResult = _validationService.ValidateUpdatingSetting(updatingParam);

            //// Assert
            Assert.IsTrue(actualResult.IsSuccessful);
        }

        [TestCase(CustomerLevel.Super)]
        [TestCase(CustomerLevel.Master)]
        [TestCase(CustomerLevel.Agent)]
        public void ValidateUpdatingSetting_ActorIsAgent_CannotUpdateOtherAgentAndUplineSetting(CustomerLevel customerLevel)
        {
            //// Arrange
            var updatingParam = FakeObjectManager.CreateUpdatingSettingParam(1, customerLevel);
            _settingContext.Actor.Level = UserLevel.Agent;

            //// Act
            var actualResult = _validationService.ValidateUpdatingSetting(updatingParam);

            //// Assert
            Assert.IsFalse(actualResult.IsSuccessful);
        }

        [Test]
        public void ValidateUpdatingSetting_ActorIsAgent_CanUpdateMemberSetting()
        {
            //// Arrange
            var updatingParam = FakeObjectManager.CreateUpdatingSettingParam(1, CustomerLevel.Member);
            updatingParam.UplineLevel = CustomerLevel.Agent;
            _settingContext.Actor.Level = UserLevel.Agent;
            _validationDataAccess.ValidatePermission(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<byte>()).Returns(Result.Success);
            _productService.GetProductSettingTypes(updatingParam.Setting.ProductId).Returns(updatingParam.Setting.SettingTypes);

            //// Act
            var actualResult = _validationService.ValidateUpdatingSetting(updatingParam);

            //// Assert
            Assert.IsTrue(actualResult.IsSuccessful);
        }

        [Test]
        public void ValidateUpdatingSetting_CustomerSettingTypeDoesNotMatchWithProductSettingType_ReturnFailedResult()
        {
            //// Arrange
            var updatingParam = FakeObjectManager.CreateUpdatingSettingParam();
            _validationDataAccess.ValidatePermission(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<byte>()).Returns(Result.Success);
            updatingParam.Setting.SettingTypes = new List<SettingType>
            {
                FakeObjectManager.CreateSettingType(1)
            };
            _productService.GetProductSettingTypes(updatingParam.Setting.ProductId)
                .Returns(new List<SettingType>
                {
                    FakeObjectManager.CreateSettingType(2)
                });

            //// Act
            var actualResult = _validationService.ValidateUpdatingSetting(updatingParam);

            //// Assert
            Assert.IsFalse(actualResult.IsSuccessful);
        }

        [Test]
        public void ValidateUpdatingSetting_CustomerSettingTypeMatchWithProductSettingType_ReturnSuccessResult()
        {
            //// Arrange
            var updatingParam = FakeObjectManager.CreateUpdatingSettingParam();
            _validationDataAccess.ValidatePermission(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<byte>()).Returns(Result.Success);
            updatingParam.Setting.SettingTypes = new List<SettingType>
            {
                FakeObjectManager.CreateSettingType(1)
            };
            _productService.GetProductSettingTypes(updatingParam.Setting.ProductId)
                .Returns(new List<SettingType>
                {
                    FakeObjectManager.CreateSettingType(1)
                });

            //// Act
            var actualResult = _validationService.ValidateUpdatingSetting(updatingParam);

            //// Assert
            Assert.IsTrue(actualResult.IsSuccessful);
        }

        [Test]
        public void ValidateUpdatingSetting_ExecuteValidateMethodOnEachSettingItem()
        {
            //// Arrange
            var updatingParam = FakeObjectManager.CreateUpdatingSettingParam();
            var settingItemMock = Substitute.For<ISettingItem>();
            updatingParam.Setting.SettingTypes = new List<SettingType>
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
            _productService.GetProductSettingTypes(updatingParam.Setting.ProductId).Returns(updatingParam.Setting.SettingTypes);

            //// Act
            _validationService.ValidateUpdatingSetting(updatingParam);

            //// Assert
            settingItemMock.Received().Validate();
        }

        [Test]
        public void ValidateUpdatingSetting_CustomerIsMemberAndUplineLevelIsNull_ReturnAccessDeniedResult()
        {
            //// Arrange
            var updatingParam = FakeObjectManager.CreateUpdatingSettingParam(1, CustomerLevel.Member);
            updatingParam.UplineLevel = null;
            _settingContext.Actor.Level = UserLevel.Agent;
            _validationDataAccess.ValidatePermission(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<byte>()).Returns(Result.Success);
            _productService.GetProductSettingTypes(updatingParam.Setting.ProductId).Returns(updatingParam.Setting.SettingTypes);

            //// Act
            var actualResult = _validationService.ValidateUpdatingSetting(updatingParam);

            //// Assert
            Assert.AreEqual(Result.AccessDenied.Code, actualResult.Code);
        }

        [TestCase(CustomerLevel.Super)]
        [TestCase(CustomerLevel.Master)]
        [TestCase(CustomerLevel.Agent)]
        public void ValidateUpdatingSetting_CustomerIsMemberAndUplineLevelHasValue_ReturnSuccessValidationResult(CustomerLevel uplineLevel)
        {
            //// Arrange
            var updatingParam = FakeObjectManager.CreateUpdatingSettingParam(1, CustomerLevel.Member);
            updatingParam.UplineLevel = uplineLevel;
            _settingContext.Actor.Level = UserLevel.Company;
            _validationDataAccess.ValidatePermission(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<byte>()).Returns(Result.Success);
            _productService.GetProductSettingTypes(updatingParam.Setting.ProductId).Returns(updatingParam.Setting.SettingTypes);

            //// Act
            var actualResult = _validationService.ValidateUpdatingSetting(updatingParam);

            //// Assert
            Assert.IsTrue(actualResult.IsSuccessful);
        }

        #endregion validate Setting Command Tests

        #region Validate Multiple Setting Command Tests

        [Test]
        public void ValidateUpdatingMultipleSetting_UpdatingMultipleSettingParamIsNull_ReturnValidationError()
        {
            //// Arrange
            UpdatingMultipleSettingParam updatingMultipleSettingParam = null;

            //// Act
            var actualValidationResult = _validationService.ValidateUpdatingMultipleSetting(updatingMultipleSettingParam);

            //// Assert
            Assert.AreEqual(Result.SystemError.Code, actualValidationResult.Code);
        }

        [Test]
        public void ValidateUpdatingMultipleSetting_CustomersIsNull_ReturnValidationError()
        {
            //// Arrange
            UpdatingMultipleSettingParam updatingMultipleParam = FakeObjectManager.CreateUpdatingMultipleSettingParam();
            updatingMultipleParam.Customers = null;

            //// Act
            var actualValidationResult = _validationService.ValidateUpdatingMultipleSetting(updatingMultipleParam);

            //// Assert
            Assert.AreEqual(Result.SystemError.Code, actualValidationResult.Code);
        }

        [Test]
        public void ValidateUpdatingMultipleSetting_ExecuteValidatePermissionOnDataAccess()
        {
            //// Arrange
            var updatingMultipleParam = FakeObjectManager.CreateUpdatingMultipleSettingParam();

            //// Act
            _validationService.ValidateUpdatingMultipleSetting(updatingMultipleParam);

            //// Assert
            _validationDataAccess.Received().ValidatePermission(
                _settingContext.Actor.Id,
                updatingMultipleParam.Setting.Customer.Id,
                (byte)updatingMultipleParam.Setting.Customer.Level);
        }

        [Test]
        public void ValidateUpdatingMultipleSetting_ExecuteGetProductSettingTypesOnProductService()
        {
            //// Arrange
            var updatingMultipleParam = FakeObjectManager.CreateUpdatingMultipleSettingParam();
            _validationDataAccess.ValidatePermission(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<byte>()).Returns(Result.Success);

            //// Act
            _validationService.ValidateUpdatingMultipleSetting(updatingMultipleParam);

            //// Assert
            _productService.Received().GetProductSettingTypes(Arg.Is<byte>(
                productId => productId == updatingMultipleParam.Setting.ProductId));
        }

        [Test]
        public void ValidateUpdatingMultipleSetting_CustomerLevelIsMember_ExecuteValidatePermissionMultipleOnDataAccess()
        {
            //// Arrange
            var updatingMultipleParam = FakeObjectManager.CreateUpdatingMultipleSettingParam(1, CustomerLevel.Member, 1, CustomerLevel.Agent);
            _validationDataAccess.ValidatePermission(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<byte>()).Returns(Result.Success);
            _productService.GetProductSettingTypes(updatingMultipleParam.Setting.ProductId)
                .Returns(updatingMultipleParam.Setting.SettingTypes);

            //// Act
            _validationService.ValidateUpdatingMultipleSetting(updatingMultipleParam);

            //// Assert
            _validationDataAccess.Received().ValidatePermissionMultiple(
                Arg.Is<int>(actorId => actorId == _settingContext.Actor.Id),
                Arg.Is<string>(memberIds => memberIds == string.Join(",", updatingMultipleParam.Customers.Select(cust => cust.Id))));
        }

        [TestCase(CustomerLevel.Super, null)]
        [TestCase(CustomerLevel.Master, null)]
        [TestCase(CustomerLevel.Agent, null)]
        [TestCase(CustomerLevel.Member, CustomerLevel.Agent)]
        public void ValidateUpdatingMultipleSetting_ActorIsCompany_CanUpdateAllMultipleCustomerSetting(CustomerLevel customerLevel, CustomerLevel? uplineLevel)
        {
            //// Arrange
            var updatingMultipleSettingParam = FakeObjectManager.CreateUpdatingMultipleSettingParam(1, customerLevel, 1, uplineLevel);
            _settingContext.Actor.Level = UserLevel.Company;
            _validationDataAccess.ValidatePermission(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<byte>()).Returns(Result.Success);
            _validationDataAccess.ValidatePermissionMultiple(Arg.Any<int>(), Arg.Any<string>()).Returns(Result.Success);
            _productService.GetProductSettingTypes(updatingMultipleSettingParam.Setting.ProductId).Returns(updatingMultipleSettingParam.Setting.SettingTypes);

            //// Act
            var actualResult = _validationService.ValidateUpdatingMultipleSetting(updatingMultipleSettingParam);

            //// Assert
            Assert.IsTrue(actualResult.IsSuccessful);
        }

        [TestCase(CustomerLevel.Super)]
        [TestCase(CustomerLevel.Agent)]
        public void ValidateUpdatingMultipleSetting_ActorIsSuper_CannotUpdateMultipleOtherSuperAndAgentSetting(CustomerLevel customerLevel)
        {
            //// Arrange
            var updatingMultipleSettingParam = FakeObjectManager.CreateUpdatingMultipleSettingParam(1, customerLevel);
            _settingContext.Actor.Level = UserLevel.Super;

            //// Act
            var actualResult = _validationService.ValidateUpdatingMultipleSetting(updatingMultipleSettingParam);

            //// Assert
            Assert.IsFalse(actualResult.IsSuccessful);
        }

        [TestCase(CustomerLevel.Master, null)]
        [TestCase(CustomerLevel.Member, CustomerLevel.Super)]
        public void ValidateUpdatingMultipleSetting_ActorIsSuper_CanUpdateMultipleDownlineSettingExceptAgent(CustomerLevel downlineLevel, CustomerLevel? uplineLevel)
        {
            //// Arrange
            var updatingMultipleSettingParam = FakeObjectManager.CreateUpdatingMultipleSettingParam(1, downlineLevel, 1, uplineLevel);
            _settingContext.Actor.Level = UserLevel.Super;
            _validationDataAccess.ValidatePermission(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<byte>()).Returns(Result.Success);
            _validationDataAccess.ValidatePermissionMultiple(Arg.Any<int>(), Arg.Any<string>()).Returns(Result.Success);
            _productService.GetProductSettingTypes(updatingMultipleSettingParam.Setting.ProductId).Returns(updatingMultipleSettingParam.Setting.SettingTypes);

            //// Act
            var actualResult = _validationService.ValidateUpdatingMultipleSetting(updatingMultipleSettingParam);

            //// Assert
            Assert.IsTrue(actualResult.IsSuccessful);
        }

        [TestCase(CustomerLevel.Super)]
        [TestCase(CustomerLevel.Master)]
        public void ValidateUpdatingMultipleSetting_ActorIsMaster_CannotUpdateMultipleOtherMasterAndSuperSetting(CustomerLevel customerLevel)
        {
            //// Arrange
            var updatingMultipleParam = FakeObjectManager.CreateUpdatingMultipleSettingParam(1, customerLevel);
            _settingContext.Actor.Level = UserLevel.Master;

            //// Act
            var actualResult = _validationService.ValidateUpdatingMultipleSetting(updatingMultipleParam);

            //// Assert
            Assert.IsFalse(actualResult.IsSuccessful);
        }

        [TestCase(CustomerLevel.Agent, null)]
        [TestCase(CustomerLevel.Member, CustomerLevel.Master)]
        public void ValidateUpdatingMultipleSetting_ActorIsMaster_CanUpdateMultipleDownlineSetting(CustomerLevel downlineLevel, CustomerLevel? uplineLevel)
        {
            //// Arrange
            var updatingMultipleParam = FakeObjectManager.CreateUpdatingMultipleSettingParam(1, downlineLevel, 1, uplineLevel);
            _settingContext.Actor.Level = UserLevel.Master;
            _validationDataAccess.ValidatePermission(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<byte>()).Returns(Result.Success);
            _validationDataAccess.ValidatePermissionMultiple(Arg.Any<int>(), Arg.Any<string>()).Returns(Result.Success);
            _productService.GetProductSettingTypes(updatingMultipleParam.Setting.ProductId).Returns(updatingMultipleParam.Setting.SettingTypes);

            //// Act
            var actualResult = _validationService.ValidateUpdatingMultipleSetting(updatingMultipleParam);

            //// Assert
            Assert.IsTrue(actualResult.IsSuccessful);
        }

        [TestCase(CustomerLevel.Super)]
        [TestCase(CustomerLevel.Master)]
        [TestCase(CustomerLevel.Agent)]
        public void ValidateUpdatingMultipleSetting_ActorIsAgent_CannotUpdateMultipleOtherAgentAndUplineSetting(CustomerLevel customerLevel)
        {
            //// Arrange
            var updatingMultipleParam = FakeObjectManager.CreateUpdatingMultipleSettingParam(1, customerLevel);
            _settingContext.Actor.Level = UserLevel.Agent;

            //// Act
            var actualResult = _validationService.ValidateUpdatingMultipleSetting(updatingMultipleParam);

            //// Assert
            Assert.IsFalse(actualResult.IsSuccessful);
        }

        [Test]
        public void ValidateUpdatingMultipleSetting_ActorIsAgent_CanUpdateMemberSetting()
        {
            //// Arrange
            var updatingMultipleParam = FakeObjectManager.CreateUpdatingMultipleSettingParam(1, CustomerLevel.Member);
            updatingMultipleParam.UplineLevel = CustomerLevel.Agent;
            _settingContext.Actor.Level = UserLevel.Agent;
            _validationDataAccess.ValidatePermission(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<byte>()).Returns(Result.Success);
            _validationDataAccess.ValidatePermissionMultiple(Arg.Any<int>(), Arg.Any<string>()).Returns(Result.Success);
            _productService.GetProductSettingTypes(updatingMultipleParam.Setting.ProductId).Returns(updatingMultipleParam.Setting.SettingTypes);

            //// Act
            var actualResult = _validationService.ValidateUpdatingMultipleSetting(updatingMultipleParam);

            //// Assert
            Assert.IsTrue(actualResult.IsSuccessful);
        }

        [Test]
        public void ValidateUpdatingMultipleSetting_CustomerIsMemberAndUplineLevelIsNull_ReturnAccessDeniedResult()
        {
            //// Arrange
            var updatingMultipleParam = FakeObjectManager.CreateUpdatingMultipleSettingParam(1, CustomerLevel.Member);
            updatingMultipleParam.UplineLevel = null;
            _settingContext.Actor.Level = UserLevel.Agent;
            _validationDataAccess.ValidatePermission(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<byte>()).Returns(Result.Success);
            _validationDataAccess.ValidatePermissionMultiple(Arg.Any<int>(), Arg.Any<string>()).Returns(Result.Success);
            _productService.GetProductSettingTypes(updatingMultipleParam.Setting.ProductId).Returns(updatingMultipleParam.Setting.SettingTypes);

            //// Act
            var actualResult = _validationService.ValidateUpdatingMultipleSetting(updatingMultipleParam);

            //// Assert
            Assert.AreEqual(Result.AccessDenied.Code, actualResult.Code);
        }

        [TestCase(CustomerLevel.Super)]
        [TestCase(CustomerLevel.Master)]
        [TestCase(CustomerLevel.Agent)]
        public void ValidateUpdatingMultipleSetting_CustomerIsMemberAndUplineLevelHasValue_ReturnSuccessValidationResult(CustomerLevel uplineLevel)
        {
            //// Arrange
            var updatingMultipleParam = FakeObjectManager.CreateUpdatingMultipleSettingParam(1, CustomerLevel.Member);
            updatingMultipleParam.UplineLevel = uplineLevel;
            _settingContext.Actor.Level = UserLevel.Company;
            _validationDataAccess.ValidatePermission(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<byte>()).Returns(Result.Success);
            _validationDataAccess.ValidatePermissionMultiple(Arg.Any<int>(), Arg.Any<string>()).Returns(Result.Success);
            _productService.GetProductSettingTypes(updatingMultipleParam.Setting.ProductId).Returns(updatingMultipleParam.Setting.SettingTypes);

            //// Act
            var actualResult = _validationService.ValidateUpdatingMultipleSetting(updatingMultipleParam);

            //// Assert
            Assert.IsTrue(actualResult.IsSuccessful);
        }

        [Test]
        public void ValidateUpdatingMultipleSetting_CustomerSettingTypeDoesNotMatchWithProductSettingType_ReturnFailedResult()
        {
            //// Arrange
            var updatingMultipleParam = FakeObjectManager.CreateUpdatingMultipleSettingParam();
            _validationDataAccess.ValidatePermission(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<byte>()).Returns(Result.Success);
            _validationDataAccess.ValidatePermissionMultiple(Arg.Any<int>(), Arg.Any<string>()).Returns(Result.Success);
            updatingMultipleParam.Setting.SettingTypes = new List<SettingType>
            {
                FakeObjectManager.CreateSettingType(1)
            };
            _productService.GetProductSettingTypes(updatingMultipleParam.Setting.ProductId)
                .Returns(new List<SettingType>
                {
                    FakeObjectManager.CreateSettingType(2)
                });

            //// Act
            var actualResult = _validationService.ValidateUpdatingMultipleSetting(updatingMultipleParam);

            //// Assert
            Assert.IsFalse(actualResult.IsSuccessful);
        }

        [Test]
        public void ValidateUpdatingMultipleSetting_CustomerSettingTypeMatchWithProductSettingType_ReturnSuccessResult()
        {
            //// Arrange
            var updatingMultipleParam = FakeObjectManager.CreateUpdatingMultipleSettingParam();
            _validationDataAccess.ValidatePermission(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<byte>()).Returns(Result.Success);
            _validationDataAccess.ValidatePermissionMultiple(Arg.Any<int>(), Arg.Any<string>()).Returns(Result.Success);
            updatingMultipleParam.Setting.SettingTypes = new List<SettingType>
            {
                FakeObjectManager.CreateSettingType(1)
            };
            _productService.GetProductSettingTypes(updatingMultipleParam.Setting.ProductId)
                .Returns(new List<SettingType>
                {
                    FakeObjectManager.CreateSettingType(1)
                });

            //// Act
            var actualResult = _validationService.ValidateUpdatingMultipleSetting(updatingMultipleParam);

            //// Assert
            Assert.IsTrue(actualResult.IsSuccessful);
        }

        [Test]
        public void ValidateUpdatingMultipleSetting_ExecuteValidateMethodOnEachSettingItem()
        {
            //// Arrange
            var updatingMultipleParam = FakeObjectManager.CreateUpdatingMultipleSettingParam();
            var settingItemMock = Substitute.For<ISettingItem>();
            updatingMultipleParam.Setting.SettingTypes = new List<SettingType>
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
            _productService.GetProductSettingTypes(updatingMultipleParam.Setting.ProductId).Returns(updatingMultipleParam.Setting.SettingTypes);

            //// Act
            _validationService.ValidateUpdatingMultipleSetting(updatingMultipleParam);

            //// Assert
            settingItemMock.Received().Validate();
        }

        #endregion Validate Multiple Setting Command Tests
    }
}