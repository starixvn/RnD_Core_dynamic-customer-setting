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
    public class ValidationServiceValidateUpdatingMultipleSettingTests
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
            _validationDataAccess.ValidateLevel(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<byte>()).Returns(Result.Success);
        }

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
    }
}