namespace Fanex.CustomerSetting.SettingType2.Template1.Tests.Services
{
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Dtos.Params;
    using Fanex.CustomerSetting.Dtos.Results;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Services;
    using Fanex.CustomerSetting.SettingType2.Template1.DataAccesses;
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
            _dataAccess.Received().GetCustomerBetLimit(Arg.Is<QuerySettingParam>(queryParam));
        }

        [Test]
        public void GetCustomerSettingType_Always_ReturnCustomerBetLimit()
        {
            //// Arrange
            var queryParam = InstanceFactory.CreateQuerySettingParam();
            var productSettingType = InstanceFactory.CreateProductSettingType(1);
            var customerBetLimit = new SettingType
            {
                Id = 1,
                SettingTemplate = 1,
                SettingLevel = 1,
                SettingItems = new List<BetLimitItem>
                {
                   new BetLimitItem
                   {
                       SportTypeId = 1
                   }
                }
            };

            //// Stub
            _dataAccess.GetCustomerBetLimit(Arg.Any<QuerySettingParam>()).Returns(customerBetLimit);

            //// Act
            var actualCustomerBetLimit = _settingType2Service.GetCustomerSettingType(_settingContext, queryParam, productSettingType);

            //// Assert
            Assert.AreSame(customerBetLimit, actualCustomerBetLimit);
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