using System.Collections.Generic;
using System.Linq;
using Fanex.CustomerSetting.Models;
using Fanex.CustomerSetting.Models.Enumerations;
using Fanex.CustomerSetting.Models.Requests;
using Fanex.CustomerSetting.Models.Results;
using Fanex.CustomerSetting.Services;
using Fanex.CustomerSetting.SettingType21.DataAccesses;
using Fanex.CustomerSetting.SettingType21.Dtos;
using Fanex.CustomerSetting.SettingType21.Models;
using Fanex.CustomerSetting.SettingType21.Resources;
using Fanex.CustomerSetting.SettingType21.Services;
using Fanex.CustomerSetting.Tests;
using Fanex.Logging;
using NSubstitute;
using NUnit.Framework;

namespace Fanex.CustomerSetting.SettingType21.Tests.Services
{
    public class SettingTypeServiceTests
    {
        private const byte SettingTypeId = 21;
        private IDataAccess _dataAccess;
        private ILogger _logger;
        private IMessageService _messageService;
        private IResourceService _resourceService;
        private ISettingContext _settingContext;

        private ISettingTypeService _settingTypeService;

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
        public void GetCustomerSettingType_Always_CallGetCustomerProductStatus()
        {
            // Arrange
            var query = InstanceFactory.CreateQuerySettingParam();

            // Act
            _settingTypeService.GetCustomerSettingType(_settingContext, query);

            // Assert
            _dataAccess.Received().GetCustomerProductStatus(Arg.Is(query));
        }

        [Test]
        public void GetCustomerSettingType_Always_ReturnCustomerProductStatus()
        {
            // Arrange
            var query = InstanceFactory.CreateQuerySettingParam();
            var rawData = new List<ProductStatusDto>
            {
                new ProductStatusDto
                {
                    BetTypeId = 1,
                    SportTypeId = 1,
                    IsDisabledUpline = false,
                    IsDisabled = false
                }
            };

            _dataAccess.GetCustomerProductStatus(Arg.Any<CustomerSettingQuery>()).Returns(rawData);

            // Act
            var actualResult = _settingTypeService.GetCustomerSettingType(_settingContext, query);
            var firstProductStatusItem = actualResult.SettingItems.First() as ProductStatusItem;

            // Assert
            var expectedFirstResult = new ProductStatusItem
            {
                BetTypeId = 1,
                SportTypeId = 1,
                IsDisabled = false,
                Upline = new ProductStatusItem(false)
            };

            Assert.AreEqual(expectedFirstResult, firstProductStatusItem);
        }

        [Test]
        public void UpdateCustomerSettingType_Always_CallUpdateCustomerProductStatus()
        {
            // Arrange
            var customerId = 1;
            var customerLevel = Level.Member;
            var uplineLevel = Level.Agent;
            byte productId = 1;
            var productStatus = InstanceFactory.CreateCustomerSettingType(SettingTypeId);

            // Act
            _settingTypeService.UpdateCustomerSettingType(
                _settingContext,
                customerId,
                customerLevel,
                uplineLevel,
                productId,
                productStatus);

            // Assert
            _dataAccess.Received().UpdateCustomerProductStatus(
                Arg.Is(_settingContext),
                Arg.Is(customerId),
                Arg.Is(customerLevel),
                Arg.Is(uplineLevel),
                Arg.Is(productStatus.ToXml()),
                Arg.Is(productStatus.Id));
        }

        [Test]
        public void UpdateCustomerSettingType_UpdatedSuccessfully_ReturnSuccessResult()
        {
            // Arrange
            var customerId = 1;
            var customerLevel = Level.Member;
            var uplineLevel = Level.Agent;
            byte productId = 1;
            var productStatus = InstanceFactory.CreateCustomerSettingType(SettingTypeId);
            var successResult = Result.Success;

            _dataAccess.UpdateCustomerProductStatus(
                Arg.Any<ISettingContext>(),
                Arg.Any<int>(),
                Arg.Any<Level>(),
                Arg.Any<Level>(),
                Arg.Any<string>(),
                Arg.Any<short>()).Returns(successResult);

            // Act
            var actualResult = _settingTypeService.UpdateCustomerSettingType(
                _settingContext,
                customerId,
                customerLevel,
                uplineLevel,
                productId,
                productStatus);

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
            var productStatus = InstanceFactory.CreateCustomerSettingType(SettingTypeId);
            var successResult = Result.Success;

            _dataAccess.UpdateCustomerProductStatus(
                Arg.Any<ISettingContext>(),
                Arg.Any<int>(),
                Arg.Any<Level>(),
                Arg.Any<Level>(),
                Arg.Any<string>(),
                Arg.Any<short>()).Returns(successResult);

            // Act
            var actualResult = _settingTypeService.UpdateCustomerSettingType(
                _settingContext,
                customerId,
                customerLevel,
                uplineLevel,
                productId,
                productStatus);

            // Assert
            Assert.AreEqual(Message.UpdateProductStatusSuccessfully, actualResult.Message);
        }
    }
}