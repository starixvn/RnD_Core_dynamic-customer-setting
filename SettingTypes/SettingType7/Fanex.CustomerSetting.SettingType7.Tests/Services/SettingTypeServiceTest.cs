namespace Fanex.CustomerSetting.SettingType7.Tests.Services
{
    using CustomerSetting.Models;
    using CustomerSetting.Models.Enumerations;
    using CustomerSetting.Models.Results;
    using CustomerSetting.Services;
    using CustomerSetting.Tests;
    using CustomerSetting.Tests.Models;
    using DataAccesses;
    using Dtos;
    using Fanex.CustomerSetting.Exceptions;
    using Logging;
    using NSubstitute;
    using NUnit.Framework;
    using Resources;
    using SettingType7.Models;
    using SettingType7.Services;
    using System.Collections.Generic;
    using System.Linq;

    [TestFixture]
    public class SettingTypeServiceTest
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
            _dataAccess = Substitute.For<IDataAccess>();
            _messageService = new MessageService(_resourceService);

            _settingContext = new SettingContextStub();
            _settingTypeService = new SettingTypeService(_messageService, _resourceService, _logger, _dataAccess);
        }

        [Test]
        public void GetCustomerSettingType_Always_CallGetCustomerStakeLevelRawData()
        {
            // Arrange.
            var query = InstanceFactory.CreateQuerySettingParam();

            // Act.
            _settingTypeService.GetCustomerSettingType(_settingContext, query);

            // Assert.
            _dataAccess.Received().GetCustomerStakeLevelRawData(Arg.Is(query));
        }

        [Test]
        public void GetCustomerSettingType_Always_ReturnCustomerStakeLevel()
        {
            // Arrange.
            var query = InstanceFactory.CreateQuerySettingParam();
            var stakeLevelRawData = new List<StakeLevelDto>
            {
                new StakeLevelDto
                {
                    BetTypeId = 1,
                    SportTypeId = 1,
                    StakeLevelId = 1,
                    StakeLevelRange = "100 - 1000",
                    Selected = true
                }
            };
            _dataAccess.GetCustomerStakeLevelRawData(query).Returns(stakeLevelRawData);

            // Act.
            var actualCustomerStakeLevelSetting = _settingTypeService.GetCustomerSettingType(_settingContext, query);
            var actualCustomerStakeLevelItem = actualCustomerStakeLevelSetting.SettingItems.First() as StakeLevelItem;

            // Assert.
            var expectedStakeLevelItem = new StakeLevelItem
            {
                BetTypeId = 1,
                SportTypeId = 1,
                StakeRanges = new List<StakeRange>
                {
                    new StakeRange
                    {
                        StakeLevelId = 1,
                        StakeLevelRange = "100 - 1000",
                        Selected = true
                    }
                }
            };
            Assert.AreEqual(expectedStakeLevelItem, actualCustomerStakeLevelItem);
        }

        [Test]
        public void UpdateCustomerSettingType_UpdateMemberStakeLevelWithStakeLevelValuesIsEmpty_ReturnError()
        {
            // Arrange
            var customerId = 1;
            var customerLevel = Level.Member;
            var uplineLevel = Level.Agent;
            byte productId = 1;
            var customerStakeLevel = InstanceFactory.CreateCustomerSettingType(7);
            customerStakeLevel.SettingItems = new List<ISettingItem>
            {
                new StakeLevelItem
                {
                    SportTypeId = 1,
                    BetTypeId = 1,
                    StakeRanges = new List<StakeRange>()
                }
            };

            // Act
            TestDelegate updateDelegate = () => _settingTypeService.UpdateCustomerSettingType(
                _settingContext,
                customerId,
                customerLevel,
                uplineLevel,
                productId,
                customerStakeLevel);

            // Assert
            Assert.Throws(typeof(CustomerSettingException), updateDelegate);
        }

        [Test]
        public void UpdateCustomerSettingType_Always_CallUpdateCustomerStakeLevel()
        {
            // Arrange.
            var customerId = 1;
            var customerLevel = Level.Agent;
            var uplineLevel = Level.Master;
            byte productId = 1;
            var customerStakeLevel = InstanceFactory.CreateCustomerSettingType(7);

            // Act.
            _settingTypeService.UpdateCustomerSettingType(
                _settingContext,
                customerId,
                customerLevel,
                uplineLevel,
                productId,
                customerStakeLevel);

            // Assert.
            _dataAccess.Received().UpdateCustomerStakeLevel(
                Arg.Is(_settingContext),
                Arg.Is(customerId),
                Arg.Is(customerLevel),
                Arg.Is(uplineLevel),
                Arg.Is(customerStakeLevel.ToXml()));
        }

        [Test]
        public void UpdateCustomerSettingType_UpdatedSuccessfully_ReturnSuccessResult()
        {
            // Arrange.
            var customerId = 1;
            var customerLevel = Level.Agent;
            var uplineLevel = Level.Master;
            byte productId = 1;
            var customerStakeLevel = InstanceFactory.CreateCustomerSettingType(7);
            var successResult = Result.Success;
            _dataAccess.UpdateCustomerStakeLevel(
                Arg.Any<ISettingContext>(),
                Arg.Any<int>(),
                Arg.Any<Level>(),
                Arg.Any<Level>(),
                Arg.Any<string>()).Returns(successResult);

            // Act.
            var actualResult = _settingTypeService.UpdateCustomerSettingType(
                _settingContext,
                customerId,
                customerLevel,
                uplineLevel,
                productId,
                customerStakeLevel);

            // Assert.
            Assert.IsTrue(actualResult.IsSuccessful);
        }

        [Test]
        public void UpdateCustomerSettingType_UpdatedSuccessfully_ReturnSuccessMsg()
        {
            // Arrange.
            var customerId = 1;
            var customerLevel = Level.Agent;
            var uplineLevel = Level.Master;
            byte productId = 1;
            var customerStakeLevel = InstanceFactory.CreateCustomerSettingType(7);
            var successResult = Result.Success;
            _dataAccess.UpdateCustomerStakeLevel(
                Arg.Any<ISettingContext>(),
                Arg.Any<int>(),
                Arg.Any<Level>(),
                Arg.Any<Level>(),
                Arg.Any<string>()).Returns(successResult);

            // Act.
            var actualResult = _settingTypeService.UpdateCustomerSettingType(
                _settingContext,
                customerId,
                customerLevel,
                uplineLevel,
                productId,
                customerStakeLevel);

            // Assert.
            Assert.AreEqual(Message.UpdatedBetLimitSuccessfully, actualResult.Message);
        }
    }
}