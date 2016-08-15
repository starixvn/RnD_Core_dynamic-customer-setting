namespace Fanex.CustomerSetting.SettingType9.Tests.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using CustomerSetting.Models;
    using CustomerSetting.Models.Enumerations;
    using CustomerSetting.Models.Requests;
    using CustomerSetting.Models.Results;
    using CustomerSetting.Services;
    using CustomerSetting.Tests;
    using DataAccesses;
    using Dtos;
    using Logging;
    using NSubstitute;
    using NUnit.Framework;
    using Resources;
    using SettingType9.Models;
    using SettingType9.Services;

    public class SettingTypeServiceTests
    {
        private const byte SettingTypeId = 9;

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
            _settingContext = Substitute.For<ISettingContext>();
            _settingTypeService = new SettingTypeService(_messageService, _resourceService, _logger, _dataAccess);
        }

        [Test]
        public void GetCustomerSettingType_Always_CallGetCustomerBetLimit()
        {
            // Arrange
            var query = InstanceFactory.CreateQuerySettingParam();

            // Act
            _settingTypeService.GetCustomerSettingType(_settingContext, query);

            // Assert
            _dataAccess.Received().GetCustomerBetLimit(Arg.Is(query));
        }

        [Test]
        public void GetCustomerSettingType_Always_ReturnCustomerBetLimit()
        {
            // Arrange
            var query = InstanceFactory.CreateQuerySettingParam();
            var rawData = new List<BetLimitDto>
            {
                new BetLimitDto
                {
                    SportTypeId = 1,
                    BetTypeId = 1,
                    MinBet = 1,
                    MaxBet = 2,
                    MaxPerNumber = 3,
                    MinBetMin = 0,
                    MaxBetMax = 3,
                    MaxPerNumberMax = 4
                }
            };

            _dataAccess.GetCustomerBetLimit(Arg.Any<CustomerSettingQuery>()).Returns(rawData);

            // Act
            var actualResult = _settingTypeService.GetCustomerSettingType(_settingContext, query);
            var firstBetLimitItem = actualResult.SettingItems.First() as BetLimitItem;

            // Assert
            var expectedFirstResult = new BetLimitItem
            {
                SportTypeId = 1,
                BetTypeId = 1,
                MinBet = 1,
                MaxBet = 2,
                MaxPerNumber = 3,
                Upline = new BetLimitItem
                {
                    MinBet = 0,
                    MaxBet = 3,
                    MaxPerNumber = 4
                }
            };

            Assert.AreEqual(expectedFirstResult, firstBetLimitItem);
        }

        [Test]
        public void UpdateCustomerSettingType_Always_CallUpdateCustomerBetLimit()
        {
            // Arrange
            var customerId = 1;
            var customerLevel = Level.Member;
            var uplineLevel = Level.Agent;
            byte productId = 1;
            var betLimit = InstanceFactory.CreateCustomerSettingType(SettingTypeId);

            // Act
            _settingTypeService.UpdateCustomerSettingType(
                _settingContext,
                customerId,
                customerLevel,
                uplineLevel,
                productId,
                betLimit);

            // Assert
            _dataAccess.Received().UpdateCustomerBetLimit(
                Arg.Is(_settingContext),
                Arg.Is(customerId),
                Arg.Is(customerLevel),
                Arg.Is(uplineLevel),
                Arg.Is(betLimit.ToXml()));
        }

        [Test]
        public void UpdateCustomerSettingType_UpdatedSuccessfully_ReturnSuccessResult()
        {
            // Arrange
            var customerId = 1;
            var customerLevel = Level.Member;
            var uplineLevel = Level.Agent;
            byte productId = 1;
            var betLimit = InstanceFactory.CreateCustomerSettingType(SettingTypeId);
            var successResult = Result.Success;

            _dataAccess.UpdateCustomerBetLimit(
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
                betLimit);

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
            var betLimit = InstanceFactory.CreateCustomerSettingType(SettingTypeId);
            var successResult = Result.Success;

            _dataAccess.UpdateCustomerBetLimit(
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
                betLimit);

            // Assert
            Assert.AreEqual(Message.UpdateBetSettingSuccessfully, actualResult.Message);
        }
    }
}