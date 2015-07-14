namespace Fanex.CustomerSetting.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
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
    public class ValidationServiceTestsValidateUpdatingMultipleSetting
    {
        private ISettingContext _settingContext;
        private IValidationService _validationService;
        private IValidationDataAccess _validationDataAccess;
        private IDynamicRepository _dynamicRepository;
        private ILoggingService _loggingService;
        private IProductService _productService;

        [SetUp]
        public void Setup()
        {
            _settingContext = new SettingContextStub();
            _productService = Substitute.For<IProductService>();
            _loggingService = Substitute.For<ILoggingService>();
            _validationDataAccess = Substitute.For<IValidationDataAccess>();
            _dynamicRepository = Substitute.For<IDynamicRepository>();

            _validationService = new ValidationService(
                _settingContext,
                _productService,
                _loggingService,
                _validationDataAccess,
                _dynamicRepository);
        }

        private void AssumeAllValidationsExecutedOnDatabaseAreSuccessful()
        {
            _validationDataAccess.ValidateBelongIn(Arg.Any<int>(), Arg.Any<int>()).Returns(Result.Success);
            _validationDataAccess.ValidatePermissionMultiple(Arg.Any<int>(), Arg.Any<string>()).Returns(Result.Success);
        }

        private void AssumeActorStatusIsEnabled()
        {
            _dynamicRepository.Get<bool>(Arg.Any<ActorStatusQueryCriteria>()).Returns(true);
        }

        private void AssumeCustomerLevelIsEqualWithValueStoreInDatabase(CustomerLevel customerLevel)
        {
            _dynamicRepository.Get<byte>(Arg.Any<CustomerLevelQueryCriteria>()).Returns((byte)customerLevel);
        }

        private void AssumeCustomerSettingTypeAreValid(IEnumerable<SettingType> dummySettingTypes)
        {
            _productService.GetProductSettingTypes(Arg.Any<byte>()).Returns(dummySettingTypes);
        }

        [Test]
        public void ValidateUpdatingMultipleSetting_CustomerIsMember_CallValidatePermissionMultiple()
        {
            //// Arrange
            var updatingMultipleSettingParam = InstanceFactory.CreateUpdatingMultipleSettingParam(1, CustomerLevel.Member, 1, CustomerLevel.Agent);
            AssumeAllValidationsExecutedOnDatabaseAreSuccessful();
            AssumeActorStatusIsEnabled();
            AssumeCustomerLevelIsEqualWithValueStoreInDatabase(updatingMultipleSettingParam.Setting.CustomerLevel);
            AssumeCustomerSettingTypeAreValid(updatingMultipleSettingParam.Setting.SettingTypes);

            //// Act
            _validationService.ValidateUpdatingMultipleSetting(updatingMultipleSettingParam);

            //// Assert
            _validationDataAccess.Received().ValidatePermissionMultiple(
                Arg.Is<int>(actorId => actorId == _settingContext.Actor.Id),
                Arg.Is<string>(memberIds => memberIds == string.Join(",", updatingMultipleSettingParam.CustomerIds)));
        }

        [Test]
        public void ValidateUpdatingMultipleSetting_CustomerIsNotMember_DontCallValidatePermissionMultiple()
        {
            //// Arrange
            var updatingMultipleSettingParam = InstanceFactory.CreateUpdatingMultipleSettingParam();
            AssumeAllValidationsExecutedOnDatabaseAreSuccessful();
            AssumeActorStatusIsEnabled();
            AssumeCustomerLevelIsEqualWithValueStoreInDatabase(updatingMultipleSettingParam.Setting.CustomerLevel);
            AssumeCustomerSettingTypeAreValid(updatingMultipleSettingParam.Setting.SettingTypes);

            //// Act
            _validationService.ValidateUpdatingMultipleSetting(updatingMultipleSettingParam);

            //// Assert
            _validationDataAccess.DidNotReceive().ValidatePermissionMultiple(Arg.Any<int>(), Arg.Any<string>());
        }

        [Test, TestCaseSource("ValidateUpdatingMultipleSetting_ActorHasPermission_Cases")]
        public void ValidateUpdatingMultipleSetting_ActorHasPermission_ReturnSuccessResult(
            UserLevel actorLevel,
            CustomerLevel customerLevel, 
            CustomerLevel? uplineLevel)
        {
            //// Arrange
            var updatingMultipleSettingParam = InstanceFactory.CreateUpdatingMultipleSettingParam(1, customerLevel, 1, uplineLevel);
            _settingContext.Actor.Level = actorLevel;

            AssumeAllValidationsExecutedOnDatabaseAreSuccessful();
            AssumeActorStatusIsEnabled();
            AssumeCustomerLevelIsEqualWithValueStoreInDatabase(updatingMultipleSettingParam.Setting.CustomerLevel);
            AssumeCustomerSettingTypeAreValid(updatingMultipleSettingParam.Setting.SettingTypes);

            //// Act
            var actualResult = _validationService.ValidateUpdatingMultipleSetting(updatingMultipleSettingParam);

            //// Assert
            Assert.IsTrue(actualResult.IsSuccessful);
        }

        private static object[] ValidateUpdatingMultipleSetting_ActorHasPermission_Cases = 
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

        [Test, TestCaseSource("ValidateUpdatingMultipleSetting_ActorDoesNotHavePermission_Cases")]
        public void ValidateUpdatingMultipleSetting_ActorDoesNotHavePermission_ReturnFailedResult(
            UserLevel actorLevel,
            CustomerLevel customerLevel)
        {
            //// Arrange
            var updatingMultipleSettingParam = InstanceFactory.CreateUpdatingMultipleSettingParam(1, customerLevel);
            _settingContext.Actor.Level = actorLevel;

            //// Act
            var actualResult = _validationService.ValidateUpdatingMultipleSetting(updatingMultipleSettingParam);

            //// Assert
            Assert.IsFalse(actualResult.IsSuccessful);
        }

        private static object[] ValidateUpdatingMultipleSetting_ActorDoesNotHavePermission_Cases = 
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
        public void UpdateMultipleCustomerSetting_RaisedException_CallLogUpdatingMultipleCustomersSettingTypeError()
        {
            //// Arrange
            var updatingMultipleSettingParam = InstanceFactory.CreateUpdatingMultipleSettingParam(1, CustomerLevel.Member, 1, CustomerLevel.Agent);
            AssumeActorStatusIsEnabled();
            AssumeCustomerLevelIsEqualWithValueStoreInDatabase(updatingMultipleSettingParam.Setting.CustomerLevel);
            AssumeCustomerSettingTypeAreValid(updatingMultipleSettingParam.Setting.SettingTypes);

            //// Simulate throw exception
            var expectedException = new Exception("Exception");
            _validationDataAccess.ValidatePermissionMultiple(Arg.Any<int>(), Arg.Any<string>()).Returns(x => { throw expectedException; });

            //// Act
            _validationService.ValidateUpdatingMultipleSetting(updatingMultipleSettingParam);

            //// Assert
            _loggingService.Received().LogValidatingUpdatingMultipleError(
                Arg.Is<IEnumerable<int>>(customerIds => string.Join(",", customerIds) == string.Join(",", updatingMultipleSettingParam.CustomerIds)),
                Arg.Is<CustomerLevel?>(uplineLevel => uplineLevel == updatingMultipleSettingParam.UplineLevel),
                Arg.Is<byte>(productId => productId == updatingMultipleSettingParam.Setting.ProductId),
                Arg.Any<Exception>());
        }
    }
}