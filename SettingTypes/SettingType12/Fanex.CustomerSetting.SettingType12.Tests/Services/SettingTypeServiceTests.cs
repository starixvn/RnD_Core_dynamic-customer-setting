namespace Fanex.CustomerSetting.SettingType12.Tests.Services
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
    using SettingType12.Models;
    using SettingType12.Models.Requests;
    using SettingType12.Services;

    [TestFixture]
    public class SettingTypeServiceTests
    {
        private const byte SettingTypeId = 12;

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
        public void GetCustomerSettingType_Always_CallGetCustomerLossLimit()
        {
            // Arrange
            var query = InstanceFactory.CreateQuerySettingParam();

            // Act
            _settingTypeService.GetCustomerSettingType(_settingContext, query);

            // Assert
            _dataAccess.Received().GetCustomerLossLimit(Arg.Is(query));
        }

        [Test]
        public void GetCustomerSettingType_Always_ReturnCustomerLossLimit()
        {
            // Arrange
            var query = InstanceFactory.CreateQuerySettingParam();
            var lossLimits = new List<LossLimitItem>
            {
                new LossLimitItem
                {
                    BetTypeId = 1,
                    SportTypeId = 1,
                    LossLimit = 200,
                    MaxLossRange = 3000,
                    MinLossRange = 100,
                    IsAutoReset = false
                }
            };

            _dataAccess.GetCustomerLossLimit(Arg.Any<CustomerSettingQuery>()).Returns(lossLimits);

            // Act
            var actualCustomerLossLimits = _settingTypeService.GetCustomerSettingType(_settingContext, query);
            var actualCustomerLossLimitItem = actualCustomerLossLimits.SettingItems.First() as LossLimitItem;

            // Assert
            var expectedLossLimit = new LossLimitItem
            {
                BetTypeId = 1,
                SportTypeId = 1,
                LossLimit = 200,
                MaxLossRange = 3000,
                MinLossRange = 100,
                IsAutoReset = false
            };

            Assert.AreEqual(expectedLossLimit, actualCustomerLossLimitItem);
        }

        [Test]
        public void UpdateCustomerSettingType_Always_CallUpdateCustomerLossLimit()
        {
            // Arrange
            var customerId = 1;
            var customerLevel = Level.Member;
            var uplineLevel = Level.Agent;
            byte productId = 1;
            var customerLossLimit = InstanceFactory.CreateCustomerSettingType(SettingTypeId);

            // Act
            _settingTypeService.UpdateCustomerSettingType(
                _settingContext,
                customerId,
                customerLevel,
                uplineLevel,
                productId,
                customerLossLimit);

            // Assert
            _dataAccess.Received().UpdateCustomerLossLimit(
                Arg.Is(_settingContext),
                Arg.Is(customerId),
                Arg.Is(customerLevel),
                Arg.Is(uplineLevel),
                Arg.Is(customerLossLimit.ToXml()));
        }

        [Test]
        public void UpdateCustomerSettingType_UpdatedSuccessfully_ReturnSuccessResult()
        {
            // Arrange
            var customerId = 1;
            var customerLevel = Level.Member;
            var uplineLevel = Level.Agent;
            byte productId = 1;
            var customerLossLimit = InstanceFactory.CreateCustomerSettingType(SettingTypeId);
            var successResult = Result.Success;

            _dataAccess.UpdateCustomerLossLimit(
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
                customerLossLimit);

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
            var customerCommission = InstanceFactory.CreateCustomerSettingType(SettingTypeId);
            var successResult = Result.Success;

            _dataAccess.UpdateCustomerLossLimit(
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
            Assert.AreEqual(Message.UpdatedLossLimitSuccessfully, actualResult.Message);
        }

        [Test]
        public void Handle_Always_CallGetCustomerCurrentLossAmount()
        {
            // Arrange
            var routeService = new SettingTypeService(_messageService, _resourceService, _logger, _dataAccess);
            var jsonParameters = "{\"customerId\":123,\"sportTypeId\":1,\"betTypeId\":1}";

            // Act
            routeService.Handle(_settingContext, 0, ActionType.Query, jsonParameters);

            // Assert
            _dataAccess.Received().GetCustomerCurrentLossAmount(Arg.Is(new CurrentLossAmountQuery
            {
                CustomerId = 123,
                SportTypeId = 1,
                BetTypeId = 1
            }));
        }

        [Test]
        public void Handle_Always_ReturnCustomerCurrentLossAmount()
        {
            // Arrange
            var routeService = new SettingTypeService(_messageService, _resourceService, _logger, _dataAccess);
            var jsonParameters = "{\"customerId\":123,\"sportTypeId\":1,\"betTypeId\":1}";
            var currentLossAmount = new CurrentLossAmountItem
            {
                LossPercent = 0,
                MaxLossAmount = 1000
            };

            _dataAccess.GetCustomerCurrentLossAmount(Arg.Any<CurrentLossAmountQuery>()).Returns(currentLossAmount);

            // Act
            var actualCurrentLossAmount = routeService.Handle(_settingContext, 0, ActionType.Query, jsonParameters);

            // Assert
            var expectedLossAmount = new CurrentLossAmountItem
            {
                LossPercent = 0,
                MaxLossAmount = 1000
            };

            Assert.AreEqual(expectedLossAmount, actualCurrentLossAmount);
        }
    }
}