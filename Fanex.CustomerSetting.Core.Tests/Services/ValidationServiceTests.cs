namespace Fanex.CustomerSetting.Core.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.CustomerSetting.Core.DataAccesses;
    using Fanex.CustomerSetting.Core.Dtos.Params;
    using Fanex.CustomerSetting.Core.Dtos.Results;
    using Fanex.CustomerSetting.Core.Enumerations;
    using Fanex.CustomerSetting.Core.Models;
    using Fanex.CustomerSetting.Core.Services;
    using Fanex.CustomerSetting.Core.Services.Impl;
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
        public void ValidateSettingQuery_SettingQueryIsNull_ThrowArgumentNullException()
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
            _validationDataAccess.ValidatePermission(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<byte>())
                .Returns(Result.Success);

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
            _validationDataAccess.ValidatePermission(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<byte>())
                .Returns(Result.Success);

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
            _validationDataAccess.ValidatePermission(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<byte>())
                .Returns(Result.Success);

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
            _validationDataAccess.ValidatePermission(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<byte>()).Returns(Result.Success);

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
            _validationDataAccess.Received().ValidatePermission(_settingContext.Actor.Id, settingQuery.CustomerId, (byte)settingQuery.CustomerLevel);
        }

        #endregion Validatie Setting Query Tests

        #region validate Setting Command Tests

        [Test]
        public void ValidateSettingCommand_SettingCommandIsNull_ThrowArgumentNullException()
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

        [Test]
        public void ValidateSettingCommand_ExecuteValidatePermissionOnDataAccess()
        {
            //// Arrange
            var settingCommand = FakeObjectManager.CreateSettingCommandFakeObject();

            //// Act
            _validationService.ValidateSettingCommand(settingCommand);

            //// Assert
            _validationDataAccess.Received().ValidatePermission(
                _settingContext.Actor.Id,
                settingCommand.Setting.Customer.Id,
                (byte)settingCommand.Setting.Customer.Level);
        }

        [Test]
        public void ValidateSettingCommand_ExecuteGetProductSettingTypesOnProductService()
        {
            //// Arrange
            var settingCommand = FakeObjectManager.CreateSettingCommandFakeObject();
            _validationDataAccess.ValidatePermission(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<byte>()).Returns(Result.Success);

            //// Act
            _validationService.ValidateSettingCommand(settingCommand);

            //// Assert
            _productService.Received().GetProductSettingTypes(Arg.Is<byte>(
                productId => productId == settingCommand.Setting.Product.Id));
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
            _validationDataAccess.ValidatePermission(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<byte>()).Returns(Result.Success);
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
            _validationDataAccess.ValidatePermission(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<byte>()).Returns(Result.Success);
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
            _validationDataAccess.ValidatePermission(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<byte>()).Returns(Result.Success);
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
            _validationDataAccess.ValidatePermission(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<byte>()).Returns(Result.Success);
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
            _validationDataAccess.ValidatePermission(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<byte>()).Returns(Result.Success);
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
        public void ValidateSettingCommand_CustomerSettingTypeMatchWithProductSettingType_ReturnSuccessResult()
        {
            //// Arrange
            var settingCommand = FakeObjectManager.CreateSettingCommandFakeObject();
            _validationDataAccess.ValidatePermission(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<byte>()).Returns(Result.Success);
            settingCommand.Setting.SettingTypes = new List<SettingType>
            {
                FakeObjectManager.CreateSettingTypeFakeObject(1)
            };
            _productService.GetProductSettingTypes(settingCommand.Setting.Product.Id)
                .Returns(new List<SettingType>
                {
                    FakeObjectManager.CreateSettingTypeFakeObject(1)
                });

            //// Act
            var actualResult = _validationService.ValidateSettingCommand(settingCommand);

            //// Assert
            Assert.IsTrue(actualResult.IsSuccessful);
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
            _validationDataAccess.ValidatePermission(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<byte>()).Returns(Result.Success);
            _productService.GetProductSettingTypes(settingCommand.Setting.Product.Id).Returns(settingCommand.Setting.SettingTypes);

            //// Act
            _validationService.ValidateSettingCommand(settingCommand);

            //// Assert
            settingItemMock.Received().Validate();
        }

        #endregion validate Setting Command Tests

        #region Validate Multiple Setting Command Tests

        [Test]
        public void ValidateMultipleSettingCommand_MultipleSettingCommandIsNull_ThrowArgumentNullException()
        {
            //// Arrange
            MultipleSettingCommand multipleSettingCommand = null;

            //// Act
            Assert.Throws<ArgumentNullException>(
                new TestDelegate(
                    () => _validationService.ValidateMultipleSettingCommand(multipleSettingCommand)));
        }

        [Test]
        public void ValidateMultipleSettingCommand_CustomersIsNull_ThrowArgumentNullException()
        {
            //// Arrange
            MultipleSettingCommand settingCommand = FakeObjectManager.CreateMultipleSettingCommandFakeObject();
            settingCommand.Customers = null;

            //// Act
            Assert.Throws<ArgumentNullException>(
                new TestDelegate(
                    () => _validationService.ValidateMultipleSettingCommand(settingCommand)));
        }

        [Test]
        public void ValidateMultipleSettingCommand_ExecuteValidatePermissionOnDataAccess()
        {
            //// Arrange
            var settingCommand = FakeObjectManager.CreateMultipleSettingCommandFakeObject();

            //// Act
            _validationService.ValidateMultipleSettingCommand(settingCommand);

            //// Assert
            _validationDataAccess.Received().ValidatePermission(
                _settingContext.Actor.Id,
                settingCommand.Setting.Customer.Id,
                (byte)settingCommand.Setting.Customer.Level);
        }

        [Test]
        public void ValidateMultipleSettingCommand_ExecuteGetProductSettingTypesOnProductService()
        {
            //// Arrange
            var settingCommand = FakeObjectManager.CreateMultipleSettingCommandFakeObject();
            _validationDataAccess.ValidatePermission(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<byte>()).Returns(Result.Success);

            //// Act
            _validationService.ValidateMultipleSettingCommand(settingCommand);

            //// Assert
            _productService.Received().GetProductSettingTypes(Arg.Is<byte>(
                productId => productId == settingCommand.Setting.Product.Id));
        }

        [Test]
        public void ValidateMultipleSettingCommand_CustomerLevelIsMember_ExecuteValidatePermissionMultipleOnDataAccess()
        {
            //// Arrange
            var settingCommand = FakeObjectManager.CreateMultipleSettingCommandFakeObject(1, CustomerLevel.Member);
            _validationDataAccess.ValidatePermission(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<byte>()).Returns(Result.Success);
            _productService.GetProductSettingTypes(settingCommand.Setting.Product.Id).Returns(settingCommand.Setting.SettingTypes);

            //// Act
            _validationService.ValidateMultipleSettingCommand(settingCommand);

            //// Assert
            _validationDataAccess.Received().ValidatePermissionMultiple(
                Arg.Is<int>(actorId => actorId == _settingContext.Actor.Id),
                Arg.Is<string>(memberIds => memberIds == string.Join(",", settingCommand.Customers.Select(cust => cust.Id))));
        }

        [TestCase(CustomerLevel.Super)]
        [TestCase(CustomerLevel.Master)]
        [TestCase(CustomerLevel.Agent)]
        [TestCase(CustomerLevel.Member)]
        public void ValidateMultipleSettingCommand_ActorIsCompany_CanUpdateAllMultipleCustomerSetting(CustomerLevel customerLevel)
        {
            //// Arrange
            var multipleSettingCommand = FakeObjectManager.CreateMultipleSettingCommandFakeObject(1, customerLevel);
            _settingContext.Actor.Level = UserLevel.Company;
            _validationDataAccess.ValidatePermission(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<byte>()).Returns(Result.Success);
            _validationDataAccess.ValidatePermissionMultiple(Arg.Any<int>(), Arg.Any<string>()).Returns(Result.Success);
            _productService.GetProductSettingTypes(multipleSettingCommand.Setting.Product.Id).Returns(multipleSettingCommand.Setting.SettingTypes);

            //// Act
            var actualResult = _validationService.ValidateMultipleSettingCommand(multipleSettingCommand);

            //// Assert
            Assert.IsTrue(actualResult.IsSuccessful);
        }

        [TestCase(CustomerLevel.Super)]
        [TestCase(CustomerLevel.Agent)]
        public void ValidateMultipleSettingCommand_ActorIsSuper_CannotUpdateMultipleOtherSuperAndAgentSetting(CustomerLevel customerLevel)
        {
            //// Arrange
            var multipleSettingCommand = FakeObjectManager.CreateMultipleSettingCommandFakeObject(1, customerLevel);
            _settingContext.Actor.Level = UserLevel.Super;

            //// Act
            var actualResult = _validationService.ValidateMultipleSettingCommand(multipleSettingCommand);

            //// Assert
            Assert.IsFalse(actualResult.IsSuccessful);
        }

        [TestCase(CustomerLevel.Master)]
        [TestCase(CustomerLevel.Member)]
        public void ValidateMultipleSettingCommand_ActorIsSuper_CanUpdateMultipleDownlineSettingExceptAgent(CustomerLevel downlineLevel)
        {
            //// Arrange
            var multipleSettingCommand = FakeObjectManager.CreateMultipleSettingCommandFakeObject(1, downlineLevel);
            _settingContext.Actor.Level = UserLevel.Super;
            _validationDataAccess.ValidatePermission(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<byte>()).Returns(Result.Success);
            _validationDataAccess.ValidatePermissionMultiple(Arg.Any<int>(), Arg.Any<string>()).Returns(Result.Success);
            _productService.GetProductSettingTypes(multipleSettingCommand.Setting.Product.Id).Returns(multipleSettingCommand.Setting.SettingTypes);

            //// Act
            var actualResult = _validationService.ValidateMultipleSettingCommand(multipleSettingCommand);

            //// Assert
            Assert.IsTrue(actualResult.IsSuccessful);
        }

        [TestCase(CustomerLevel.Super)]
        [TestCase(CustomerLevel.Master)]
        public void ValidateMultipleSettingCommand_ActorIsMaster_CannotUpdateMultipleOtherMasterAndSuperSetting(CustomerLevel customerLevel)
        {
            //// Arrange
            var settingCommand = FakeObjectManager.CreateMultipleSettingCommandFakeObject(1, customerLevel);
            _settingContext.Actor.Level = UserLevel.Master;

            //// Act
            var actualResult = _validationService.ValidateMultipleSettingCommand(settingCommand);

            //// Assert
            Assert.IsFalse(actualResult.IsSuccessful);
        }

        [TestCase(CustomerLevel.Agent)]
        [TestCase(CustomerLevel.Member)]
        public void ValidateMultipleSettingCommand_ActorIsMaster_CanUpdateMultipleDownlineSetting(CustomerLevel downlineLevel)
        {
            //// Arrange
            var settingCommand = FakeObjectManager.CreateMultipleSettingCommandFakeObject(1, downlineLevel);
            _settingContext.Actor.Level = UserLevel.Master;
            _validationDataAccess.ValidatePermission(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<byte>()).Returns(Result.Success);
            _validationDataAccess.ValidatePermissionMultiple(Arg.Any<int>(), Arg.Any<string>()).Returns(Result.Success);
            _productService.GetProductSettingTypes(settingCommand.Setting.Product.Id).Returns(settingCommand.Setting.SettingTypes);

            //// Act
            var actualResult = _validationService.ValidateMultipleSettingCommand(settingCommand);

            //// Assert
            Assert.IsTrue(actualResult.IsSuccessful);
        }

        [TestCase(CustomerLevel.Super)]
        [TestCase(CustomerLevel.Master)]
        [TestCase(CustomerLevel.Agent)]
        public void ValidateMultipleSettingCommand_ActorIsAgent_CannotUpdateMultipleOtherAgentAndUplineSetting(CustomerLevel customerLevel)
        {
            //// Arrange
            var settingCommand = FakeObjectManager.CreateMultipleSettingCommandFakeObject(1, customerLevel);
            _settingContext.Actor.Level = UserLevel.Agent;

            //// Act
            var actualResult = _validationService.ValidateMultipleSettingCommand(settingCommand);

            //// Assert
            Assert.IsFalse(actualResult.IsSuccessful);
        }

        [Test]
        public void ValidateMultipleSettingCommand_ActorIsAgent_CanUpdateMemberSetting()
        {
            //// Arrange
            var settingCommand = FakeObjectManager.CreateMultipleSettingCommandFakeObject(1, CustomerLevel.Member);
            _settingContext.Actor.Level = UserLevel.Agent;
            _validationDataAccess.ValidatePermission(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<byte>()).Returns(Result.Success);
            _validationDataAccess.ValidatePermissionMultiple(Arg.Any<int>(), Arg.Any<string>()).Returns(Result.Success);
            _productService.GetProductSettingTypes(settingCommand.Setting.Product.Id).Returns(settingCommand.Setting.SettingTypes);

            //// Act
            var actualResult = _validationService.ValidateMultipleSettingCommand(settingCommand);

            //// Assert
            Assert.IsTrue(actualResult.IsSuccessful);
        }

        [Test]
        public void ValidateMultipleSettingCommand_CustomerSettingTypeDoesNotMatchWithProductSettingType_ReturnFailedResult()
        {
            //// Arrange
            var settingCommand = FakeObjectManager.CreateMultipleSettingCommandFakeObject();
            _validationDataAccess.ValidatePermission(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<byte>()).Returns(Result.Success);
            _validationDataAccess.ValidatePermissionMultiple(Arg.Any<int>(), Arg.Any<string>()).Returns(Result.Success);
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
            var actualResult = _validationService.ValidateMultipleSettingCommand(settingCommand);

            //// Assert
            Assert.IsFalse(actualResult.IsSuccessful);
        }

        [Test]
        public void ValidateMultipleSettingCommand_CustomerSettingTypeMatchWithProductSettingType_ReturnSuccessResult()
        {
            //// Arrange
            var settingCommand = FakeObjectManager.CreateMultipleSettingCommandFakeObject();
            _validationDataAccess.ValidatePermission(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<byte>()).Returns(Result.Success);
            _validationDataAccess.ValidatePermissionMultiple(Arg.Any<int>(), Arg.Any<string>()).Returns(Result.Success);
            settingCommand.Setting.SettingTypes = new List<SettingType>
            {
                FakeObjectManager.CreateSettingTypeFakeObject(1)
            };
            _productService.GetProductSettingTypes(settingCommand.Setting.Product.Id)
                .Returns(new List<SettingType>
                {
                    FakeObjectManager.CreateSettingTypeFakeObject(1)
                });

            //// Act
            var actualResult = _validationService.ValidateMultipleSettingCommand(settingCommand);

            //// Assert
            Assert.IsTrue(actualResult.IsSuccessful);
        }

        [Test]
        public void ValidateMultipleSettingCommand_ExecuteValidateMethodOnEachSettingItem()
        {
            //// Arrange
            var settingCommand = FakeObjectManager.CreateMultipleSettingCommandFakeObject();
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
            _validationDataAccess.ValidatePermission(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<byte>()).Returns(Result.Success);
            _validationDataAccess.ValidatePermissionMultiple(Arg.Any<int>(), Arg.Any<string>()).Returns(Result.Success);
            _productService.GetProductSettingTypes(settingCommand.Setting.Product.Id).Returns(settingCommand.Setting.SettingTypes);

            //// Act
            _validationService.ValidateMultipleSettingCommand(settingCommand);

            //// Assert
            settingItemMock.Received().Validate();
        }

        #endregion Validate Multiple Setting Command Tests
    }
}