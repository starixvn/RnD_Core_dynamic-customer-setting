namespace Fanex.CustomerSetting.SettingType6.Tests.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using CustomerSetting.Models;
    using CustomerSetting.Models.Enumerations;
    using CustomerSetting.Models.Requests;
    using CustomerSetting.Models.Results;
    using CustomerSetting.Services;
    using CustomerSetting.Tests;
    using CustomerSetting.Tests.Models;
    using DataAccesses;
    using Logging;
    using NSubstitute;
    using NUnit.Framework;
    using Resources;
    using SettingType6.Models;
    using SettingType6.Models.Requests;
    using SettingType6.Services;

    [TestFixture]
    public class SettingTypeServiceTests
    {
        private const byte SettingTypeId = 6;

        private ISettingTypeService _settingTypeService;
        private IResourceService _resourceService;
        private ILogger _logger;
        private IMessageService _messageService;
        private IDataAccess _dataAccess;
        private ISettingContext _settingContext;

        [SetUp]
        public void SetUp()
        {
            _resourceService = Substitute.For<IResourceService>();
            _logger = Substitute.For<ILogger>();
            _messageService = Substitute.For<IMessageService>();
            _dataAccess = Substitute.For<IDataAccess>();

            _settingContext = new SettingContextStub();
            _settingTypeService = new SettingTypeService(_messageService, _resourceService, _logger, _dataAccess);
        }

        [Test]
        public void GetCustomerSettingType_Always_CallGetCustomerWinLossLimit()
        {
            // Arrange
            var query = InstanceFactory.CreateQuerySettingParam();

            // Act
            _settingTypeService.GetCustomerSettingType(_settingContext, query);

            // Assert
            _dataAccess.Received().GetCustomerWinLossLimit(Arg.Is(query));
        }

        [Test]
        public void GetCustomerSettingType_Always_ReturnCustomerWinLossLimit()
        {
            // Arrange
            var query = InstanceFactory.CreateQuerySettingParam();
            var winLossLimits = new List<WinLossLimitItem>
            {
                new WinLossLimitItem
                {
                    BetTypeId = 1,
                    SportTypeId = 1,
                    WinLimit = 2,
                    MaxWinRange = 0,
                    LossLimit = 200,
                    MaxLossRange = 3000,
                    MinLossRange = 100,
                    IsAutoReset = false
                }
            };
            _dataAccess.GetCustomerWinLossLimit(Arg.Any<CustomerSettingQuery>()).Returns(winLossLimits);

            // Act
            var actualCustomerWinLossLimits = _settingTypeService.GetCustomerSettingType(_settingContext, query);
            var actualCustomerWinLossLimitItem = actualCustomerWinLossLimits.SettingItems.First() as WinLossLimitItem;

            // Assert
            var expectedWinLossLimit = new WinLossLimitItem
            {
                BetTypeId = 1,
                SportTypeId = 1,
                WinLimit = 2,
                MaxWinRange = 0,
                LossLimit = 200,
                MaxLossRange = 3000,
                MinLossRange = 100,
                IsAutoReset = false
            };
            Assert.AreEqual(expectedWinLossLimit, actualCustomerWinLossLimitItem);
        }

        [Test]
        public void UpdateCustomerSettingType_Always_CallUpdateCustomerWinLossLimit()
        {
            // Arrange
            var customerId = 1;
            var customerLevel = Level.Member;
            var uplineLevel = Level.Agent;
            byte productId = 1;
            var customerWinLossLimit = InstanceFactory.CreateCustomerSettingType(6);

            // Act
            _settingTypeService.UpdateCustomerSettingType(
                _settingContext,
                customerId,
                customerLevel,
                uplineLevel,
                productId,
                customerWinLossLimit);

            // Assert
            _dataAccess.Received().UpdateCustomerWinLossLimit(
                Arg.Is(_settingContext),
                Arg.Is(customerId),
                Arg.Is(customerLevel),
                Arg.Is(uplineLevel),
                Arg.Is(customerWinLossLimit.ToXml()));
        }

        [Test]
        public void UpdateCustomerSettingType_UpdatedSuccessfully_ReturnSuccessResult()
        {
            // Arrange
            var customerId = 1;
            var customerLevel = Level.Member;
            var uplineLevel = Level.Agent;
            byte productId = 1;
            var customerWinLossLimit = InstanceFactory.CreateCustomerSettingType(6);
            var successResult = Result.Success;
            _dataAccess.UpdateCustomerWinLossLimit(
                Arg.Any<ISettingContext>(),
                Arg.Any<int>(),
                Arg.Any<Level>(),
                Arg.Any<Level>(),
                Arg.Any<string>()).Returns(successResult);

            // Act
            var actualResult = _settingTypeService.UpdateCustomerSettingType(
                _settingContext,
                customerId,
                customerLevel,
                uplineLevel,
                productId,
                customerWinLossLimit);

            // Assert
            Assert.IsTrue(actualResult.IsSuccessful);
        }

        [Test]
        public void UpdateCustomerSettingType_UpdatedSuccessfully_ReturnSuccessMsg()
        {
            // Arrange
            var customerId = 1;
            var customerLevel = Level.Member;
            var uplineLevel = Level.Agent;
            byte productId = 1;
            var customerCommission = InstanceFactory.CreateCustomerSettingType(6);
            var successResult = Result.Success;
            _dataAccess.UpdateCustomerWinLossLimit(
                Arg.Any<ISettingContext>(),
                Arg.Any<int>(),
                Arg.Any<Level>(),
                Arg.Any<Level>(),
                Arg.Any<string>()).Returns(successResult);

            // Act
            var actualResult = _settingTypeService.UpdateCustomerSettingType(
                _settingContext,
                customerId,
                customerLevel,
                uplineLevel,
                productId,
                customerCommission);

            // Assert
            Assert.AreEqual(Message.UpdatedWinLossLimitSuccessfully, actualResult.Message);
        }

        [Test]
        public void Handle_Always_CallGetCustomerCurrentWinLossAmount()
        {
            // Arrange
            var routeService = new SettingTypeService(_messageService, _resourceService, _logger, _dataAccess);
            var redirectCommand = new RedirectCommand(
                customerId: 123,
                customerLevel: Level.Member,
                productId: 1,
                uplineLevel: Level.Agent,
                settingTypeId: SettingTypeId,
                actionId: 0,
                actionType: ActionType.Query,
                settingStatus: 0,
                jsonParameters: "{\"sportTypeId\":1,\"betTypeId\":1}");

            // Act
            routeService.Handle(_settingContext, redirectCommand);

            // Assert
            _dataAccess.Received().GetCustomerCurrentWinLossAmount(Arg.Is(new CurrentWinLossAmountQuery
            {
                CustomerId = 123,
                SportTypeId = 1,
                BetTypeId = 1
            }));
        }

        [Test]
        public void Handle_Always_ReturnCustomerCurrentWinLossAmount()
        {
            // Arrange
            var routeService = new SettingTypeService(_messageService, _resourceService, _logger, _dataAccess);
            var redirectCommand = new RedirectCommand(
                customerId: 123,
                customerLevel: Level.Member,
                productId: 1,
                uplineLevel: Level.Agent,
                settingTypeId: SettingTypeId,
                actionId: 0,
                actionType: ActionType.Query,
                settingStatus: 0,
                jsonParameters: "{\"sportTypeId\":1,\"betTypeId\":1}");
            var currentWinLossAmount = new CurrentWinLossAmountItem
            {
                WinPercent = 10,
                MaxWinAmount = 1000,
                LossPercent = 0,
                MaxLossAmount = 1000
            };
            _dataAccess.GetCustomerCurrentWinLossAmount(Arg.Any<CurrentWinLossAmountQuery>()).Returns(currentWinLossAmount);

            // Act
            var actualCurrentWinLossAmount = routeService.Handle(_settingContext, redirectCommand);

            // Assert
            var expectedWinLossAmount = new CurrentWinLossAmountItem
            {
                WinPercent = 10,
                MaxWinAmount = 1000,
                LossPercent = 0,
                MaxLossAmount = 1000
            };
            Assert.AreEqual(expectedWinLossAmount, actualCurrentWinLossAmount);
        }
    }
}