namespace Fanex.CustomerSetting.SettingType4.Template1.Tests.Services
{
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Dtos.Params;
    using Fanex.CustomerSetting.Dtos.Results;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Services;
    using Fanex.CustomerSetting.SettingType4.Template1.DataAccesses;
    using Fanex.CustomerSetting.SettingType4.Template1.Models;
    using Fanex.CustomerSetting.SettingType4.Template1.Services;
    using Fanex.CustomerSetting.Tests;
    using Fanex.CustomerSetting.Tests.Models;
    using Fanex.Logging;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class SettingTypeServiceTests
    {
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

        private static object[] GetCustomerSettingType_ViewDirectDownlineCustomer_Cases =
        {
            new object[] { Level.Super, null },
            new object[] { Level.Master, null },
            new object[] { Level.Agent, null},
            new object[] { Level.Member, Level.Agent }
        };

        [Test, TestCaseSource("GetCustomerSettingType_ViewDirectDownlineCustomer_Cases")]
        public void GetCustomerSettingType_ViewDirectDownlineCustomer_AlwaysCallGetCustomerProductStatus(
            Level customerLevel,
            Level uplineLevel)
        {
            //// Arrange
            var queryParam = InstanceFactory.CreateQuerySettingParam(1, customerLevel, 1, uplineLevel);
            var productSettingType = InstanceFactory.CreateProductSettingType(1);

            //// Act
            _settingType4Service.GetCustomerSettingType(_settingContext, queryParam, productSettingType);

            //// Assert
            _dataAccess.Received().GetCustomerProductStatus(Arg.Is<QuerySettingParam>(queryParam));
        }

        [Test, TestCaseSource("GetCustomerSettingType_ViewDirectDownlineCustomer_Cases")]
        public void GetCustomerSettingType_ViewDirectDownlineCustomer_AlwaysReturnCustomerProductStatus(
            Level customerLevel,
            Level uplineLevel)
        {
            //// Arrange
            var queryParam = InstanceFactory.CreateQuerySettingParam(1, customerLevel, 1, uplineLevel);
            var productSettingType = InstanceFactory.CreateProductSettingType(1);
            var customerProductStatus = new SettingType
            {
                Id = 1,
                SettingTemplate = 1,
                SettingLevel = 1,
                SettingItems = new List<ProductStatusItem>
                {
                   new ProductStatusItem
                   {
                       SportTypeId = 1
                   }
                }
            };

            //// Stub
            _dataAccess.GetCustomerProductStatus(Arg.Any<QuerySettingParam>()).Returns(customerProductStatus);

            //// Act
            var actualCustomerProductStatus = _settingType4Service.GetCustomerSettingType(_settingContext, queryParam, productSettingType);

            //// Assert
            Assert.AreSame(customerProductStatus, actualCustomerProductStatus);
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
            var queryParam = InstanceFactory.CreateQuerySettingParam(1, customerLevel, 1, uplineLevel);
            var productSettingType = InstanceFactory.CreateProductSettingType(1);

            //// Act
            var actualCustomerProductStatus = _settingType4Service.GetCustomerSettingType(_settingContext, queryParam, productSettingType);

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
                Arg.Is(productId),
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
                Arg.Any<byte>(),
                Arg.Any<string>()).Returns(successResult);

            //// Act
            var actualResult = _settingType4Service.UpdateCustomerSettingType(_settingContext, customerId, customerLevel, uplineLevel, productId, customerProductStatus);

            //// Assert
            Assert.IsTrue(actualResult.IsSuccessful);
        }
    }
}