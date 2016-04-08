namespace Fanex.CustomerSetting.SettingType8.Tests.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using CustomerSetting.Models;
    using CustomerSetting.Models.Enumerations;
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
    using SettingType8.Models;
    using SettingType8.Services;

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
        public void GetCustomerSettingType_Always_CallGetCustomerStakeLevelGroups()
        {
            // Arrange.
            var query = InstanceFactory.CreateQuerySettingParam();

            // Act.
            _settingTypeService.GetCustomerSettingType(_settingContext, query);

            // Assert.
            _dataAccess.Received().GetCustomerStakeLevelGroups(Arg.Is(query.CustomerId), Arg.Is(query.ProductId));
        }

        [Test]
        public void GetCustomerSettingType_Always_ReturnCustomerStakeLevel()
        {
            // Arrange.
            var query = InstanceFactory.CreateQuerySettingParam();
            var stakeLevelGroupRawData = CreateSampleDataOfStakeLevelGroups();
            var stakeLevelDetailRawData = CreateSampleDataOfStakeLevelGroupDetails();
            int numberOfDetailedItems = 0;

            _resourceService.GetResourceData("StakeLevel_GG", "1", "en-US").Returns("Level 1");
            _dataAccess.GetCustomerStakeLevelGroups(query.CustomerId, query.ProductId).Returns(stakeLevelGroupRawData);
            _dataAccess
                .GetCustomerStakeLevelGroupsDetails(query.CustomerId, 1, out numberOfDetailedItems, false)
                .Returns(parameters =>
                {
                    parameters[2] = 1;

                    return stakeLevelDetailRawData;
                });

            // Act.
            var actualCustomerStakeLevelGroupSetting = _settingTypeService.GetCustomerSettingType(_settingContext, query);
            var actualCustomerStakeLevelGroupItem = actualCustomerStakeLevelGroupSetting.SettingItems.First() as StakeLevelGroupItem;

            // Assert.
            var expectedStakeLevelGroupItem = CreateExpectedStakeLevelGroupItem();
            Assert.AreEqual(expectedStakeLevelGroupItem, actualCustomerStakeLevelGroupItem);
        }

        [Test]
        public void GetCustomerSettingType_StakeLevelGroupIsNotEmpty_CallGetCustomerStakeLevelGroupsDetails()
        {
            // Arrange.
            var query = InstanceFactory.CreateQuerySettingParam();
            var stakeLevelGroupRawData = CreateSampleDataOfStakeLevelGroups();

            _dataAccess.GetCustomerStakeLevelGroups(query.CustomerId, query.ProductId).Returns(stakeLevelGroupRawData);

            // Act.
            _settingTypeService.GetCustomerSettingType(_settingContext, query);

            // Assert.
            int numberOfDetailedItems = 0;
            _dataAccess
                .Received()
                .GetCustomerStakeLevelGroupsDetails(Arg.Is(query.CustomerId), Arg.Is<short>(1), out numberOfDetailedItems, Arg.Is(false));
        }

        [Test]
        public void GetCustomerSettingType_StakeLevelGroupIsEmpty_DoNotCallGetCustomerStakeLevelGroupsDetails()
        {
            // Arrange.
            var query = InstanceFactory.CreateQuerySettingParam();

            _dataAccess.GetCustomerStakeLevelGroups(query.CustomerId, query.ProductId).Returns(Enumerable.Empty<StakeLevelGroupDto>());

            // Act.
            _settingTypeService.GetCustomerSettingType(_settingContext, query);

            // Assert.
            int numberOfDetailedItems = 0;
            _dataAccess
                .DidNotReceive()
                .GetCustomerStakeLevelGroupsDetails(Arg.Is(query.CustomerId), Arg.Is<short>(1), out numberOfDetailedItems, Arg.Is(false));
        }

        [Test]
        public void UpdateCustomerSettingType_Always_CallUpdateCustomerStakeLevelGroups()
        {
            // Arrange.
            var customerId = 1;
            var customerLevel = Level.Agent;
            var uplineLevel = Level.Master;
            byte productId = 1;
            var customerStakeLevelGroup = InstanceFactory.CreateCustomerSettingType(8);

            // Act.
            _settingTypeService.UpdateCustomerSettingType(
                _settingContext,
                customerId,
                customerLevel,
                uplineLevel,
                productId,
                customerStakeLevelGroup);

            // Assert.
            _dataAccess.Received().UpdateCustomerStakeLevelGroups(
                Arg.Is(_settingContext),
                Arg.Is(customerId),
                Arg.Is(customerLevel),
                Arg.Is(uplineLevel),
                Arg.Is(customerStakeLevelGroup.ToXml()));
        }

        [Test]
        public void UpdateCustomerSettingType_UpdatedSuccessfully_ReturnSuccessResult()
        {
            // Arrange.
            var customerId = 1;
            var customerLevel = Level.Agent;
            var uplineLevel = Level.Master;
            byte productId = 1;
            var customerStakeLevelGroup = InstanceFactory.CreateCustomerSettingType(8);
            var successResult = Result.Success;
            _dataAccess.UpdateCustomerStakeLevelGroups(
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
                customerStakeLevelGroup);

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
            var customerStakeLevel = InstanceFactory.CreateCustomerSettingType(8);
            var successResult = Result.Success;
            _dataAccess.UpdateCustomerStakeLevelGroups(
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

        [Test]
        public void Handle_Always_CallGetCustomerStakeLevelGroups()
        {
            // Arrange
            int customerId = 123;
            byte productId = 1;
            short sportTypeId = 1;
            var routeService = new SettingTypeService(_messageService, _resourceService, _logger, _dataAccess);
            var jsonParameters = $"{{\"{nameof(customerId)}\":{customerId},\"{nameof(productId)}\":{productId},\"{nameof(sportTypeId)}\":{sportTypeId}}}";

            // Act
            routeService.Handle(_settingContext, 0, ActionType.Query, jsonParameters);

            // Assert
            _dataAccess.Received().GetCustomerStakeLevelGroups(Arg.Is(123), Arg.Is<byte>(1));
        }

        [Test]
        public void Handle_Always_CallGetCustomerStakeLevelGroupsDetailsWithShowAllIsTrue()
        {
            // Arrange
            int customerId = 123;
            byte productId = 1;
            short sportTypeId = 1;
            var routeService = new SettingTypeService(_messageService, _resourceService, _logger, _dataAccess);
            var jsonParameters = $"{{\"{nameof(customerId)}\":{customerId},\"{nameof(productId)}\":{productId},\"{nameof(sportTypeId)}\":{sportTypeId}}}";
            var stakeLevelGroupRawData = CreateSampleDataOfStakeLevelGroups();

            _dataAccess.GetCustomerStakeLevelGroups(customerId, productId).Returns(stakeLevelGroupRawData);

            // Act
            routeService.Handle(_settingContext, 0, ActionType.Query, jsonParameters);

            // Assert
            int numberOfDetailedItems = 0;
            _dataAccess
                .Received()
                .GetCustomerStakeLevelGroupsDetails(Arg.Is(customerId), Arg.Is(sportTypeId), out numberOfDetailedItems, Arg.Is(true));
        }

        [Test]
        public void Handle_Always_ReturnCustomerStakeLevelGroupDetails()
        {
            // Arrange.
            int customerId = 123;
            byte productId = 1;
            short sportTypeId = 1;
            var routeService = new SettingTypeService(_messageService, _resourceService, _logger, _dataAccess);
            var jsonParameters = $"{{\"{nameof(customerId)}\":{customerId},\"{nameof(productId)}\":{productId},\"{nameof(sportTypeId)}\":{sportTypeId}}}";
            var stakeLevelGroupRawData = CreateSampleDataOfStakeLevelGroups();
            var stakeLevelDetailRawData = CreateSampleDataOfStakeLevelGroupDetails();
            int numberOfDetailedItems = 0;

            _resourceService.GetResourceData("StakeLevel_GG", "1", "en-US").Returns("Level 1");
            _dataAccess.GetCustomerStakeLevelGroups(customerId, productId).Returns(stakeLevelGroupRawData);
            _dataAccess
                .GetCustomerStakeLevelGroupsDetails(customerId, sportTypeId, out numberOfDetailedItems, true)
                .Returns(parameters =>
                {
                    parameters[2] = 1;

                    return stakeLevelDetailRawData;
                });

            // Act.
            var actualCustomerStakeLevelGroupSetting
                = routeService.Handle(_settingContext, 0, ActionType.Query, jsonParameters) as SettingType;
            var actualCustomerStakeLevelGroupItem = actualCustomerStakeLevelGroupSetting.SettingItems.First() as StakeLevelGroupItem;

            // Assert.
            var expectedStakeLevelGroupItem = CreateExpectedStakeLevelGroupItem();
            Assert.AreEqual(expectedStakeLevelGroupItem, actualCustomerStakeLevelGroupItem);
        }

        private List<StakeLevelGroupDto> CreateSampleDataOfStakeLevelGroups()
        {
            return new List<StakeLevelGroupDto>
            {
                new StakeLevelGroupDto
                {
                    BetTypeId = 1,
                    SportTypeId = 1,
                    StakeLevelId = 1,
                    StakeLevelGroupId = 1,
                    Selected = true
                }
            };
        }

        private List<StakeLevelDetailDto> CreateSampleDataOfStakeLevelGroupDetails()
        {
            return new List<StakeLevelDetailDto>
            {
                new StakeLevelDetailDto
                {
                    BetTypeId = 1,
                    SportTypeId = 1,
                    StakeLevelGroupId = 1,
                    StakeLevel = "100 - 1,000"
                }
            };
        }

        private StakeLevelGroupItem CreateExpectedStakeLevelGroupItem()
        {
            return new StakeLevelGroupItem
            {
                BetTypeId = 1,
                SportTypeId = 1,
                ShowMore = false,
                LevelGroups = new List<LevelGroup>
                {
                    new LevelGroup
                    {
                        LevelId = 1,
                        LevelGroupId = 1,
                        LevelGroupName = "Level 1",
                        Selected = true
                    }
                },
                StakeLevelDetailedItems = new List<StakeLevelDetailedItem>
                {
                    new StakeLevelDetailedItem
                    {
                        SportTypeId = 1,
                        BetTypeId = 1,
                        StakeRanges = new List<StakeRange>
                        {
                            new StakeRange
                            {
                                Id = 1,
                                Value = "100 - 1,000"
                            }
                        }
                    }
                }
            };
        }
    }
}