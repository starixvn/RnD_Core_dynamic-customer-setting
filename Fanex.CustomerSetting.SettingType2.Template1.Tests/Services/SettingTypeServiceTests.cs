namespace Fanex.CustomerSetting.SettingType2.Template1.Tests.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Params;
    using Fanex.CustomerSetting.Results;
    using Fanex.CustomerSetting.Services;
    using Fanex.CustomerSetting.SettingType2.Template1.DataAccesses;
    using Fanex.CustomerSetting.SettingType2.Template1.Dtos;
    using Fanex.CustomerSetting.SettingType2.Template1.Models;
    using Fanex.CustomerSetting.SettingType2.Template1.Services;
    using Fanex.CustomerSetting.Tests;
    using Fanex.CustomerSetting.Tests.Models;
    using Fanex.Logging;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class SettingTypeServiceTests
    {
        private ISettingTypeService _settingType2Service;
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

            _settingType2Service = new SettingTypeService(_messageService, _resourceService, _logger, _dataAccess);
        }

        [Test]
        public void GetCustomerSettingType_Always_CallGetCustomerBetLimit()
        {
            //// Arrange
            var queryParam = InstanceFactory.CreateQuerySettingParam();
            var productSettingType = InstanceFactory.CreateProductSettingType(1);

            //// Act
            _settingType2Service.GetCustomerSettingType(_settingContext, queryParam, productSettingType);

            //// Assert
            _dataAccess.Received().GetCustomerBetLimitItemsRawData(Arg.Is<QueryCustomerSettingParam>(queryParam));
        }

        [Test]
        public void GetCustomerSettingType_Always_ReturnCustomerBetLimit()
        {
            //// Arrange
            var queryParam = InstanceFactory.CreateQuerySettingParam();
            var productSettingType = InstanceFactory.CreateProductSettingType(1);
            var betLimitItemsRawData = new List<BetLimitItemDto>
                {
                    new BetLimitItemDto
                    {
                        BetTypeId = 1,
                        SportTypeId = 1,
                        MinBet = 10,
                        MaxBet = 100,
                        MaxPerMatch = 1000,
                        MinBetMin = 1,
                        MaxBetMax = 120,
                        MaxPerMatchMax = 1200
                    }
                };

            //// Stub
            _dataAccess.GetCustomerBetLimitItemsRawData(Arg.Any<QueryCustomerSettingParam>()).Returns(betLimitItemsRawData);

            //// Act
            var customerBetLimit = _settingType2Service.GetCustomerSettingType(_settingContext, queryParam, productSettingType);

            //// Assert
            var actualCustomerBetLimitItem = customerBetLimit.SettingItems.First() as BetLimitItem;

            Assert.AreEqual(1, actualCustomerBetLimitItem.SportTypeId);
            Assert.AreEqual(1, actualCustomerBetLimitItem.BetTypeId);
            Assert.AreEqual(10, actualCustomerBetLimitItem.MinBet);
            Assert.AreEqual(100, actualCustomerBetLimitItem.MaxBet);
            Assert.AreEqual(1000, actualCustomerBetLimitItem.MaxPerMatch);
            Assert.AreEqual(1, actualCustomerBetLimitItem.Constraint.MinBet);
            Assert.AreEqual(120, actualCustomerBetLimitItem.Constraint.MaxBet);
            Assert.AreEqual(1200, actualCustomerBetLimitItem.Constraint.MaxPerMatch);
        }

        [Test]
        public void UpdateCustomerSettingType_Always_CallUpdateCustomerBetLimit()
        {
            //// Arrange
            var customerId = 1;
            var customerLevel = Level.Member;
            var uplineLevel = Level.Agent;
            byte productId = 1;
            var customerBetLimit = InstanceFactory.CreateCustomerSettingType(1);

            //// Act
            _settingType2Service.UpdateCustomerSettingType(_settingContext, customerId, customerLevel, uplineLevel, productId, customerBetLimit);

            //// Assert
            _dataAccess.Received().UpdateCustomerBetLimit(
                Arg.Is(_settingContext),
                Arg.Is(customerId),
                Arg.Is(customerLevel),
                Arg.Is(uplineLevel),
                Arg.Is(productId),
                Arg.Is(customerBetLimit.ToXml()));
        }

        [Test]
        public void UpdateCustomerSettingType_UpdatedSuccessfully_ReturnSuccessResult()
        {
            //// Arrange
            var customerId = 1;
            var customerLevel = Level.Member;
            var uplineLevel = Level.Agent;
            byte productId = 1;
            var customerBetLimit = InstanceFactory.CreateCustomerSettingType(1);
            var successResult = Result.Success;

            //// Stub
            _dataAccess.UpdateCustomerBetLimit(
                Arg.Any<ISettingContext>(),
                Arg.Any<int>(),
                Arg.Any<Level>(),
                Arg.Any<Level>(),
                Arg.Any<byte>(),
                Arg.Any<string>()).Returns(successResult);

            //// Act
            var actualResult = _settingType2Service.UpdateCustomerSettingType(_settingContext, customerId, customerLevel, uplineLevel, productId, customerBetLimit);

            //// Assert
            Assert.IsTrue(actualResult.IsSuccessful);
        }
    }
}