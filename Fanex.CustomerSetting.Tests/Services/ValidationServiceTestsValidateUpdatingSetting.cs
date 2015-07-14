namespace Fanex.CustomerSetting.Tests.Services
{
    using System.Collections.Generic;
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
    public class ValidationServiceTestsValidateUpdatingSetting
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
            _validationDataAccess = Substitute.For<IValidationDataAccess>();
            _dynamicRepository = Substitute.For<IDynamicRepository>();
            _productService = Substitute.For<IProductService>();
            _logger = Substitute.For<ILogger>();

            _validationService = new ValidationService(
                _settingContext,
                _validationDataAccess,
                _dynamicRepository,
                _productService,
                _logger);
        }

        private void AssumeCutomerIsDownlineOfActor()
        {
            _validationDataAccess.ValidateBelongIn(Arg.Any<int>(), Arg.Any<int>()).Returns(Result.Success);
        }

        private void AssumeActorStatusIsEnabled()
        {
            _dynamicRepository.Get<bool>(Arg.Any<ActorStatusQueryCriteria>()).Returns(true);
        }

        private void AssumeCustomerLevelIsEqualWithValueStoreInDatabase(CustomerLevel customerLevel)
        {
            _dynamicRepository.Get<byte>(Arg.Any<CustomerLevelQueryCriteria>()).Returns((byte)customerLevel);
        }

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

        [Test]
        public void ValidateUpdatingSetting_Always_CallValidateBelongIn()
        {
            //// Arrange
            var updatingParam = InstanceFactory.CreateUpdatingSettingParam();
            _settingContext.Actor.Level = UserLevel.Super;

            AssumeActorStatusIsEnabled();
            AssumeCustomerLevelIsEqualWithValueStoreInDatabase(updatingParam.Setting.CustomerLevel);

            //// Act
            _validationService.ValidateUpdatingSetting(updatingParam);

            //// Assert
            _validationDataAccess.Received().ValidateBelongIn(_settingContext.Actor.Id, updatingParam.Setting.CustomerId);
        }

        [Test]
        public void ValidateUpdatingSetting_Always_CallGetProductSettingTypes()
        {
            //// Arrange
            var updatingParam = InstanceFactory.CreateUpdatingSettingParam();

            AssumeActorStatusIsEnabled();
            AssumeCustomerLevelIsEqualWithValueStoreInDatabase(updatingParam.Setting.CustomerLevel);
            AssumeCutomerIsDownlineOfActor();

            //// Act
            _validationService.ValidateUpdatingSetting(updatingParam);

            //// Assert
            _productService.Received().GetProductSettingTypes(Arg.Is<byte>(
                productId => productId == updatingParam.Setting.ProductId));
        }

        private void AssumeCustomerSettingTypesAreValid(IEnumerable<SettingType> dummySettingTypes)
        {
            _productService.GetProductSettingTypes(Arg.Any<byte>()).Returns(dummySettingTypes);
        }

        [Test, TestCaseSource("ValidateUpdatingSetting_ActorHasPermission_Cases")]
        public void ValidateUpdatingSetting_ActorHasPermission_ReturnSuccessResult(
            UserLevel actorLevel,
            CustomerLevel customerLevel,
            CustomerLevel? uplineLevel)
        {
            //// Arrange
            var updatingParam = InstanceFactory.CreateUpdatingSettingParam(1, customerLevel, 1, uplineLevel);
            _settingContext.Actor.Level = actorLevel;

            AssumeActorStatusIsEnabled();
            AssumeCustomerLevelIsEqualWithValueStoreInDatabase(updatingParam.Setting.CustomerLevel);
            AssumeCutomerIsDownlineOfActor();
            AssumeCustomerSettingTypesAreValid(updatingParam.Setting.SettingTypes);

            //// Act
            var actualResult = _validationService.ValidateUpdatingSetting(updatingParam);

            //// Assert
            Assert.IsTrue(actualResult.IsSuccessful);
        }

        private static object[] ValidateUpdatingSetting_ActorHasPermission_Cases =
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

        [Test, TestCaseSource("ValidateUpdatingSetting_ActorDoesNotHavePermission_Cases")]
        public void ValidateUpdatingSetting_ActorDoesNotHavePermission_ReturnFailedResult(
            UserLevel actorLevel,
            CustomerLevel customerLevel)
        {
            //// Arrange
            var updatingParam = InstanceFactory.CreateUpdatingSettingParam(1, customerLevel);
            _settingContext.Actor.Level = actorLevel;

            AssumeActorStatusIsEnabled();
            AssumeCustomerLevelIsEqualWithValueStoreInDatabase(updatingParam.Setting.CustomerLevel);
            AssumeCutomerIsDownlineOfActor();

            //// Act
            var actualResult = _validationService.ValidateUpdatingSetting(updatingParam);

            //// Assert
            Assert.IsFalse(actualResult.IsSuccessful);
        }

        private static object[] ValidateUpdatingSetting_ActorDoesNotHavePermission_Cases =
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
        public void ValidateUpdatingSetting_CustomerSettingTypesDontMatchWithProductSettingTypes_ReturnFailedResult()
        {
            //// Arrange
            var updatingParam = InstanceFactory.CreateUpdatingSettingParam();

            AssumeActorStatusIsEnabled();
            AssumeCustomerLevelIsEqualWithValueStoreInDatabase(updatingParam.Setting.CustomerLevel);
            AssumeCutomerIsDownlineOfActor();

            updatingParam.Setting.SettingTypes = new List<SettingType>
            {
                InstanceFactory.CreateCustomerSettingType(1)
            };
            _productService.GetProductSettingTypes(updatingParam.Setting.ProductId)
                .Returns(new List<SettingType>
                {
                    InstanceFactory.CreateCustomerSettingType(2)
                });

            //// Act
            var actualResult = _validationService.ValidateUpdatingSetting(updatingParam);

            //// Assert
            Assert.IsFalse(actualResult.IsSuccessful);
        }

        [Test]
        public void ValidateUpdatingSetting_CustomerSettingTypesMatchWithProductSettingTypes_ReturnSuccessResult()
        {
            //// Arrange
            var updatingParam = InstanceFactory.CreateUpdatingSettingParam();

            AssumeActorStatusIsEnabled();
            AssumeCustomerLevelIsEqualWithValueStoreInDatabase(updatingParam.Setting.CustomerLevel);
            AssumeCutomerIsDownlineOfActor();

            updatingParam.Setting.SettingTypes = new List<SettingType>
            {
                InstanceFactory.CreateCustomerSettingType(1)
            };
            _productService.GetProductSettingTypes(updatingParam.Setting.ProductId)
                .Returns(new List<SettingType>
                {
                    InstanceFactory.CreateCustomerSettingType(1)
                });

            //// Act
            var actualResult = _validationService.ValidateUpdatingSetting(updatingParam);

            //// Assert
            Assert.IsTrue(actualResult.IsSuccessful);
        }

        [Test]
        public void ValidateUpdatingSetting_Always_CallValidateMethodOnEachSettingItem()
        {
            //// Arrange
            var updatingParam = InstanceFactory.CreateUpdatingSettingParam();
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

            AssumeActorStatusIsEnabled();
            AssumeCustomerLevelIsEqualWithValueStoreInDatabase(updatingParam.Setting.CustomerLevel);
            AssumeCutomerIsDownlineOfActor();
            _productService.GetProductSettingTypes(updatingParam.Setting.ProductId).Returns(updatingParam.Setting.SettingTypes);

            //// Act
            _validationService.ValidateUpdatingSetting(updatingParam);

            //// Assert
            settingItemMock.Received().Validate();
        }

        [Test]
        public void ValidateUpdatingSetting_CompanyPerformsUpdatingActionInAgencySystem_ReturnAccessDeniedResult()
        {
            //// Arrange
            var updatingParam = InstanceFactory.CreateUpdatingSettingParam();
            _settingContext.Actor.Level = UserLevel.Company;

            AssumeActorStatusIsEnabled();
            AssumeCustomerLevelIsEqualWithValueStoreInDatabase(updatingParam.Setting.CustomerLevel);
            AssumeCutomerIsDownlineOfActor();
            AssumeCustomerSettingTypesAreValid(updatingParam.Setting.SettingTypes);

            var validationServiceStub = Substitute.ForPartsOf<ValidationService>(
                _settingContext,
                _validationDataAccess,
                _dynamicRepository,
                _productService,
                _logger);
            validationServiceStub.GetCurrentSystemId().Returns((byte)SystemType.Agency);

            //// Act
            var actualResult = validationServiceStub.ValidateUpdatingSetting(updatingParam);

            //// Assert
            Assert.AreEqual(Result.AccessDenied.Code, actualResult.Code);
        }

        [TestCase((byte)SystemType.Accounting)]
        [TestCase((byte)SystemType.ExternalAccounting)]
        public void ValidateUpdatingSetting_CompanyPerformsUpdatingActionInSystemIsNotAgency_ReturnSuccessResult(byte systemId)
        {
            //// Arrange
            var updatingParam = InstanceFactory.CreateUpdatingSettingParam();
            _settingContext.Actor.Level = UserLevel.Company;

            AssumeActorStatusIsEnabled();
            AssumeCustomerLevelIsEqualWithValueStoreInDatabase(updatingParam.Setting.CustomerLevel);
            AssumeCutomerIsDownlineOfActor();
            AssumeCustomerSettingTypesAreValid(updatingParam.Setting.SettingTypes);

            var validationServiceStub = Substitute.ForPartsOf<ValidationService>(
                _settingContext,
                _validationDataAccess,
                _dynamicRepository,
                _productService,
                _logger);
            validationServiceStub.GetCurrentSystemId().Returns(systemId);

            //// Act
            var actualResult = validationServiceStub.ValidateUpdatingSetting(updatingParam);

            //// Assert
            Assert.IsTrue(actualResult.IsSuccessful);
        }
    }
}