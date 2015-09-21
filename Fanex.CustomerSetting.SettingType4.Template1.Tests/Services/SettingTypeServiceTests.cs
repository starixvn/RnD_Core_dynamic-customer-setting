namespace Fanex.CustomerSetting.SettingType4.Tests.Services
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Models.Enumerations;
    using Fanex.CustomerSetting.Models.Requests;
    using Fanex.CustomerSetting.Models.Results;
    using Fanex.CustomerSetting.Services;
    using Fanex.CustomerSetting.SettingType4.DataAccesses;
    using Fanex.CustomerSetting.SettingType4.Dtos;
    using Fanex.CustomerSetting.SettingType4.Models;
    using Fanex.CustomerSetting.SettingType4.Services;
    using Fanex.CustomerSetting.Tests;
    using Fanex.CustomerSetting.Tests.Models;
    using Fanex.Logging;
    using NSubstitute;
    using NUnit.Framework;

    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1310:FieldNamesMustNotContainUnderscore", Justification = "Reviewed.")]
    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1306:FieldNamesMustBeginWithLowerCaseLetter", Justification = "Reviewed.")]
    [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1013:ClosingCurlyBracketsMustBeSpacedCorrectly", Justification = "Reviewed.")]
    [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1204:StaticElementsMustAppearBeforeInstanceElements", Justification = "Reviewed.")]
    [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1012:OpeningCurlyBracketsMustBeSpacedCorrectly", Justification = "Reviewed.")]
    [TestFixture]
    public class SettingTypeServiceTests
    {
        private static object[] GetCustomerSettingType_ViewDirectDownlineCustomer_Cases =
        {
            new object[] { Level.Super, null },
            new object[] { Level.Master, null },
            new object[] { Level.Agent, null},
            new object[] { Level.Member, Level.Agent }
        };

        private ISettingTypeService _settingType4Service;
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

            _settingType4Service = new SettingTypeService(_messageService, _resourceService, _logger, _dataAccess);
        }

        [Test, TestCaseSource("GetCustomerSettingType_ViewDirectDownlineCustomer_Cases")]
        public void GetCustomerSettingType_ViewDirectDownlineCustomer_AlwaysCallGetCustomerProductStatus(
            Level customerLevel,
            Level uplineLevel)
        {
            //// Arrange
            var query = InstanceFactory.CreateQuerySettingParam(1, customerLevel, 1, uplineLevel);

            //// Act
            _settingType4Service.GetCustomerSettingType(_settingContext, query);

            //// Assert
            _dataAccess.Received().GetCustomerProductStatusItemsRawData(Arg.Is<CustomerSettingQuery>(query));
        }

        [Test, TestCaseSource("GetCustomerSettingType_ViewDirectDownlineCustomer_Cases")]
        public void GetCustomerSettingType_ViewDirectDownlineCustomer_AlwaysReturnCustomerProductStatus(
            Level customerLevel,
            Level uplineLevel)
        {
            //// Arrange
            var query = InstanceFactory.CreateQuerySettingParam(1, customerLevel, 1, uplineLevel);
            var productStatusItemsRawData = new List<ProductStatusItemDto>
            {
                new ProductStatusItemDto
                {
                    BetTypeId = 1,
                    SportTypeId = 1,
                    IsDisabled = true,
                    IsDisabledDownline = true,
                    IsDisabledUpline = false
                }
            };

            //// Stub
            _dataAccess.GetCustomerProductStatusItemsRawData(Arg.Any<CustomerSettingQuery>()).Returns(productStatusItemsRawData);

            //// Act
            var actualCustomerProductStatus = _settingType4Service.GetCustomerSettingType(_settingContext, query);

            //// Assert
            var actualCustomerProductStatusItem = actualCustomerProductStatus.SettingItems.First() as ProductStatusItem;
            Assert.AreEqual(1, actualCustomerProductStatusItem.SportTypeId);
            Assert.AreEqual(1, actualCustomerProductStatusItem.BetTypeId);
            Assert.AreEqual(true, actualCustomerProductStatusItem.IsDisabled);
            Assert.AreEqual(false, actualCustomerProductStatusItem.Upline.IsDisabled);
            Assert.AreEqual(true, actualCustomerProductStatusItem.Downline.IsDisabled);
        }

        private static object[] GetCustomerSettingType_ViewInDirectDownlineCustomer_Cases =
        {
            new object[] { Level.Member, Level.Super },
            new object[] { Level.Member, Level.Master }
        };

        [Test, TestCaseSource("GetCustomerSettingType_ViewInDirectDownlineCustomer_Cases")]
        public void GetCustomerSettingType_ViewInDirectDownlineCustomer_AlwaysReturnNoSettingItem(
            Level customerLevel,
            Level uplineLevel)
        {
            //// Arrange
            var query = InstanceFactory.CreateQuerySettingParam(1, customerLevel, 1, uplineLevel);

            //// Act
            var actualCustomerProductStatus = _settingType4Service.GetCustomerSettingType(_settingContext, query);

            //// Assert
            Assert.IsEmpty(actualCustomerProductStatus.SettingItems);
        }

        [Test]
        public void UpdateCustomerSettingType_Always_CallUpdateCustomerProductStatus()
        {
            //// Arrange
            var customerId = 1;
            var customerLevel = Level.Member;
            var uplineLevel = Level.Agent;
            byte productId = 1;
            var customerProductStatus = InstanceFactory.CreateCustomerSettingType(1);

            //// Act
            _settingType4Service.UpdateCustomerSettingType(_settingContext, customerId, customerLevel, uplineLevel, productId, customerProductStatus);

            //// Assert
            _dataAccess.Received().UpdateCustomerProductStatus(
                Arg.Is(_settingContext),
                Arg.Is(customerId),
                Arg.Is(customerLevel),
                Arg.Is(uplineLevel),
                Arg.Is(customerProductStatus.ToXml()));
        }

        [Test]
        public void UpdateCustomerSettingType_UpdatedSuccessfully_ReturnSuccessResult()
        {
            //// Arrange
            var customerId = 1;
            var customerLevel = Level.Member;
            var uplineLevel = Level.Agent;
            byte productId = 1;
            var customerProductStatus = InstanceFactory.CreateCustomerSettingType(1);
            var successResult = Result.Success;

            //// Stub
            _dataAccess.UpdateCustomerProductStatus(
                Arg.Any<ISettingContext>(),
                Arg.Any<int>(),
                Arg.Any<Level>(),
                Arg.Any<Level>(),
                Arg.Any<string>()).Returns(successResult);

            //// Act
            var actualResult = _settingType4Service.UpdateCustomerSettingType(_settingContext, customerId, customerLevel, uplineLevel, productId, customerProductStatus);

            //// Assert
            Assert.IsTrue(actualResult.IsSuccessful);
        }
    }
}