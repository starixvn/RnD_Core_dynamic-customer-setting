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
    public class ValidationServiceTestsValidateUpdatingMultipleSetting
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

        private void AssumeAllValidationsExecutedOnDatabaseAreSuccessful()
        {
            _validationDataAccess.ValidateActorEnabled(Arg.Any<int>()).Returns(Result.Success);
            _validationDataAccess.ValidateBelongIn(Arg.Any<int>(), Arg.Any<int>()).Returns(Result.Success);
            _validationDataAccess.ValidateCustomerAndActorLevel(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<byte>()).Returns(Result.Success);
            _validationDataAccess.ValidatePermissionMultiple(Arg.Any<int>(), Arg.Any<string>()).Returns(Result.Success);
        }

        private void AssumeCustomerSettingTypeAreValid(IEnumerable<SettingType> dummySettingTypes)
        {
            _productService.GetProductSettingTypes(Arg.Any<byte>()).Returns(dummySettingTypes);
        }

        [Test]
        public void ValidateUpdatingMultipleSetting_UpdatingMultipleSettingParamIsNull_ReturnValidationError()
        {
            //// Arrange
            UpdatingMultipleSettingParam parameter = null;

            //// Act
            var actualValidationResult = _validationService.ValidateUpdatingMultipleSetting(parameter);

            //// Assert
            Assert.AreEqual(Result.SystemError.Code, actualValidationResult.Code);
        }

        [Test]
        public void ValidateUpdatingMultipleSetting_CustomerIsMember_CallValidatePermissionMultiple()
        {
            //// Arrange
            var updatingParam = InstanceFactory.CreateUpdatingMultipleSettingParam(1, CustomerLevel.Member, 1, CustomerLevel.Agent);
            AssumeAllValidationsExecutedOnDatabaseAreSuccessful();
            AssumeCustomerSettingTypeAreValid(updatingParam.Setting.SettingTypes);

            //// Act
            _validationService.ValidateUpdatingMultipleSetting(updatingParam);

            //// Assert
            _validationDataAccess.Received().ValidatePermissionMultiple(
                Arg.Is<int>(actorId => actorId == _settingContext.Actor.Id),
                Arg.Is<string>(memberIds => memberIds == string.Join(",", updatingParam.CustomerIds)));
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
            var updatingParam = InstanceFactory.CreateUpdatingMultipleSettingParam(1, customerLevel, 1, uplineLevel);
            _settingContext.Actor.Level = actorLevel;

            AssumeAllValidationsExecutedOnDatabaseAreSuccessful();
            AssumeCustomerSettingTypeAreValid(updatingParam.Setting.SettingTypes);

            //// Act
            var actualResult = _validationService.ValidateUpdatingMultipleSetting(updatingParam);

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
        public void ValidateUpdatingMultipleSetting_ActorHasNoPermissionOnCustomer_CannotUpdateMultipleCustomerSetting(
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
    }
}