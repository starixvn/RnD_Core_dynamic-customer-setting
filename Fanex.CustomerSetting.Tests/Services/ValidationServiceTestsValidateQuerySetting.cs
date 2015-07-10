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
    public class ValidationServiceTestsValidateQuerySetting
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

        ////[Test]
        ////public void ValidateQuerySetting_ActorEditsMemberAndUplineLevelIsNull_ReturnAccessDeniedResult()
        ////{
        ////    //// Arrange
        ////    QuerySettingParam queryParam = new QuerySettingParam
        ////    {
        ////        CustomerId = 1,
        ////        CustomerLevel = CustomerLevel.Member,
        ////        UplineLevel = null
        ////    };
        ////    _settingContext.Actor.Level = UserLevel.Agent;
        ////    AssumeAllValidationsExecutedOnDatabaseAreSuccessful();

        ////    //// Act
        ////    var actualResult = _validationService.ValidateQuerySetting(queryParam);

        ////    //// Assert
        ////    Assert.AreEqual(Result.AccessDenied.Code, actualResult.Code);
        ////}

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
    }
}
