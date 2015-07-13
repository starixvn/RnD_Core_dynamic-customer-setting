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

        [Test, TestCaseSource("ValidateUpdatingSetting_ActorHasPermission_Cases")]
        public void ValidateUpdatingSetting_ActorHasPermission_ReturnSuccessResult(
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

        static object[] ValidateUpdatingSetting_ActorHasPermission_Cases = 
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

       [Test, TestCaseSource("ValidateUpdatingSetting_ActorHasNoPermission_Cases")]
        public void ValidateUpdatingSetting_ActorHasNoPermission_ReturnFailedResult(
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


        static object[] ValidateUpdatingSetting_ActorHasNoPermission_Cases = 
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

        [Test]
        public void ValidateUpdatingSetting_CompanyPerformUpdatingActionInAgencySystem_ReturnAccessDeniedResult()
        {
            //// Arrange
            var updatingParam = InstanceFactory.CreateUpdatingSettingParam();
            _settingContext.Actor.Level = UserLevel.Company;
            AssumeAllValidationsExecutedOnDatabaseAreSuccessful();
            AssumeCustomerSettingTypesAreValid(updatingParam.Setting.SettingTypes);

            var validationServiceStub = Substitute.ForPartsOf<ValidationService>(_settingContext, _validationDataAccess, _productService, _logger);
            validationServiceStub.GetCurrentSystemId().Returns((byte)SystemType.Agency);

            //// Act
            var actualResult = validationServiceStub.ValidateUpdatingSetting(updatingParam);

            //// Assert
            Assert.AreEqual(Result.AccessDenied.Code, actualResult.Code);
        }

        [TestCase((byte)SystemType.Accounting)]
        [TestCase((byte)SystemType.ExternalAccounting)]
        public void ValidateUpdatingSetting_CompanyPerformUpdatingActionInSystemIsNotAgency_ReturnSuccessResult(byte systemId)
        {
            //// Arrange
            var updatingParam = InstanceFactory.CreateUpdatingSettingParam();
            _settingContext.Actor.Level = UserLevel.Company;
            AssumeAllValidationsExecutedOnDatabaseAreSuccessful();
            AssumeCustomerSettingTypesAreValid(updatingParam.Setting.SettingTypes);

            var validationServiceStub = Substitute.ForPartsOf<ValidationService>(_settingContext, _validationDataAccess, _productService, _logger);
            validationServiceStub.GetCurrentSystemId().Returns(systemId);

            //// Act
            var actualResult = validationServiceStub.ValidateUpdatingSetting(updatingParam);

            //// Assert
            Assert.IsTrue(actualResult.IsSuccessful);
        }
    }
}
