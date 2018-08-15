namespace Fanex.CustomerSetting.SettingType15.Tests.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using CustomerSetting.Models;
    using CustomerSetting.Models.Enumerations;
    using CustomerSetting.Models.Requests;
    using CustomerSetting.Services;
    using CustomerSetting.SettingType15.Models;
    using CustomerSetting.Tests;
    using DataAccesses;
    using Dtos;
    using Logging;
    using NSubstitute;
    using NUnit.Framework;
    using SettingType15.Services;

    public class SettingTypeServiceTests
    {
        private ISettingTypeService _settingTypeService;
        private IDataAccess _dataAccess;
        private ISettingContext _settingContext;
        private CustomerSettingQuery _query;

        [SetUp]
        public void SetUp()
        {
            IResourceService _resourceService = Substitute.For<IResourceService>();
            ILogger _logger = Substitute.For<ILogger>();
            IMessageService _messageService = Substitute.For<IMessageService>();

            _dataAccess = Substitute.For<IDataAccess>();
            _settingContext = Substitute.For<ISettingContext>();
            _settingTypeService = new SettingTypeService(_messageService, _resourceService, _logger, _dataAccess);
            _query = InstanceFactory.CreateQuerySettingParam();
        }

        [Test]
        public void GetCustomerSettingType_CustomerIsNotMember_CallGetCustomerWinningShare()
        {
            // Arrange.
            _query.CustomerLevel = Level.Super;

            // Act
            _settingTypeService.GetCustomerSettingType(_settingContext, _query);

            // Assert
            _dataAccess.Received().GetCustomerWinningShare(Arg.Is(_query));
        }

        [Test]
        public void GetCustomerSettingType_CustomerIsNotMember_ReturnCustomerWinningShare()
        {
            // Arrange
            var rawData = new List<WinningShareDto>
            {
                new WinningShareDto
                {
                    BetTypeId = 1,
                    SportTypeId = 1,
                    WinningShare = 1
                }
            };

            _query.CustomerLevel = Level.Super;
            _dataAccess.GetCustomerWinningShare(Arg.Any<CustomerSettingQuery>()).Returns(rawData);

            // Act
            var actualResult = _settingTypeService.GetCustomerSettingType(_settingContext, _query);
            var firstWinningShareItem = actualResult.SettingItems.First() as WinningShareItem;

            // Assert
            var expectedFirstResult = new WinningShareItem
            {
                BetTypeId = 1,
                SportTypeId = 1,
                WinningShare = 1
            };

            Assert.AreEqual(expectedFirstResult.SportTypeId, firstWinningShareItem.SportTypeId);
            Assert.AreEqual(expectedFirstResult.BetTypeId, firstWinningShareItem.BetTypeId);
            Assert.AreEqual(expectedFirstResult.WinningShare, firstWinningShareItem.WinningShare);
        }

        [Test]
        public void GetCustomerSettingType_CustomerIsMember_ReturnEmptyListOfWinningShare()
        {
            // Arrange
            _query.CustomerLevel = Level.Member;

            // Act
            SettingType winningShareSettings = _settingTypeService.GetCustomerSettingType(_settingContext, _query);

            // Assert
            Assert.IsEmpty(winningShareSettings.SettingItems);
        }
    }
}