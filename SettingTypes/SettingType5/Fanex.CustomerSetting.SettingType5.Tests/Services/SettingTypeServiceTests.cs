namespace Fanex.CustomerSetting.SettingType5.Tests.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Models.Enumerations;
    using Fanex.CustomerSetting.Models.Requests;
    using Fanex.CustomerSetting.Models.Results;
    using Fanex.CustomerSetting.Services;
    using Fanex.CustomerSetting.SettingType5.DataAccesses;
    using Fanex.CustomerSetting.SettingType5.Dtos;
    using Fanex.CustomerSetting.SettingType5.Models;
    using Fanex.CustomerSetting.SettingType5.Resources;
    using Fanex.CustomerSetting.SettingType5.Services;
    using Fanex.CustomerSetting.Tests;
    using Fanex.CustomerSetting.Tests.Models;
    using Fanex.Logging;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class SettingTypeServiceTests
    {
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
        public void GetCustomerSettingType_Always_CallGetCustomerChargeCommission()
        {
            // Arrange
            var query = InstanceFactory.CreateQuerySettingParam();

            // Act
            _settingTypeService.GetCustomerSettingType(_settingContext, query);

            // Assert
            _dataAccess.Received().GetCustomerChargeCommissionRawData(Arg.Is(query));
        }

        [Test]
        public void GetCustomerSettingType_Always_ReturnCustomerChargeCommission()
        {
            // Arrange
            var query = InstanceFactory.CreateQuerySettingParam();
            var chargeCommissionItemsRawData = new List<ChargeCommissionDto>
            {
                new ChargeCommissionDto
                {
                    BetTypeId = 1,
                    SportTypeId = 1,
                    ChargeCommission = 0.88m,
                    UplineChargeCommission = 0.9m
                }
            };
            _dataAccess.GetCustomerChargeCommissionRawData(Arg.Any<CustomerSettingQuery>()).Returns(chargeCommissionItemsRawData);

            // Act
            var actualCustomerCommission = _settingTypeService.GetCustomerSettingType(_settingContext, query);
            var actualCustomerCommissionItem = actualCustomerCommission.SettingItems.First() as ChargeCommissionItem;

            // Assert
            var expectedChargeComm = new ChargeCommissionItem
            {
                BetTypeId = 1,
                SportTypeId = 1,
                ChargeCommission = 0.88m,
                Upline = new ChargeCommissionItem
                {
                    ChargeCommission = 0.9m
                }
            };
            Assert.AreEqual(expectedChargeComm, actualCustomerCommissionItem);
        }

        [Test]
        public void UpdateCustomerSettingType_Always_CallUpdateCustomerCommission()
        {
            // Arrange
            var customerId = 1;
            var customerLevel = Level.Member;
            var uplineLevel = Level.Agent;
            byte productId = 1;
            var customerCommission = InstanceFactory.CreateCustomerSettingType(5);

            // Act
            _settingTypeService.UpdateCustomerSettingType(
                _settingContext,
                customerId,
                customerLevel,
                uplineLevel,
                productId,
                customerCommission);

            // Assert
            _dataAccess.Received().UpdateCustomerChargeCommission(
                Arg.Is(_settingContext),
                Arg.Is(customerId),
                Arg.Is(customerLevel),
                Arg.Is(uplineLevel),
                Arg.Is(customerCommission.ToXml()));
        }

        [Test]
        public void UpdateCustomerSettingType_UpdatedSuccessfully_ReturnSuccessResult()
        {
            // Arrange
            var customerId = 1;
            var customerLevel = Level.Member;
            var uplineLevel = Level.Agent;
            byte productId = 1;
            var customerCommission = InstanceFactory.CreateCustomerSettingType(5);
            var successResult = Result.Success;
            _dataAccess.UpdateCustomerChargeCommission(
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
            var customerCommission = InstanceFactory.CreateCustomerSettingType(5);
            var successResult = Result.Success;
            _dataAccess.UpdateCustomerChargeCommission(
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
            Assert.AreEqual(Message.UpdatedChargeCommissionSuccessfully, actualResult.Message);
        }
    }
}