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
            _validationDataAccess.ValidateCustomerAndActorLevel(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<byte>()).Returns(Result.Success);
        }

        [Test, TestCaseSource("ValidateQuerySetting_ActorHasPermission_Cases")]
        public void ValidateQuerySetting_ActorHasPermission_ReturnSuccessValidationResult(
            UserLevel userLevel,
            CustomerLevel customerLevel,
            CustomerLevel? uplineLevel)
        {
            //// Arrange
            QuerySettingParam queryParam = new QuerySettingParam(
                customerId: 1,
                customerLevel: customerLevel,
                productId: 1,
                uplineLevel: uplineLevel);

            _settingContext.Actor.Level = userLevel;

            AssumeAllValidationsExecutedOnDatabaseAreSuccessful();

            //// Act
            var actualResult = _validationService.ValidateQuerySetting(queryParam);

            //// Assert
            Assert.IsTrue(actualResult.IsSuccessful);
        }

        static object[] ValidateQuerySetting_ActorHasPermission_Cases = 
        {
            new object[] {UserLevel.Company, CustomerLevel.Super, null},
            new object[] {UserLevel.Company, CustomerLevel.Master, null},
            new object[] {UserLevel.Company, CustomerLevel.Agent, null},
            new object[] {UserLevel.Company, CustomerLevel.Member, CustomerLevel.Agent},
            new object[] {UserLevel.Super, CustomerLevel.Master, null},
            new object[] {UserLevel.Super, CustomerLevel.Member, CustomerLevel.Super},
            new object[] {UserLevel.Master, CustomerLevel.Agent, null},
            new object[] {UserLevel.Master, CustomerLevel.Member, CustomerLevel.Master},
            new object[] {UserLevel.Agent, CustomerLevel.Member, CustomerLevel.Agent}
        };

        [Test, TestCaseSource("ValidateQuerySetting_ActorDoesNotHavePermission_Cases")]
        public void ValidateQuerySetting_ActorDoesNotHavePermission_ReturnFailedValidationResult(
            UserLevel userLevel,
            CustomerLevel customerLevel)
        {
            //// Arrange
            QuerySettingParam queryParam = new QuerySettingParam(
                customerId: 1,
                customerLevel: customerLevel,
                productId: 1);

            _settingContext.Actor.Level = userLevel;

            AssumeAllValidationsExecutedOnDatabaseAreSuccessful();

            //// Act
            var actualResult = _validationService.ValidateQuerySetting(queryParam);

            //// Assert
            Assert.IsFalse(actualResult.IsSuccessful);
        }

        static object[] ValidateQuerySetting_ActorDoesNotHavePermission_Cases = 
        {
            new object[] {UserLevel.Super, CustomerLevel.Super},
            new object[] {UserLevel.Super, CustomerLevel.Agent},
            new object[] {UserLevel.Master, CustomerLevel.Super},
            new object[] {UserLevel.Master, CustomerLevel.Master},
            new object[] {UserLevel.Agent, CustomerLevel.Super},
            new object[] {UserLevel.Agent, CustomerLevel.Master},
            new object[] {UserLevel.Agent, CustomerLevel.Agent}
        };

        [Test]
        public void ValidateQuerySetting_Always_CallValidateActorEnabled()
        {
            //// Arrange
            QuerySettingParam queryParam = new QuerySettingParam(
                customerId: 1,
                customerLevel: CustomerLevel.Member,
                productId: 1,
                uplineLevel: CustomerLevel.Agent);

            _settingContext.Actor.Level = UserLevel.Agent;

            AssumeAllValidationsExecutedOnDatabaseAreSuccessful();

            //// Act
            _validationService.ValidateQuerySetting(queryParam);

            //// Assert
            _validationDataAccess.Received().ValidateActorEnabled(_settingContext.Actor.Id);
        }

        [Test]
        public void ValidateQuerySetting_Always_CallValidateLevel()
        {
            //// Arrange
            QuerySettingParam queryParam = new QuerySettingParam(
                customerId: 1,
                customerLevel: CustomerLevel.Member,
                productId: 1,
                uplineLevel: CustomerLevel.Agent);
            _settingContext.Actor.Level = UserLevel.Agent;

            _validationDataAccess.ValidateActorEnabled(Arg.Any<int>()).Returns(Result.Success);

            //// Act
            _validationService.ValidateQuerySetting(queryParam);

            //// Assert
            _validationDataAccess.Received().ValidateCustomerAndActorLevel(_settingContext.Actor.Id, queryParam.CustomerId, (byte)queryParam.CustomerLevel);
        }

        [Test]
        public void ValidateQuerySetting_Always_CallValidateBelongIn()
        {
            //// Arrange
            QuerySettingParam queryParam = new QuerySettingParam(
                customerId: 1,
                customerLevel: CustomerLevel.Member,
                productId: 1,
                uplineLevel: CustomerLevel.Agent);
            _settingContext.Actor.Level = UserLevel.Agent;

            _validationDataAccess.ValidateActorEnabled(Arg.Any<int>()).Returns(Result.Success);
            _validationDataAccess.ValidateCustomerAndActorLevel(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<byte>()).Returns(Result.Success);

            //// Act
            _validationService.ValidateQuerySetting(queryParam);

            //// Assert
            _validationDataAccess.Received().ValidateBelongIn(_settingContext.Actor.Id, queryParam.CustomerId);
        }

        [Test, TestCaseSource("ValidateQuerySetting_EdittingMemberAndActorLevelIsDifferentWithUplineLevel_Cases")]
        public void ValidateQuerySetting_EdittingMemberAndActorLevelIsDifferentWithUplineLevel_ReturnAccessDeniedResult(
            UserLevel actorLevel,
            CustomerLevel uplineLevel)
        {
            //// Arrange
            QuerySettingParam queryParam = new QuerySettingParam(
                customerId: 1,
                customerLevel: CustomerLevel.Member,
                productId: 1,
                uplineLevel: uplineLevel);
            _settingContext.Actor.Level = actorLevel;

            AssumeAllValidationsExecutedOnDatabaseAreSuccessful();

            //// Act
            var actualResult = _validationService.ValidateQuerySetting(queryParam);

            //// Assert
            Assert.AreEqual(Result.AccessDenied.Code, actualResult.Code);
        }

        static object[] ValidateQuerySetting_EdittingMemberAndActorLevelIsDifferentWithUplineLevel_Cases = 
        {
            new object[] {UserLevel.Super, CustomerLevel.Master},
            new object[] {UserLevel.Master, CustomerLevel.Agent},
            new object[] {UserLevel.Agent, CustomerLevel.Super}
        };

       [Test, TestCaseSource("ValidateQuerySetting_CompanyEditMemberAndUplineLevelHasValue_Cases")]
        public void ValidateQuerySetting_CompanyEditMemberAndUplineLevelHasValue_ReturnSuccessResult(
            UserLevel actorLevel,
            CustomerLevel uplineLevel)
        {
            //// Arrange
            QuerySettingParam queryParam = new QuerySettingParam(
                customerId: 1,
                customerLevel: CustomerLevel.Member,
                productId: 1,
                uplineLevel: uplineLevel);
            _settingContext.Actor.Level = actorLevel;

            AssumeAllValidationsExecutedOnDatabaseAreSuccessful();

            //// Act
            var actualResult = _validationService.ValidateQuerySetting(queryParam);

            //// Assert
            Assert.IsTrue(actualResult.IsSuccessful);
        }

        static object[] ValidateQuerySetting_CompanyEditMemberAndUplineLevelHasValue_Cases = 
        {
            new object[] {UserLevel.Company, CustomerLevel.Super},
            new object[] {UserLevel.Company, CustomerLevel.Master},
            new object[] {UserLevel.Company, CustomerLevel.Agent},
             new object[] {UserLevel.Super, CustomerLevel.Super},
            new object[] {UserLevel.Master, CustomerLevel.Master},
            new object[] {UserLevel.Agent, CustomerLevel.Agent}
        };

       [Test, TestCaseSource("ValidateQuerySetting_CompanyEditsCustomerIsNotMemberAndUplineLevelIsNull_Cases")]
        public void ValidateQuerySetting_CompanyEditsCustomerIsNotMemberAndUplineLevelIsNull_ReturnSuccessResult(CustomerLevel customerLevel)
        {
            //// Arrange
            QuerySettingParam queryParam = new QuerySettingParam(
                customerId: 1,
                customerLevel: customerLevel,
                productId: 1);
            _settingContext.Actor.Level = UserLevel.Company;

            AssumeAllValidationsExecutedOnDatabaseAreSuccessful();

            //// Act
            var actualResult = _validationService.ValidateQuerySetting(queryParam);

            //// Assert
            Assert.IsTrue(actualResult.IsSuccessful);
        }

        static CustomerLevel[] ValidateQuerySetting_CompanyEditsCustomerIsNotMemberAndUplineLevelIsNull_Cases = new CustomerLevel[] {
            CustomerLevel.Super, CustomerLevel.Master, CustomerLevel.Agent
        };
    }
}
