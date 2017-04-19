namespace Fanex.CustomerSetting.SettingType14.Tests.Services
{
    using CustomerSetting.Models;
    using CustomerSetting.Models.Enumerations;
    using CustomerSetting.Models.Requests;
    using CustomerSetting.Models.Results;
    using CustomerSetting.Services;
    using CustomerSetting.Tests;
    using CustomerSetting.Tests.Models;
    using DataAccesses;
    using Dtos;
    using Logging;
    using NSubstitute;
    using NUnit.Framework;
    using Resources;
    using SettingType14.Models;
    using SettingType14.Services;
    using System.Collections.Generic;
    using System.Linq;

    [TestFixture]
    public class SettingTypeServiceTests
    {
        private const byte SettingTypeId = 14;

        private ISettingTypeService _settingTypeService;
        private IDataAccess _dataAccess;
        private ISettingContext _settingContext;

        [SetUp]
        public void SetUp()
        {
            IResourceService resourceService = Substitute.For<IResourceService>();
            ILogger logger = Substitute.For<ILogger>();
            IMessageService messageService = Substitute.For<IMessageService>();

            _dataAccess = Substitute.For<IDataAccess>();
            _settingContext = new SettingContextStub();
            _settingTypeService = new SettingTypeService(messageService, resourceService, logger, _dataAccess);
        }

        [Test]
        public void GetCustomerSettingType_Always_CallGetCustomerTicketCommissionRawData()
        {
            // Arrange
            var query = InstanceFactory.CreateQuerySettingParam();

            // Act
            _settingTypeService.GetCustomerSettingType(_settingContext, query);

            // Assert
            _dataAccess.Received().GetCustomerTicketCommissionRawData(Arg.Is(query));
        }

        [Test]
        public void GetCustomerSettingType_Always_ReturnCustomerTicketCommission()
        {
            // Arrange
            var query = InstanceFactory.CreateQuerySettingParam();
            var commissionRawData = new List<TicketCommissionDto>
            {
                new TicketCommissionDto
                {
                    BetTypeId = 1,
                    SportTypeId = 1,
                    MaxGivenCommission = 0.1m,
                    SuperCommission = 0.08m
                }
            };

            query.CustomerLevel = Level.Super;
            _dataAccess.GetCustomerTicketCommissionRawData(Arg.Any<CustomerSettingQuery>()).Returns(commissionRawData);

            // Act
            var actualCustomerCommission = _settingTypeService.GetCustomerSettingType(_settingContext, query);
            var actualCustomerCommissionItem = actualCustomerCommission.SettingItems.First() as TicketCommissionItem;

            // Assert
            var expectedCommission = new TicketCommissionItem
            {
                BetTypeId = 1,
                SportTypeId = 1,
                UplineCommission = 0.1m,
                Commission = 0.08m
            };

            Assert.AreEqual(expectedCommission, actualCustomerCommissionItem);
        }

        [Test]
        public void UpdateCustomerSettingType_Always_CallUpdateCustomerTicketCommission()
        {
            // Arrange
            var customerId = 1;
            var customerLevel = Level.Member;
            var uplineLevel = Level.Agent;
            byte productId = 1;
            SettingType customerCommission = InstanceFactory.CreateCustomerSettingType(SettingTypeId);

            // Act
            _settingTypeService.UpdateCustomerSettingType(
                _settingContext,
                customerId,
                customerLevel,
                uplineLevel,
                productId,
                customerCommission);

            // Assert
            _dataAccess.Received().UpdateCustomerTicketCommission(
                Arg.Is(_settingContext),
                Arg.Is(customerId),
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
            SettingType customerCommission = InstanceFactory.CreateCustomerSettingType(SettingTypeId);
            Result successResult = Result.Success;

            _dataAccess.UpdateCustomerTicketCommission(
                Arg.Any<ISettingContext>(),
                Arg.Any<int>(),
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
            SettingType customerCommission = InstanceFactory.CreateCustomerSettingType(SettingTypeId);
            Result successResult = Result.Success;

            _dataAccess.UpdateCustomerTicketCommission(
                Arg.Any<ISettingContext>(),
                Arg.Any<int>(),
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
            Assert.AreEqual(Message.UpdatedTicketCommissionSuccessfully, actualResult.Message);
        }
    }
}