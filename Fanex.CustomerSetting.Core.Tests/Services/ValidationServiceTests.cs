namespace Fanex.CustomerSetting.Core.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Core.DataAccesses;
    using Fanex.CustomerSetting.Core.Dtos.Params;
    using Fanex.CustomerSetting.Core.Dtos.Results;
    using Fanex.CustomerSetting.Core.Enumerations;
    using Fanex.CustomerSetting.Core.Models;
    using Fanex.CustomerSetting.Core.Services;
    using Fanex.CustomerSetting.Core.Services.Impl;
    using Fanex.CustomerSetting.Core.SPObjects.Commands.Validation;
    using Fanex.CustomerSetting.Core.Tests.Models;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class ValidationServiceTests
    {
        private ISettingContext _settingContext;
        private IValidationService _validationService;
        private IValidationDataAccess _validationDataAccess;
        private IProductService _productService;

        [SetUp]
        public void Setup()
        {
            _validationDataAccess = Substitute.For<IValidationDataAccess>();
            _settingContext = new SettingContextStub();
            _productService = Substitute.For<IProductService>();
            _validationService = new ValidationService(_settingContext, _validationDataAccess, _productService);
        }

        #region Validatie Setting Query Tests

        [Test]
        public void ValidateSettingQuery_SettingQueryIsNull_ThrownArgumentNullException()
        {
            //// Arrange
            SettingQuery settingQuery = null;

            //// Act
            Assert.Throws<ArgumentNullException>(
                new TestDelegate(
                    () => _validationService.ValidateSettingQuery(settingQuery)));
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void ValidateSettingQuery_CustomerIdIsNegativeOrZero_ThrowArgumentOutOfRangeException(int customerId)
        {
            //// Arrange
            SettingQuery settingQuery = new SettingQuery
            {
                CustomerId = customerId
            };

            //// Act
            Assert.Throws<ArgumentOutOfRangeException>(
                new TestDelegate(
                    () => _validationService.ValidateSettingQuery(settingQuery)));
        }

        [TestCase(CustomerLevel.Super)]
        [TestCase(CustomerLevel.Master)]
        [TestCase(CustomerLevel.Agent)]
        [TestCase(CustomerLevel.Member)]
        public void ValidateSettingQuery_ActorIsCompany_CanViewAllCustomerSetting(CustomerLevel customerLevel)
        {
            //// Arrange
            SettingQuery settingQuery = new SettingQuery
            {
                CustomerId = 1,
                CustomerLevel = customerLevel
            };
            _settingContext.Actor.Level = UserLevel.Company;
            _validationDataAccess.ValidatePermission(Arg.Any<PermissionValidationCommand>()).Returns(Result.Success);

            //// Act
            var actualResult = _validationService.ValidateSettingQuery(settingQuery);

            //// Assert
            Assert.IsTrue(actualResult.IsSuccessful);
        }

        [TestCase(CustomerLevel.Super)]
        [TestCase(CustomerLevel.Agent)]
        public void ValidateSettingQuery_ActorIsSuper_CannotViewOtherSuperAndAgentSetting(CustomerLevel customerLevel)
        {
            //// Arrange
            SettingQuery settingQuery = new SettingQuery
            {
                CustomerId = 1,
                CustomerLevel = customerLevel
            };
            _settingContext.Actor.Level = UserLevel.Super;

            //// Act
            var actualResult = _validationService.ValidateSettingQuery(settingQuery);

            //// Assert
            Assert.IsFalse(actualResult.IsSuccessful);
        }

        [TestCase(CustomerLevel.Master)]
        [TestCase(CustomerLevel.Member)]
        public void ValidateSettingQuery_ActorIsSuper_CanViewDownlineSettingExceptAgent(CustomerLevel downlineLevel)
        {
            //// Arrange
            SettingQuery settingQuery = new SettingQuery
            {
                CustomerId = 1,
                CustomerLevel = downlineLevel
            };
            _settingContext.Actor.Level = UserLevel.Super;
            _validationDataAccess.ValidatePermission(Arg.Any<PermissionValidationCommand>()).Returns(Result.Success);

            //// Act
            var actualResult = _validationService.ValidateSettingQuery(settingQuery);

            //// Assert
            Assert.IsTrue(actualResult.IsSuccessful);
        }

        [TestCase(CustomerLevel.Super)]
        [TestCase(CustomerLevel.Master)]
        public void ValidateSettingQuery_ActorIsMaster_CannotViewOtherMasterAndSuperSetting(CustomerLevel customerLevel)
        {
            //// Arrange
            SettingQuery settingQuery = new SettingQuery
            {
                CustomerId = 1,
                CustomerLevel = customerLevel
            };
            _settingContext.Actor.Level = UserLevel.Master;

            //// Act
            var actualResult = _validationService.ValidateSettingQuery(settingQuery);

            //// Assert
            Assert.IsFalse(actualResult.IsSuccessful);
        }

        [TestCase(CustomerLevel.Agent)]
        [TestCase(CustomerLevel.Member)]
        public void ValidateSettingQuery_ActorIsMaster_CanViewDownlineSetting(CustomerLevel downlineLevel)
        {
            //// Arrange
            SettingQuery settingQuery = new SettingQuery
            {
                CustomerId = 1,
                CustomerLevel = downlineLevel
            };
            _settingContext.Actor.Level = UserLevel.Master;
            _validationDataAccess.ValidatePermission(Arg.Any<PermissionValidationCommand>()).Returns(Result.Success);

            //// Act
            var actualResult = _validationService.ValidateSettingQuery(settingQuery);

            //// Assert
            Assert.IsTrue(actualResult.IsSuccessful);
        }

        [TestCase(CustomerLevel.Super)]
        [TestCase(CustomerLevel.Master)]
        [TestCase(CustomerLevel.Agent)]
        public void ValidateSettingQuery_ActorIsAgent_CannotViewOtherAgentAndUplineSetting(CustomerLevel customerLevel)
        {
            //// Arrange
            SettingQuery settingQuery = new SettingQuery
            {
                CustomerId = 1,
                CustomerLevel = customerLevel
            };
            _settingContext.Actor.Level = UserLevel.Agent;

            //// Act
            var actualResult = _validationService.ValidateSettingQuery(settingQuery);

            //// Assert
            Assert.IsFalse(actualResult.IsSuccessful);
        }

        [Test]
        public void ValidateSettingQuery_ActorIsAgent_CanViewMemberSetting()
        {
            //// Arrange
            SettingQuery settingQuery = new SettingQuery
            {
                CustomerId = 1,
                CustomerLevel = CustomerLevel.Member
            };
            _settingContext.Actor.Level = UserLevel.Agent;
            _validationDataAccess.ValidatePermission(Arg.Any<PermissionValidationCommand>()).Returns(Result.Success);

            //// Act
            var actualResult = _validationService.ValidateSettingQuery(settingQuery);

            //// Assert
            Assert.IsTrue(actualResult.IsSuccessful);
        }

        [Test]
        public void ValidateSettingQuery_ExecuteValidatePerrmissionMethod()
        {
            //// Arrange
            SettingQuery settingQuery = new SettingQuery
            {
                CustomerId = 1,
                CustomerLevel = CustomerLevel.Member
            };
            _settingContext.Actor.Level = UserLevel.Agent;

            //// Act
            _validationService.ValidateSettingQuery(settingQuery);

            //// Assert
            _validationDataAccess.Received().ValidatePermission(
                Arg.Is<PermissionValidationCommand>(
                command => command.ActorId == _settingContext.Actor.Id
                    && command.CustomerId == settingQuery.CustomerId
                    && command.CustomerLevelId == (int)settingQuery.CustomerLevel));
        }

        #endregion Validatie Setting Query Tests

        #region validate Setting Command Tests

        [Test]
        public void ValidateSettingCommand_SettingCommandIsNull_ThrownArgumentNullException()
        {
            //// Arrange
            SettingCommand settingCommand = null;

            //// Act
            Assert.Throws<ArgumentNullException>(
                new TestDelegate(
                    () => _validationService.ValidateSettingCommand(settingCommand)));
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void ValidateSettingCommand_CustomerIdIsNegativeOrZero_ThrowArgumentOutOfRangeException(int customerId)
        {
            //// Arrange
            SettingCommand settingCommand = FakeObjectManager.CreateSettingCommandFakeObject(customerId);

            //// Act
            Assert.Throws<ArgumentOutOfRangeException>(
                new TestDelegate(
                    () => _validationService.ValidateSettingCommand(settingCommand)));
        }

        [TestCase(CustomerLevel.Super)]
        [TestCase(CustomerLevel.Master)]
        [TestCase(CustomerLevel.Agent)]
        [TestCase(CustomerLevel.Member)]
        public void ValidateSettingCommand_ActorIsCompany_CanUpdateAllCustomerSetting(CustomerLevel customerLevel)
        {
            //// Arrange
            var settingCommand = FakeObjectManager.CreateSettingCommandFakeObject(1, customerLevel);
            _settingContext.Actor.Level = UserLevel.Company;
            _validationDataAccess.ValidatePermission(Arg.Any<PermissionValidationCommand>()).Returns(Result.Success);
            _productService.GetProductSettingTypes(settingCommand.Setting.Product.Id).Returns(settingCommand.Setting.SettingTypes);

            //// Act
            var actualResult = _validationService.ValidateSettingCommand(settingCommand);

            //// Assert
            Assert.IsTrue(actualResult.IsSuccessful);
        }

        [TestCase(CustomerLevel.Super)]
        [TestCase(CustomerLevel.Agent)]
        public void ValidateSettingCommand_ActorIsSuper_CannotUpdateOtherSuperAndAgentSetting(CustomerLevel customerLevel)
        {
            //// Arrange
            var settingCommand = FakeObjectManager.CreateSettingCommandFakeObject(1, customerLevel);
            _settingContext.Actor.Level = UserLevel.Super;

            //// Act
            var actualResult = _validationService.ValidateSettingCommand(settingCommand);

            //// Assert
            Assert.IsFalse(actualResult.IsSuccessful);
        }

        [TestCase(CustomerLevel.Master)]
        [TestCase(CustomerLevel.Member)]
        public void ValidateSettingCommand_ActorIsSuper_CanUpdateDownlineSettingExceptAgent(CustomerLevel downlineLevel)
        {
            //// Arrange
            var settingCommand = FakeObjectManager.CreateSettingCommandFakeObject(1, downlineLevel);
            _settingContext.Actor.Level = UserLevel.Super;
            _validationDataAccess.ValidatePermission(Arg.Any<PermissionValidationCommand>()).Returns(Result.Success);
            _productService.GetProductSettingTypes(settingCommand.Setting.Product.Id).Returns(settingCommand.Setting.SettingTypes);

            //// Act
            var actualResult = _validationService.ValidateSettingCommand(settingCommand);

            //// Assert
            Assert.IsTrue(actualResult.IsSuccessful);
        }

        [TestCase(CustomerLevel.Super)]
        [TestCase(CustomerLevel.Master)]
        public void ValidateSettingCommand_ActorIsMaster_CannotUpdateOtherMasterAndSuperSetting(CustomerLevel customerLevel)
        {
            //// Arrange
            var settingCommand = FakeObjectManager.CreateSettingCommandFakeObject(1, customerLevel);
            _settingContext.Actor.Level = UserLevel.Master;

            //// Act
            var actualResult = _validationService.ValidateSettingCommand(settingCommand);

            //// Assert
            Assert.IsFalse(actualResult.IsSuccessful);
        }

        [TestCase(CustomerLevel.Agent)]
        [TestCase(CustomerLevel.Member)]
        public void ValidateSettingCommand_ActorIsMaster_CanUpdateDownlineSetting(CustomerLevel downlineLevel)
        {
            //// Arrange
            var settingCommand = FakeObjectManager.CreateSettingCommandFakeObject(1, downlineLevel);
            _settingContext.Actor.Level = UserLevel.Master;
            _validationDataAccess.ValidatePermission(Arg.Any<PermissionValidationCommand>()).Returns(Result.Success);
            _productService.GetProductSettingTypes(settingCommand.Setting.Product.Id).Returns(settingCommand.Setting.SettingTypes);

            //// Act
            var actualResult = _validationService.ValidateSettingCommand(settingCommand);

            //// Assert
            Assert.IsTrue(actualResult.IsSuccessful);
        }

        [TestCase(CustomerLevel.Super)]
        [TestCase(CustomerLevel.Master)]
        [TestCase(CustomerLevel.Agent)]
        public void ValidateSettingCommand_ActorIsAgent_CannotUpdateOtherAgentAndUplineSetting(CustomerLevel customerLevel)
        {
            //// Arrange
            var settingCommand = FakeObjectManager.CreateSettingCommandFakeObject(1, customerLevel);
            _settingContext.Actor.Level = UserLevel.Agent;

            //// Act
            var actualResult = _validationService.ValidateSettingCommand(settingCommand);

            //// Assert
            Assert.IsFalse(actualResult.IsSuccessful);
        }

        [Test]
        public void ValidateSettingCommand_ActorIsAgent_CanUpdateMemberSetting()
        {
            //// Arrange
            var settingCommand = FakeObjectManager.CreateSettingCommandFakeObject(1, CustomerLevel.Member);
            _settingContext.Actor.Level = UserLevel.Agent;
            _validationDataAccess.ValidatePermission(Arg.Any<PermissionValidationCommand>()).Returns(Result.Success);
            _productService.GetProductSettingTypes(settingCommand.Setting.Product.Id).Returns(settingCommand.Setting.SettingTypes);

            //// Act
            var actualResult = _validationService.ValidateSettingCommand(settingCommand);

            //// Assert
            Assert.IsTrue(actualResult.IsSuccessful);
        }

        [Test]
        public void ValidateSettingCommand_CustomerSettingTypeDoesNotMatchWithProductSettingType_ReturnFailedResult()
        {
            //// Arrange
            var settingCommand = FakeObjectManager.CreateSettingCommandFakeObject();
            _validationDataAccess.ValidatePermission(Arg.Any<PermissionValidationCommand>()).Returns(Result.Success);
            settingCommand.Setting.SettingTypes = new List<SettingType>
            {
                FakeObjectManager.CreateSettingTypeFakeObject(1)
            };
            _productService.GetProductSettingTypes(settingCommand.Setting.Product.Id)
                .Returns(new List<SettingType>
                {
                    FakeObjectManager.CreateSettingTypeFakeObject(2)
                });

            //// Act
            var actualResult = _validationService.ValidateSettingCommand(settingCommand);

            //// Assert
            Assert.IsFalse(actualResult.IsSuccessful);
        }

        [Test]
        public void ValidateSettingCommand_ExecuteValidateMethodOnEachSettingItem()
        {
            //// Arrange
            var settingCommand = FakeObjectManager.CreateSettingCommandFakeObject();
            var settingItemMock = Substitute.For<ISettingItem>();
            settingCommand.Setting.SettingTypes = new List<SettingType>
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
            _validationDataAccess.ValidatePermission(Arg.Any<PermissionValidationCommand>()).Returns(Result.Success);
            _productService.GetProductSettingTypes(settingCommand.Setting.Product.Id).Returns(settingCommand.Setting.SettingTypes);

            //// Act
            _validationService.ValidateSettingCommand(settingCommand);

            //// Assert
            settingItemMock.Received().Validate();
        }

        #endregion validate Setting Command Tests
    }
}