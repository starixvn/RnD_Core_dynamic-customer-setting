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
    public class ValidationServiceTestsValidateUpdatingSetting
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
        public void ValidateUpdatingSetting_Always_CallValidateActorEnabled()
        {
            //// Arrange
            var updatingParam = InstanceFactory.CreateUpdatingSettingParam();

            //// Act
            _validationService.ValidateUpdatingSetting(updatingParam);

            //// Assert
            _validationDataAccess.Received().ValidateActorEnabled(_settingContext.Actor.Id);
        }

        [Test]
        public void ValidateUpdatingSetting_Always_CallGetProductSettingTypes()
        {
            //// Arrange
            var updatingParam = InstanceFactory.CreateUpdatingSettingParam();
            AssumeAllValidationsExecutedOnDatabaseAreSuccessful();

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
            var updatingParam = InstanceFactory.CreateUpdatingSettingParam(1, customerLevel, 1, uplineLevel);
            _settingContext.Actor.Level = actorLevel;
            AssumeAllValidationsExecutedOnDatabaseAreSuccessful();
            AssumeCustomerSettingTypesAreValid(updatingParam.Setting.SettingTypes);

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
        public void ValidateUpdatingSetting_ActorHasNoPermission_CannotUpdateCustomerSetting(
            UserLevel actorLevel,
            CustomerLevel customerLevel)
        {
            //// Arrange
            var updatingParam = InstanceFactory.CreateUpdatingSettingParam(1, customerLevel);
            _settingContext.Actor.Level = actorLevel;
            AssumeAllValidationsExecutedOnDatabaseAreSuccessful();

            //// Act
            var actualResult = _validationService.ValidateUpdatingSetting(updatingParam);

            //// Assert
            Assert.IsFalse(actualResult.IsSuccessful);
        }

        [Test]
        public void ValidateUpdatingSetting_CustomerSettingTypesDontMatchWithProductSettingTypes_ReturnFailedResult()
        {
            //// Arrange
            var updatingParam = InstanceFactory.CreateUpdatingSettingParam();
            AssumeAllValidationsExecutedOnDatabaseAreSuccessful();
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
            AssumeAllValidationsExecutedOnDatabaseAreSuccessful();
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
            AssumeAllValidationsExecutedOnDatabaseAreSuccessful();
            _productService.GetProductSettingTypes(updatingParam.Setting.ProductId).Returns(updatingParam.Setting.SettingTypes);

            //// Act
            _validationService.ValidateUpdatingSetting(updatingParam);

            //// Assert
            settingItemMock.Received().Validate();
        }
    }
}
