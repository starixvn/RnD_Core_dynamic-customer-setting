namespace Fanex.CustomerSetting.Tests.Services
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.CustomerSetting.Criteria;
    using Fanex.CustomerSetting.DataAccesses;
    using Fanex.CustomerSetting.Dtos.Params;
    using Fanex.CustomerSetting.Dtos.Results;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Services;
    using Fanex.CustomerSetting.Services.Impl;
    using Fanex.CustomerSetting.Tests.Models;
    using Fanex.Logging;
    using Fanex.NAP.Framework.Data;
    using NSubstitute;
    using NUnit.Framework;

    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1310:FieldNamesMustNotContainUnderscore", Justification = "Reviewed.")]
    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1306:FieldNamesMustBeginWithLowerCaseLetter", Justification = "Reviewed.")]
    [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1013:ClosingCurlyBracketsMustBeSpacedCorrectly", Justification = "Reviewed.")]
    [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1204:StaticElementsMustAppearBeforeInstanceElements", Justification = "Reviewed.")]
    [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1012:OpeningCurlyBracketsMustBeSpacedCorrectly", Justification = "Reviewed.")]
    [TestFixture]
    public class ValidationServiceTestsValidateQuerySetting
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
            QuerySettingParam queryParam = null;

            //// Act
            Assert.Throws<ArgumentNullException>(
                new TestDelegate(
                    () => _validationService.ValidateQuerySetting(queryParam)));
        }

        private void AssumeCutomerIsDownlineOfActor()
        {
            _validationDataAccess.ValidateBelonging(Arg.Any<int>(), Arg.Any<int>()).Returns(true);
        }

        private void AssumeActorStatusIsEnabled()
        {
            _dynamicRepository.Get<bool>(Arg.Any<ActorStatusQueryCriteria>()).Returns(true);
        }

        private void AssumeCustomerLevelIsEqualWithValueStoreInDatabase(UserLevel customerLevel)
        {
            _dynamicRepository.Get<byte>(Arg.Any<CustomerLevelQueryCriteria>()).Returns(customerLevel.Value);
        }

        [Test, TestCaseSource("ValidateQuerySetting_ActorHasPermission_Cases")]
        public void ValidateQuerySetting_ActorHasPermission_ReturnSuccessResult(
            UserLevel userLevel,
            UserLevel customerLevel,
            UserLevel uplineLevel)
        {
            //// Arrange
            QuerySettingParam queryParam = new QuerySettingParam(
                customerId: 1,
                customerLevel: customerLevel,
                productId: 1,
                uplineLevel: uplineLevel);

            _settingContext.Actor.Level = userLevel;

            AssumeCutomerIsDownlineOfActor();
            AssumeActorStatusIsEnabled();
            AssumeCustomerLevelIsEqualWithValueStoreInDatabase(queryParam.CustomerLevel);

            //// Act
            var actualResult = _validationService.ValidateQuerySetting(queryParam);

            //// Assert
            Assert.IsTrue(actualResult.IsSuccessful);
        }

        private static object[] ValidateQuerySetting_ActorHasPermission_Cases =
        {
            new object[] {UserLevel.Company, UserLevel.Super, null},
            new object[] {UserLevel.Company, UserLevel.Master, null},
            new object[] {UserLevel.Company, UserLevel.Agent, null},
            new object[] {UserLevel.Company, UserLevel.Member, UserLevel.Agent},
            new object[] {UserLevel.Super, UserLevel.Master, null},
            new object[] {UserLevel.Super, UserLevel.Member, UserLevel.Super},
            new object[] {UserLevel.Master, UserLevel.Agent, null},
            new object[] {UserLevel.Master, UserLevel.Member, UserLevel.Master},
            new object[] {UserLevel.Agent, UserLevel.Member, UserLevel.Agent}
        };

        [Test, TestCaseSource("ValidateQuerySetting_ActorDoesNotHavePermission_Cases")]
        public void ValidateQuerySetting_ActorDoesNotHavePermission_ReturnFailedResult(
            UserLevel userLevel,
            UserLevel customerLevel)
        {
            //// Arrange
            QuerySettingParam queryParam = new QuerySettingParam(
                customerId: 1,
                customerLevel: customerLevel,
                productId: 1);

            _settingContext.Actor.Level = userLevel;

            AssumeActorStatusIsEnabled();
            AssumeCustomerLevelIsEqualWithValueStoreInDatabase(queryParam.CustomerLevel);
            AssumeCutomerIsDownlineOfActor();

            //// Act
            var actualResult = _validationService.ValidateQuerySetting(queryParam);

            //// Assert
            Assert.IsFalse(actualResult.IsSuccessful);
        }

        private static object[] ValidateQuerySetting_ActorDoesNotHavePermission_Cases =
        {
            new object[] {UserLevel.Super, UserLevel.Super},
            new object[] {UserLevel.Super, UserLevel.Agent},
            new object[] {UserLevel.Master, UserLevel.Super},
            new object[] {UserLevel.Master, UserLevel.Master},
            new object[] {UserLevel.Agent, UserLevel.Super},
            new object[] {UserLevel.Agent, UserLevel.Master},
            new object[] {UserLevel.Agent, UserLevel.Agent}
        };

        [Test]
        public void ValidateQuerySetting_Always_CallGetActorStatus()
        {
            //// Arrange
            QuerySettingParam queryParam = new QuerySettingParam(
                customerId: 1,
                customerLevel: UserLevel.Member,
                productId: 1,
                uplineLevel: UserLevel.Agent);

            _settingContext.Actor.Level = UserLevel.Agent;

            //// Act
            _validationService.ValidateQuerySetting(queryParam);

            //// Assert
            _dynamicRepository.Received().Get<bool>(Arg.Is<ActorStatusQueryCriteria>(criteria => criteria.ActorId == _settingContext.Actor.Id));
        }

        [Test]
        public void ValidateQuerySetting_ActorIsDisabled_ReturnFailedResult()
        {
            //// Arrange
            QuerySettingParam queryParam = new QuerySettingParam(
                customerId: 1,
                customerLevel: UserLevel.Member,
                productId: 1,
                uplineLevel: UserLevel.Agent);

            _settingContext.Actor.Level = UserLevel.Agent;
            _settingContext.Actor.Name = "ActorName";
            _dynamicRepository.Get<bool>(Arg.Any<ActorStatusQueryCriteria>()).Returns(false);

            //// Act
            var actualResult = _validationService.ValidateQuerySetting(queryParam);

            //// Assert
            Assert.IsFalse(actualResult.IsSuccessful);
            Assert.AreEqual("Account ActorName was disbaled.", actualResult.Message);
        }

        [Test]
        public void ValidateQuerySetting_Always_CallGetCustomerLevel()
        {
            //// Arrange
            QuerySettingParam queryParam = new QuerySettingParam(
                customerId: 1,
                customerLevel: UserLevel.Member,
                productId: 1,
                uplineLevel: UserLevel.Agent);
            _settingContext.Actor.Level = UserLevel.Agent;

            AssumeActorStatusIsEnabled();

            //// Act
            _validationService.ValidateQuerySetting(queryParam);

            //// Assert
            _dynamicRepository.Received().Get<byte>(Arg.Is<CustomerLevelQueryCriteria>(criteria => criteria.CustId == queryParam.CustomerId));
        }

        private static object[] ValidateQuerySetting_ActorIsNotCompany_AlwaysCallValidateBelongIn_Cases =
        {
            new object[] {UserLevel.Super, UserLevel.Super},
            new object[] {UserLevel.Master, UserLevel.Master},
            new object[] {UserLevel.Agent, UserLevel.Agent}
        };

        [Test, TestCaseSource("ValidateQuerySetting_ActorIsNotCompany_AlwaysCallValidateBelongIn_Cases")]
        public void ValidateQuerySetting_ActorIsNotCompany_AlwaysCallValidateBelongIn(
            UserLevel actorLevel,
            UserLevel uplineLevel)
        {
            //// Arrange
            QuerySettingParam queryParam = new QuerySettingParam(
                customerId: 1,
                customerLevel: UserLevel.Member,
                productId: 1,
                uplineLevel: uplineLevel);
            _settingContext.Actor.Level = actorLevel;

            AssumeActorStatusIsEnabled();
            AssumeCustomerLevelIsEqualWithValueStoreInDatabase(queryParam.CustomerLevel);

            //// Act
            _validationService.ValidateQuerySetting(queryParam);

            //// Assert
            _validationDataAccess.Received().ValidateBelonging(_settingContext.Actor.Id, queryParam.CustomerId);
        }

        [Test]
        public void ValidateQuerySetting_ActorIsCompany_DontCallValidateBelongIn()
        {
            //// Arrange
            QuerySettingParam queryParam = new QuerySettingParam(
                customerId: 1,
                customerLevel: UserLevel.Member,
                productId: 1,
                uplineLevel: UserLevel.Agent);
            _settingContext.Actor.Level = UserLevel.Company;

            AssumeActorStatusIsEnabled();
            AssumeCustomerLevelIsEqualWithValueStoreInDatabase(queryParam.CustomerLevel);

            //// Act
            _validationService.ValidateQuerySetting(queryParam);

            //// Assert
            _validationDataAccess.DidNotReceive().ValidateBelonging(_settingContext.Actor.Id, queryParam.CustomerId);
        }

        [Test, TestCaseSource("ValidateQuerySetting_EdittingMemberAndActorLevelIsDifferentWithUplineLevel_Cases")]
        public void ValidateQuerySetting_EdittingMemberAndActorLevelIsDifferentWithUplineLevel_ReturnAccessDeniedResult(
            UserLevel actorLevel,
            UserLevel uplineLevel)
        {
            //// Arrange
            QuerySettingParam queryParam = new QuerySettingParam(
                customerId: 1,
                customerLevel: UserLevel.Member,
                productId: 1,
                uplineLevel: uplineLevel);
            _settingContext.Actor.Level = actorLevel;

            AssumeActorStatusIsEnabled();
            AssumeCustomerLevelIsEqualWithValueStoreInDatabase(queryParam.CustomerLevel);
            AssumeCutomerIsDownlineOfActor();

            //// Act
            var actualResult = _validationService.ValidateQuerySetting(queryParam);

            //// Assert
            Assert.AreEqual(Result.AccessDenied.Code, actualResult.Code);
        }

        private static object[] ValidateQuerySetting_EdittingMemberAndActorLevelIsDifferentWithUplineLevel_Cases =
        {
            new object[] {UserLevel.Super, UserLevel.Master},
            new object[] {UserLevel.Master, UserLevel.Agent},
            new object[] {UserLevel.Agent, UserLevel.Super}
        };

        [Test, TestCaseSource("ValidateQuerySetting_CompanyEditMemberAndUplineLevelHasValue_Cases")]
        public void ValidateQuerySetting_CompanyEditMemberAndUplineLevelHasValue_ReturnSuccessResult(
            UserLevel actorLevel,
            UserLevel uplineLevel)
        {
            //// Arrange
            QuerySettingParam queryParam = new QuerySettingParam(
                customerId: 1,
                customerLevel: UserLevel.Member,
                productId: 1,
                uplineLevel: uplineLevel);
            _settingContext.Actor.Level = actorLevel;

            AssumeActorStatusIsEnabled();
            AssumeCustomerLevelIsEqualWithValueStoreInDatabase(queryParam.CustomerLevel);
            AssumeCutomerIsDownlineOfActor();

            //// Act
            var actualResult = _validationService.ValidateQuerySetting(queryParam);

            //// Assert
            Assert.IsTrue(actualResult.IsSuccessful);
        }

        private static object[] ValidateQuerySetting_CompanyEditMemberAndUplineLevelHasValue_Cases =
        {
            new object[] {UserLevel.Company, UserLevel.Super},
            new object[] {UserLevel.Company, UserLevel.Master},
            new object[] {UserLevel.Company, UserLevel.Agent},
             new object[] {UserLevel.Super, UserLevel.Super},
            new object[] {UserLevel.Master, UserLevel.Master},
            new object[] {UserLevel.Agent, UserLevel.Agent}
        };

        [Test, TestCaseSource("ValidateQuerySetting_CompanyEditsCustomerIsNotMemberAndUplineLevelIsNull_Cases")]
        public void ValidateQuerySetting_CompanyEditsCustomerIsNotMemberAndUplineLevelIsNull_ReturnSuccessResult(UserLevel customerLevel)
        {
            //// Arrange
            QuerySettingParam queryParam = new QuerySettingParam(
                customerId: 1,
                customerLevel: customerLevel,
                productId: 1);
            _settingContext.Actor.Level = UserLevel.Company;

            AssumeActorStatusIsEnabled();
            AssumeCustomerLevelIsEqualWithValueStoreInDatabase(queryParam.CustomerLevel);
            AssumeCutomerIsDownlineOfActor();

            //// Act
            var actualResult = _validationService.ValidateQuerySetting(queryParam);

            //// Assert
            Assert.IsTrue(actualResult.IsSuccessful);
        }

        private static UserLevel[] ValidateQuerySetting_CompanyEditsCustomerIsNotMemberAndUplineLevelIsNull_Cases = new UserLevel[] 
        {
            UserLevel.Super, UserLevel.Master, UserLevel.Agent
        };
    }
}