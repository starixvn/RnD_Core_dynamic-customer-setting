namespace Fanex.CustomerSetting.SettingType3.Tests.Services
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
    using Dtos;
    using Logging;
    using NSubstitute;
    using NUnit.Framework;
    using SettingType3.Models;
    using SettingType3.Services;

    [TestFixture]
    public class SettingTypeServiceTests
    {
        private ISettingTypeService _settingType3Service;
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

            _settingType3Service = new SettingTypeService(_messageService, _resourceService, _logger, _dataAccess);
        }

        [Test]
        public void GetCustomerSettingType_Always_CallGetCustomerCommission()
        {
            //// Arrange
            var query = InstanceFactory.CreateQuerySettingParam();

            //// Act
            _settingType3Service.GetCustomerSettingType(_settingContext, query);

            //// Assert
            _dataAccess.Received().GetCustomerCommissionItemsRawData(Arg.Is<CustomerSettingQuery>(query));
        }

        [Test]
        public void GetCustomerSettingType_Always_ReturnCustomerCommission()
        {
            //// Arrange
            var query = InstanceFactory.CreateQuerySettingParam();
            var commissionItemsRawData = new List<CommissionItemDto>
            {
                new CommissionItemDto
                {
                    BetTypeId = 1,
                    SportTypeId = 1,
                    CommRate = 0.009m,
                    UplineCommRate = 0.01m,
                    UplineOfUplineCommRate = 0.1m
                }
            };

            //// Stub
            _dataAccess.GetCustomerCommissionItemsRawData(Arg.Any<CustomerSettingQuery>()).Returns(commissionItemsRawData);

            //// Act
            var actualCustomerCommission = _settingType3Service.GetCustomerSettingType(_settingContext, query);

            //// Assert
            var actualCustomerCommissionItem = actualCustomerCommission.SettingItems.First() as CommissionItem;
            Assert.AreEqual(1, actualCustomerCommissionItem.BetTypeId);
            Assert.AreEqual(1, actualCustomerCommissionItem.SportTypeId);
            Assert.AreEqual(0.009m, actualCustomerCommissionItem.CommRate);
            Assert.AreEqual(0.01m, actualCustomerCommissionItem.Upline.CommRate);
            Assert.AreEqual(0.1m, actualCustomerCommissionItem.Upline.Upline.CommRate);
        }

        [Test]
        public void UpdateCustomerSettingType_Always_CallUpdateCustomerCommission()
        {
            //// Arrange
            var customerId = 1;
            var customerLevel = Level.Member;
            var uplineLevel = Level.Agent;
            byte productId = 1;
            var customerCommission = InstanceFactory.CreateCustomerSettingType(1);

            //// Act
            _settingType3Service.UpdateCustomerSettingType(_settingContext, customerId, customerLevel, uplineLevel, productId, customerCommission);

            //// Assert
            _dataAccess.Received().UpdateCustomerCommission(
                Arg.Is(_settingContext),
                Arg.Is(customerId),
                Arg.Is(customerLevel),
                Arg.Is(uplineLevel),
                Arg.Is(customerCommission.ToXml()));
        }

        [Test]
        public void UpdateCustomerSettingType_UpdatedSuccessfully_ReturnSuccessResult()
        {
            //// Arrange
            var customerId = 1;
            var customerLevel = Level.Member;
            var uplineLevel = Level.Agent;
            byte productId = 1;
            var customerCommission = InstanceFactory.CreateCustomerSettingType(1);
            var successResult = Result.Success;

            //// Stub
            _dataAccess.UpdateCustomerCommission(
                Arg.Any<ISettingContext>(),
                Arg.Any<int>(),
                Arg.Any<Level>(),
                Arg.Any<Level>(),
                Arg.Any<string>()).Returns(successResult);

            //// Act
            var actualResult = _settingType3Service.UpdateCustomerSettingType(_settingContext, customerId, customerLevel, uplineLevel, productId, customerCommission);

            //// Assert
            Assert.IsTrue(actualResult.IsSuccessful);
        }
    }
}