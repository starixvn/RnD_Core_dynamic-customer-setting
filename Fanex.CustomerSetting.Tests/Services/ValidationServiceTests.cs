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

        private void AssumeAllValidationsExecutedOnDatabaseAreSuccessful()
        {
            _validationDataAccess.ValidateActorEnabled(Arg.Any<int>()).Returns(Result.Success);
            _validationDataAccess.ValidateBelongIn(Arg.Any<int>(), Arg.Any<int>()).Returns(Result.Success);
            _validationDataAccess.ValidateLevel(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<byte>()).Returns(Result.Success);
        }

        [TestCase(UserLevel.Company, CustomerLevel.Super, null)]
        [TestCase(UserLevel.Company, CustomerLevel.Master, null)]
        [TestCase(UserLevel.Company, CustomerLevel.Agent, null)]
        [TestCase(UserLevel.Company, CustomerLevel.Member, CustomerLevel.Agent)]
        [TestCase(UserLevel.Super, CustomerLevel.Master, null)]
        [TestCase(UserLevel.Super, CustomerLevel.Member, CustomerLevel.Super)]
        [TestCase(UserLevel.Master, CustomerLevel.Agent, null)]
        [TestCase(UserLevel.Master, CustomerLevel.Member, CustomerLevel.Master)]
        [TestCase(UserLevel.Agent, CustomerLevel.Member, CustomerLevel.Agent)]
        public void ValidateQuerySetting_ActorHasPermission_CanViewCustomerSetting(
            UserLevel userLevel,
            CustomerLevel customerLevel, 
            CustomerLevel? uplineLevel)
        {
            //// Arrange
            QuerySettingParam queryParam = new QuerySettingParam
            {
                CustomerId = 1,
                CustomerLevel = customerLevel,
                UplineLevel = uplineLevel
            };
            _settingContext.Actor.Level = userLevel;
            AssumeAllValidationsExecutedOnDatabaseAreSuccessful();

            //// Act
            var actualResult = _validationService.ValidateQuerySetting(queryParam);

            //// Assert
            Assert.IsTrue(actualResult.IsSuccessful);
        }

        [TestCase(UserLevel.Super, CustomerLevel.Super)]
        [TestCase(UserLevel.Super, CustomerLevel.Agent)]
        [TestCase(UserLevel.Master, CustomerLevel.Super)]
        [TestCase(UserLevel.Master, CustomerLevel.Master)]
        [TestCase(UserLevel.Agent, CustomerLevel.Super)]
        [TestCase(UserLevel.Agent, CustomerLevel.Master)]
        [TestCase(UserLevel.Agent, CustomerLevel.Agent)]
        public void ValidateQuerySetting_ActorDoesNotHavePermission_CannotViewCustomerSetting(
            UserLevel userLevel,
            CustomerLevel customerLevel)
        {
            //// Arrange
            QuerySettingParam queryParam = new QuerySettingParam
            {
                CustomerId = 1,
                CustomerLevel = customerLevel
            };
            _settingContext.Actor.Level = userLevel;
            AssumeAllValidationsExecutedOnDatabaseAreSuccessful();

            //// Act
            var actualResult = _validationService.ValidateQuerySetting(queryParam);

            //// Assert
            Assert.IsFalse(actualResult.IsSuccessful);
        }

        [Test]
        public void ValidateQuerySetting_Always_ExecuteValidateActorEnabledMethod()
        {
            //// Arrange
            QuerySettingParam queryParam = new QuerySettingParam
            {
                CustomerId = 1,
                CustomerLevel = CustomerLevel.Member
            };
            _settingContext.Actor.Level = UserLevel.Agent;
            AssumeAllValidationsExecutedOnDatabaseAreSuccessful();

            //// Act
            _validationService.ValidateQuerySetting(queryParam);

            //// Assert
            _validationDataAccess.Received().ValidateActorEnabled(_settingContext.Actor.Id);
        }

        [Test]
        public void ValidateQuerySetting_Always_ExecuteValidateLevelMethod()
        {
            //// Arrange
            QuerySettingParam queryParam = new QuerySettingParam
            {
                CustomerId = 1,
                CustomerLevel = CustomerLevel.Member
            };
            _settingContext.Actor.Level = UserLevel.Agent;
            _validationDataAccess.ValidateActorEnabled(Arg.Any<int>()).Returns(Result.Success);

            //// Act
            _validationService.ValidateQuerySetting(queryParam);

            //// Assert
            _validationDataAccess.Received().ValidateLevel(_settingContext.Actor.Id, queryParam.CustomerId, (byte)queryParam.CustomerLevel);
        }

        [Test]
        public void ValidateQuerySetting_Always_ExecuteValidateBelongInMethod()
        {
            //// Arrange
            QuerySettingParam queryParam = new QuerySettingParam
            {
                CustomerId = 1,
                CustomerLevel = CustomerLevel.Member
            };
            _settingContext.Actor.Level = UserLevel.Agent;
            _validationDataAccess.ValidateActorEnabled(Arg.Any<int>()).Returns(Result.Success);
            _validationDataAccess.ValidateLevel(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<byte>()).Returns(Result.Success);

            //// Act
            _validationService.ValidateQuerySetting(queryParam);

            //// Assert
            _validationDataAccess.Received().ValidateBelongIn(_settingContext.Actor.Id, queryParam.CustomerId);
        }

        [Test]
        public void ValidateQuerySetting_ActorEditsMemberAndUplineLevelIsNull_ReturnAccessDeniedResult()
        {
            //// Arrange
            QuerySettingParam queryParam = new QuerySettingParam
            {
                CustomerId = 1,
                CustomerLevel = CustomerLevel.Member,
                UplineLevel = null
            };
            _settingContext.Actor.Level = UserLevel.Agent;
            AssumeAllValidationsExecutedOnDatabaseAreSuccessful();

            //// Act
            var actualResult = _validationService.ValidateQuerySetting(queryParam);

            //// Assert
            Assert.AreEqual(Result.AccessDenied.Code, actualResult.Code);
        }

        [TestCase(UserLevel.Super, CustomerLevel.Master)]
        [TestCase(UserLevel.Master, CustomerLevel.Agent)]
        [TestCase(UserLevel.Agent, CustomerLevel.Super)]
        public void ValidateQuerySetting_ActorEditsMemberAndActorLevelIsDifferentWithUplineLevel_ReturnAccessDeniedResult(
            UserLevel actorLevel,
            CustomerLevel uplineLevel)
        {
            //// Arrange
            QuerySettingParam queryParam = new QuerySettingParam
            {
                CustomerId = 1,
                CustomerLevel = CustomerLevel.Member,
                UplineLevel = uplineLevel
            };
            _settingContext.Actor.Level = actorLevel;
            AssumeAllValidationsExecutedOnDatabaseAreSuccessful();

            //// Act
            var actualResult = _validationService.ValidateQuerySetting(queryParam);

            //// Assert
            Assert.AreEqual(Result.AccessDenied.Code, actualResult.Code);
        }

        [TestCase(UserLevel.Company, CustomerLevel.Super)]
        [TestCase(UserLevel.Company, CustomerLevel.Master)]
        [TestCase(UserLevel.Company, CustomerLevel.Agent)]
        [TestCase(UserLevel.Super, CustomerLevel.Super)]
        [TestCase(UserLevel.Master, CustomerLevel.Master)]
        [TestCase(UserLevel.Agent, CustomerLevel.Agent)]
        public void ValidateQuerySetting_CompanyEditMemberAndUplineLevelHasValue_ReturnSuccessValidationResult(
            UserLevel actorLevel,
            CustomerLevel uplineLevel)
        {
            //// Arrange
            QuerySettingParam queryParam = new QuerySettingParam
            {
                CustomerId = 1,
                CustomerLevel = CustomerLevel.Member,
                UplineLevel = uplineLevel
            };
            _settingContext.Actor.Level = actorLevel;
            AssumeAllValidationsExecutedOnDatabaseAreSuccessful();

            //// Act
            var actualResult = _validationService.ValidateQuerySetting(queryParam);

            //// Assert
            Assert.IsTrue(actualResult.IsSuccessful);
        }

        [TestCase(CustomerLevel.Super)]
        [TestCase(CustomerLevel.Master)]
        [TestCase(CustomerLevel.Agent)]
        public void ValidateQuerySetting_CompanyEditsCustomerIsNotMemberAndUplineLevelHasValue_ReturnAccessDeniedResult(CustomerLevel customerLevel)
        {
            //// Arrange
            QuerySettingParam queryParam = new QuerySettingParam
            {
                CustomerId = 1,
                CustomerLevel = customerLevel,
                UplineLevel = customerLevel
            };
            _settingContext.Actor.Level = UserLevel.Company;
            AssumeAllValidationsExecutedOnDatabaseAreSuccessful();

            //// Act
            var actualResult = _validationService.ValidateQuerySetting(queryParam);

            //// Assert
            Assert.AreEqual(Result.AccessDenied.Code, actualResult.Code);
        }

        [TestCase(CustomerLevel.Super)]
        [TestCase(CustomerLevel.Master)]
        [TestCase(CustomerLevel.Agent)]
        public void ValidateQuerySetting_CompanyEditsCustomerIsNotMemberAndUplineLevelIsNull_ReturnSuccessValidationResult(CustomerLevel customerLevel)
        {
            //// Arrange
            QuerySettingParam queryParam = new QuerySettingParam
            {
                CustomerId = 1,
                CustomerLevel = customerLevel,
                UplineLevel = null
            };
            _settingContext.Actor.Level = UserLevel.Company;
            AssumeAllValidationsExecutedOnDatabaseAreSuccessful();

            //// Act
            var actualResult = _validationService.ValidateQuerySetting(queryParam);

            //// Assert
            Assert.IsTrue(actualResult.IsSuccessful);
        }

        #endregion Validatie Setting Query Tests

        #region validate Updating Setting Tests

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
            UpdatingSettingParam updatingParam = InstanceCreator.CreateUpdatingSettingParam(customerId);

            //// Act
            var actualValidationResult = _validationService.ValidateUpdatingSetting(updatingParam);

            //// Assert
            Assert.AreEqual(Result.SystemError.Code, actualValidationResult.Code);
        }

        [Test]
        public void ValidateUpdatingSetting_Always_ExecuteValidateActorEnabledOnDataAccess()
        {
            //// Arrange
            var updatingParam = InstanceCreator.CreateUpdatingSettingParam();

            //// Act
            _validationService.ValidateUpdatingSetting(updatingParam);

            //// Assert
            _validationDataAccess.Received().ValidateActorEnabled(_settingContext.Actor.Id);
        }

        [Test]
        public void ValidateUpdatingSetting_Always_ExecuteGetProductSettingTypesOnProductService()
        {
            //// Arrange
            var updatingParam = InstanceCreator.CreateUpdatingSettingParam();
            AssumeAllValidationsExecutedOnDatabaseAreSuccessful();

            //// Act
            _validationService.ValidateUpdatingSetting(updatingParam);

            //// Assert
            _productService.Received().GetProductSettingTypes(Arg.Is<byte>(
                productId => productId == updatingParam.Setting.ProductId));
        }

        [TestCase(UserLevel.Company, CustomerLevel.Super, null)]
        [TestCase(UserLevel.Company, CustomerLevel.Master, null)]
        [TestCase(UserLevel.Company, CustomerLevel.Agent, null)]
        [TestCase(UserLevel.Company, CustomerLevel.Member, CustomerLevel.Agent)]
        [TestCase(UserLevel.Super, CustomerLevel.Master, null)]
        [TestCase(UserLevel.Super, CustomerLevel.Member, CustomerLevel.Super)]
        [TestCase(UserLevel.Master, CustomerLevel.Agent, null)]
        [TestCase(UserLevel.Master, CustomerLevel.Member, CustomerLevel.Master)]
        [TestCase(UserLevel.Agent, CustomerLevel.Member, CustomerLevel.Agent)]
        public void ValidateUpdatingSetting_ActorHasPermission_CanUpdateCustomerSetting(
            UserLevel actorLevel,
            CustomerLevel customerLevel, 
            CustomerLevel? uplineLevel)
        {
            //// Arrange
            var updatingParam = InstanceCreator.CreateUpdatingSettingParam(1, customerLevel, 1, uplineLevel);
            _settingContext.Actor.Level = actorLevel;
            AssumeAllValidationsExecutedOnDatabaseAreSuccessful();
            _productService.GetProductSettingTypes(updatingParam.Setting.ProductId).Returns(updatingParam.Setting.SettingTypes);

            //// Act
            var actualResult = _validationService.ValidateUpdatingSetting(updatingParam);

            //// Assert
            Assert.IsTrue(actualResult.IsSuccessful);
        }

        [TestCase(UserLevel.Super, CustomerLevel.Super)]
        [TestCase(UserLevel.Super, CustomerLevel.Agent)]
        [TestCase(UserLevel.Master, CustomerLevel.Super)]
        [TestCase(UserLevel.Master, CustomerLevel.Master)]
        [TestCase(UserLevel.Agent, CustomerLevel.Super)]
        [TestCase(UserLevel.Agent, CustomerLevel.Master)]
        [TestCase(UserLevel.Agent, CustomerLevel.Agent)]
        public void ValidateUpdatingSetting_ActorDoesNotHavePermission_CannotUpdateCustomerSetting(
            UserLevel actorLevel,
            CustomerLevel customerLevel)
        {
            //// Arrange
            var updatingParam = InstanceCreator.CreateUpdatingSettingParam(1, customerLevel);
            _settingContext.Actor.Level = actorLevel;
            AssumeAllValidationsExecutedOnDatabaseAreSuccessful();

            //// Act
            var actualResult = _validationService.ValidateUpdatingSetting(updatingParam);

            //// Assert
            Assert.IsFalse(actualResult.IsSuccessful);
        }

        [Test]
        public void ValidateUpdatingSetting_CustomerSettingTypeDoesNotMatchWithProductSettingType_ReturnFailedResult()
        {
            //// Arrange
            var updatingParam = InstanceCreator.CreateUpdatingSettingParam();
            AssumeAllValidationsExecutedOnDatabaseAreSuccessful();
            updatingParam.Setting.SettingTypes = new List<SettingType>
            {
                InstanceCreator.CreateCustomerSettingType(1)
            };
            _productService.GetProductSettingTypes(updatingParam.Setting.ProductId)
                .Returns(new List<SettingType>
                {
                    InstanceCreator.CreateCustomerSettingType(2)
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
            var updatingParam = InstanceCreator.CreateUpdatingSettingParam();
            AssumeAllValidationsExecutedOnDatabaseAreSuccessful();
            updatingParam.Setting.SettingTypes = new List<SettingType>
            {
                InstanceCreator.CreateCustomerSettingType(1)
            };
            _productService.GetProductSettingTypes(updatingParam.Setting.ProductId)
                .Returns(new List<SettingType>
                {
                    InstanceCreator.CreateCustomerSettingType(1)
                });

            //// Act
            var actualResult = _validationService.ValidateUpdatingSetting(updatingParam);

            //// Assert
            Assert.IsTrue(actualResult.IsSuccessful);
        }

        [Test]
        public void ValidateUpdatingSetting_Always_ExecuteValidateMethodOnEachSettingItem()
        {
            //// Arrange
            var updatingParam = InstanceCreator.CreateUpdatingSettingParam();
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
            AssumeAllValidationsExecutedOnDatabaseAreSuccessful();
            _productService.GetProductSettingTypes(updatingParam.Setting.ProductId).Returns(updatingParam.Setting.SettingTypes);

            //// Act
            _validationService.ValidateUpdatingSetting(updatingParam);

            //// Assert
            settingItemMock.Received().Validate();
        }

        #endregion validate Setting Command Tests

        #region Validate Updating Multiple Setting Tests

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
            UpdatingMultipleSettingParam updatingMultipleParam = InstanceCreator.CreateUpdatingMultipleSettingParam();
            updatingMultipleParam.CustomerIds = null;

            //// Act
            var actualValidationResult = _validationService.ValidateUpdatingMultipleSetting(updatingMultipleParam);

            //// Assert
            Assert.AreEqual(Result.SystemError.Code, actualValidationResult.Code);
        }

        [Test]
        public void ValidateUpdatingMultipleSetting_CustomerLevelIsMember_ExecuteValidatePermissionMultipleOnDataAccess()
        {
            //// Arrange
            var updatingMultipleParam = InstanceCreator.CreateUpdatingMultipleSettingParam(1, CustomerLevel.Member, 1, CustomerLevel.Agent);
            AssumeAllValidationsExecutedOnDatabaseAreSuccessful();
            _productService.GetProductSettingTypes(updatingMultipleParam.Setting.ProductId)
                .Returns(updatingMultipleParam.Setting.SettingTypes);

            //// Act
            _validationService.ValidateUpdatingMultipleSetting(updatingMultipleParam);

            //// Assert
            _validationDataAccess.Received().ValidatePermissionMultiple(
                Arg.Is<int>(actorId => actorId == _settingContext.Actor.Id),
                Arg.Is<string>(memberIds => memberIds == string.Join(",", updatingMultipleParam.CustomerIds)));
        }

        [TestCase(UserLevel.Company, CustomerLevel.Super, null)]
        [TestCase(UserLevel.Company, CustomerLevel.Master, null)]
        [TestCase(UserLevel.Company, CustomerLevel.Agent, null)]
        [TestCase(UserLevel.Company, CustomerLevel.Member, CustomerLevel.Agent)]
        [TestCase(UserLevel.Super, CustomerLevel.Master, null)]
        [TestCase(UserLevel.Super, CustomerLevel.Member, CustomerLevel.Super)]
        [TestCase(UserLevel.Master, CustomerLevel.Agent, null)]
        [TestCase(UserLevel.Master, CustomerLevel.Member, CustomerLevel.Master)]
        [TestCase(UserLevel.Agent, CustomerLevel.Member, CustomerLevel.Agent)]
        public void ValidateUpdatingMultipleSetting_ActorHasPermission_CanUpdateMultipleCustomerSetting(
            UserLevel actorLevel,
            CustomerLevel customerLevel, 
            CustomerLevel? uplineLevel)
        {
            //// Arrange
            var updatingMultipleSettingParam = InstanceCreator.CreateUpdatingMultipleSettingParam(1, customerLevel, 1, uplineLevel);
            _settingContext.Actor.Level = actorLevel;
            AssumeAllValidationsExecutedOnDatabaseAreSuccessful();
            _validationDataAccess.ValidatePermissionMultiple(Arg.Any<int>(), Arg.Any<string>()).Returns(Result.Success);
            _productService.GetProductSettingTypes(updatingMultipleSettingParam.Setting.ProductId).Returns(updatingMultipleSettingParam.Setting.SettingTypes);

            //// Act
            var actualResult = _validationService.ValidateUpdatingMultipleSetting(updatingMultipleSettingParam);

            //// Assert
            Assert.IsTrue(actualResult.IsSuccessful);
        }

        [TestCase(UserLevel.Super, CustomerLevel.Super)]
        [TestCase(UserLevel.Super, CustomerLevel.Agent)]
        [TestCase(UserLevel.Master, CustomerLevel.Super)]
        [TestCase(UserLevel.Master, CustomerLevel.Master)]
        [TestCase(UserLevel.Agent, CustomerLevel.Super)]
        [TestCase(UserLevel.Agent, CustomerLevel.Master)]
        [TestCase(UserLevel.Agent, CustomerLevel.Agent)]
        public void ValidateUpdatingMultipleSetting_ActorDoesNotHavePermission_CannotUpdateMultipleCustomerSetting(
            UserLevel actorLevel,
            CustomerLevel customerLevel)
        {
            //// Arrange
            var updatingMultipleSettingParam = InstanceCreator.CreateUpdatingMultipleSettingParam(1, customerLevel);
            _settingContext.Actor.Level = actorLevel;

            //// Act
            var actualResult = _validationService.ValidateUpdatingMultipleSetting(updatingMultipleSettingParam);

            //// Assert
            Assert.IsFalse(actualResult.IsSuccessful);
        }

        #endregion Validate Multiple Setting Command Tests
    }
}