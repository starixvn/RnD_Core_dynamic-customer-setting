namespace Fanex.CustomerSetting.Tests.Services
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using CustomerSetting.Criteria;
    using CustomerSetting.Models;
    using CustomerSetting.Models.Enumerations;
    using CustomerSetting.Models.Requests;
    using CustomerSetting.Models.Results;
    using CustomerSetting.Services;
    using CustomerSetting.Services.Impl;
    using Data.Repository;
    using DataAccesses;
    using Logging;
    using Models;
    using NSubstitute;
    using NUnit.Framework;
    using Resource;

    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1310:FieldNamesMustNotContainUnderscore", Justification = "Reviewed.")]
    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1306:FieldNamesMustBeginWithLowerCaseLetter", Justification = "Reviewed.")]
    [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1013:ClosingCurlyBracketsMustBeSpacedCorrectly", Justification = "Reviewed.")]
    [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1204:StaticElementsMustAppearBeforeInstanceElements", Justification = "Reviewed.")]
    [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1012:OpeningCurlyBracketsMustBeSpacedCorrectly", Justification = "Reviewed.")]
    [TestFixture]
    public class ValidationServiceTestsValidateQuerySettingAction
    {
        private ISettingContext _settingContext;
        private IValidationService _validationService;
        private IValidationDataAccess _validationDataAccess;
        private IDynamicRepository _dynamicRepository;
        private ILogger _logger;
        private IProductService _productService;

        [SetUp]
        public void Setup()
        {
            _settingContext = new SettingContextStub();
            _productService = Substitute.For<IProductService>();
            _logger = Substitute.For<ILogger>();
            _validationDataAccess = Substitute.For<IValidationDataAccess>();
            _dynamicRepository = Substitute.For<IDynamicRepository>();

            _validationService = new ValidationService(_settingContext, _productService, _validationDataAccess, _dynamicRepository, _logger);
        }

        [Test]
        public void ValidateQuerySetting_QuerySettingParamIsNull_ThrowArgumentNullException()
        {
            //// Arrange
            CustomerSettingQuery query = null;

            //// Act
            Assert.Throws<ArgumentNullException>(
                new TestDelegate(
                    () => _validationService.ValidateQuerySettingAction(query)));
        }

        private void AssumeCutomerIsDownlineOfActor()
        {
            _dynamicRepository.Get<bool>(Arg.Any<CheckBelongingCriteria>()).Returns(true);
        }

        private void AssumeActorStatusIsEnabled()
        {
            _dynamicRepository.Get<bool>(Arg.Any<ActorStatusQueryCriteria>()).Returns(true);
        }

        private void AssumeCustomerLevelIsEqualWithValueStoreInDatabase(Level customerLevel)
        {
            _dynamicRepository.Get<byte>(Arg.Any<CustomerLevelQueryCriteria>()).Returns(customerLevel.Value);
        }

        [Test, TestCaseSource("ValidateQuerySetting_ActorHasPermission_Cases")]
        public void ValidateQuerySetting_ActorHasPermission_ReturnSuccessResult(
            Level userLevel,
            Level customerLevel,
            Level uplineLevel)
        {
            //// Arrange
            var query = new CustomerSettingQuery(
                customerId: 1,
                customerLevel: customerLevel,
                productId: 1,
                uplineLevel: uplineLevel);

            _settingContext.Actor.Level = userLevel;

            AssumeCutomerIsDownlineOfActor();
            AssumeActorStatusIsEnabled();
            AssumeCustomerLevelIsEqualWithValueStoreInDatabase(query.CustomerLevel);

            //// Act
            var actualResult = _validationService.ValidateQuerySettingAction(query);

            //// Assert
            Assert.IsTrue(actualResult.IsSuccessful);
        }

        private static object[] ValidateQuerySetting_ActorHasPermission_Cases =
        {
            new object[] {Level.Company, Level.Super, null},
            new object[] {Level.Company, Level.Master, null},
            new object[] {Level.Company, Level.Agent, null},
            new object[] {Level.Company, Level.Member, Level.Agent},
            new object[] {Level.Super, Level.Master, null},
            new object[] {Level.Super, Level.Member, Level.Super},
            new object[] {Level.Master, Level.Agent, null},
            new object[] {Level.Master, Level.Member, Level.Master},
            new object[] {Level.Agent, Level.Member, Level.Agent}
        };

        [Test, TestCaseSource("ValidateQuerySetting_ActorDoesNotHavePermission_Cases")]
        public void ValidateQuerySetting_ActorDoesNotHavePermission_ReturnFailedResult(
            Level userLevel,
            Level customerLevel)
        {
            //// Arrange
            var query = new CustomerSettingQuery(
                customerId: 1,
                customerLevel: customerLevel,
                productId: 1);

            _settingContext.Actor.Level = userLevel;

            AssumeActorStatusIsEnabled();
            AssumeCustomerLevelIsEqualWithValueStoreInDatabase(query.CustomerLevel);
            AssumeCutomerIsDownlineOfActor();

            //// Act
            var actualResult = _validationService.ValidateQuerySettingAction(query);

            //// Assert
            Assert.IsFalse(actualResult.IsSuccessful);
        }

        private static object[] ValidateQuerySetting_ActorDoesNotHavePermission_Cases =
        {
            new object[] {Level.Super, Level.Super},
            new object[] {Level.Super, Level.Agent},
            new object[] {Level.Master, Level.Super},
            new object[] {Level.Master, Level.Master},
            new object[] {Level.Agent, Level.Super},
            new object[] {Level.Agent, Level.Master},
            new object[] {Level.Agent, Level.Agent}
        };

        [Test]
        public void ValidateQuerySetting_Always_CallGetActorStatus()
        {
            //// Arrange
            var query = new CustomerSettingQuery(
                customerId: 1,
                customerLevel: Level.Member,
                productId: 1,
                uplineLevel: Level.Agent);

            _settingContext.Actor.Level = Level.Agent;

            //// Act
            _validationService.ValidateQuerySettingAction(query);

            //// Assert
            _dynamicRepository.Received().Get<bool>(Arg.Is<ActorStatusQueryCriteria>(criteria => criteria.CustId == _settingContext.Actor.Id));
        }

        [Test]
        public void ValidateQuerySetting_ActorIsDisabled_ReturnFailedResult()
        {
            //// Arrange
            var query = new CustomerSettingQuery(
                customerId: 1,
                customerLevel: Level.Member,
                productId: 1,
                uplineLevel: Level.Agent);

            _settingContext.Actor.Level = Level.Agent;
            _dynamicRepository.Get<bool>(Arg.Any<ActorStatusQueryCriteria>()).Returns(false);

            //// Act
            var actualResult = _validationService.ValidateQuerySettingAction(query);

            //// Assert
            Assert.IsFalse(actualResult.IsSuccessful);
            Assert.AreEqual(Message.AccountWasDisabled, actualResult.Message);
        }

        [Test]
        public void ValidateQuerySetting_Always_CallGetCustomerLevel()
        {
            //// Arrange
            var query = new CustomerSettingQuery(
                customerId: 1,
                customerLevel: Level.Member,
                productId: 1,
                uplineLevel: Level.Agent);
            _settingContext.Actor.Level = Level.Agent;

            AssumeActorStatusIsEnabled();

            //// Act
            _validationService.ValidateQuerySettingAction(query);

            //// Assert
            _dynamicRepository.Received().Get<byte>(Arg.Is<CustomerLevelQueryCriteria>(criteria => criteria.CustId == query.CustomerId));
        }

        private static object[] ValidateQuerySetting_ActorIsNotCompany_AlwaysCallCheckBelonging_Cases =
        {
            new object[] {Level.Super, Level.Super},
            new object[] {Level.Master, Level.Master},
            new object[] {Level.Agent, Level.Agent}
        };

        [Test, TestCaseSource("ValidateQuerySetting_ActorIsNotCompany_AlwaysCallCheckBelonging_Cases")]
        public void ValidateQuerySetting_ActorIsNotCompany_AlwaysCallCheckBelonging(
            Level actorLevel,
            Level uplineLevel)
        {
            //// Arrange
            var query = new CustomerSettingQuery(
                customerId: 1,
                customerLevel: Level.Member,
                productId: 1,
                uplineLevel: uplineLevel);
            _settingContext.Actor.Level = actorLevel;

            AssumeActorStatusIsEnabled();
            AssumeCustomerLevelIsEqualWithValueStoreInDatabase(query.CustomerLevel);

            //// Act
            _validationService.ValidateQuerySettingAction(query);

            //// Assert
            _dynamicRepository.Received().Get<bool>(Arg.Is<CheckBelongingCriteria>(
                criteria => criteria.ActorId == _settingContext.Actor.Id
                    && criteria.CustomerId == query.CustomerId));
        }

        [Test]
        public void ValidateQuerySetting_IsNotSameLine_ReturnAccessDeninedResult()
        {
            //// Arrange
            var query = new CustomerSettingQuery(
                customerId: 1,
                customerLevel: Level.Member,
                productId: 1,
                uplineLevel: Level.Agent);
            _settingContext.Actor.Level = Level.Agent;

            AssumeActorStatusIsEnabled();
            AssumeCustomerLevelIsEqualWithValueStoreInDatabase(query.CustomerLevel);

            _dynamicRepository.Get<bool>(Arg.Any<CheckBelongingCriteria>()).Returns(false);

            //// Act
            var actualResult = _validationService.ValidateQuerySettingAction(query);

            //// Assert
            Assert.AreEqual(Result.AccessDenied.Code, actualResult.Code);
            Assert.AreEqual("Customers are not in the same line!", actualResult.Message);
        }

        [Test]
        public void ValidateQuerySetting_ActorIsCompany_DontCallValidateBelongIn()
        {
            //// Arrange
            var query = new CustomerSettingQuery(
                customerId: 1,
                customerLevel: Level.Member,
                productId: 1,
                uplineLevel: Level.Agent);
            _settingContext.Actor.Level = Level.Company;

            AssumeActorStatusIsEnabled();
            AssumeCustomerLevelIsEqualWithValueStoreInDatabase(query.CustomerLevel);

            //// Act
            _validationService.ValidateQuerySettingAction(query);

            //// Assert
            _dynamicRepository.DidNotReceive().Get<bool>(Arg.Is<CheckBelongingCriteria>(
                criteria => criteria.ActorId == _settingContext.Actor.Id
                    && criteria.CustomerId == query.CustomerId));
        }

        [Test, TestCaseSource("ValidateQuerySetting_EditingMemberAndActorLevelIsDifferentWithUplineLevel_Cases")]
        public void ValidateQuerySetting_EditingMemberAndActorLevelIsDifferentWithUplineLevel_ReturnAccessDeniedResult(
            Level actorLevel,
            Level uplineLevel)
        {
            //// Arrange
            var query = new CustomerSettingQuery(
                customerId: 1,
                customerLevel: Level.Member,
                productId: 1,
                uplineLevel: uplineLevel);
            _settingContext.Actor.Level = actorLevel;

            AssumeActorStatusIsEnabled();
            AssumeCustomerLevelIsEqualWithValueStoreInDatabase(query.CustomerLevel);
            AssumeCutomerIsDownlineOfActor();

            //// Act
            var actualResult = _validationService.ValidateQuerySettingAction(query);

            //// Assert
            Assert.AreEqual(Result.AccessDenied.Code, actualResult.Code);
            Assert.AreEqual("Upline Level is not valid.", actualResult.Message);
        }

        private static object[] ValidateQuerySetting_EditingMemberAndActorLevelIsDifferentWithUplineLevel_Cases =
        {
            new object[] {Level.Super, Level.Master},
            new object[] {Level.Master, Level.Agent},
            new object[] {Level.Agent, Level.Super}
        };

        [Test, TestCaseSource("ValidateQuerySetting_CompanyEditMemberAndUplineLevelHasValue_Cases")]
        public void ValidateQuerySetting_CompanyEditMemberAndUplineLevelHasValue_ReturnSuccessResult(
            Level actorLevel,
            Level uplineLevel)
        {
            //// Arrange
            var query = new CustomerSettingQuery(
                customerId: 1,
                customerLevel: Level.Member,
                productId: 1,
                uplineLevel: uplineLevel);
            _settingContext.Actor.Level = actorLevel;

            AssumeActorStatusIsEnabled();
            AssumeCustomerLevelIsEqualWithValueStoreInDatabase(query.CustomerLevel);
            AssumeCutomerIsDownlineOfActor();

            //// Act
            var actualResult = _validationService.ValidateQuerySettingAction(query);

            //// Assert
            Assert.IsTrue(actualResult.IsSuccessful);
        }

        private static object[] ValidateQuerySetting_CompanyEditMemberAndUplineLevelHasValue_Cases =
        {
            new object[] {Level.Company, Level.Super},
            new object[] {Level.Company, Level.Master},
            new object[] {Level.Company, Level.Agent},
             new object[] {Level.Super, Level.Super},
            new object[] {Level.Master, Level.Master},
            new object[] {Level.Agent, Level.Agent}
        };

        [Test, TestCaseSource("ValidateQuerySetting_CompanyEditsCustomerIsNotMemberAndUplineLevelIsNull_Cases")]
        public void ValidateQuerySetting_CompanyEditsCustomerIsNotMemberAndUplineLevelIsNull_ReturnSuccessResult(Level customerLevel)
        {
            //// Arrange
            var query = new CustomerSettingQuery(
                customerId: 1,
                customerLevel: customerLevel,
                productId: 1);
            _settingContext.Actor.Level = Level.Company;

            AssumeActorStatusIsEnabled();
            AssumeCustomerLevelIsEqualWithValueStoreInDatabase(query.CustomerLevel);
            AssumeCutomerIsDownlineOfActor();

            //// Act
            var actualResult = _validationService.ValidateQuerySettingAction(query);

            //// Assert
            Assert.IsTrue(actualResult.IsSuccessful);
        }

        private static Level[] ValidateQuerySetting_CompanyEditsCustomerIsNotMemberAndUplineLevelIsNull_Cases =
        {
            Level.Super,
            Level.Master,
            Level.Agent
        };
    }
}