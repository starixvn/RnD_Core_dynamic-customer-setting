namespace Fanex.CustomerSetting.SettingType1.Template1.Tests.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.CustomerSetting.Dtos.Params;
    using Fanex.CustomerSetting.Dtos.Results;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Services;
    using Fanex.CustomerSetting.SettingType1.Template1.DataAccesses;
    using Fanex.CustomerSetting.SettingType1.Template1.Models;
    using Fanex.CustomerSetting.SettingType1.Template1.Services;
    using Fanex.CustomerSetting.Tests;
    using Fanex.CustomerSetting.Tests.Models;
    using Fanex.Logging;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class SettingTypeServiceTests
    {
        private ISettingTypeService _settingType1Service;
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

            _settingType1Service = new SettingTypeService(_messageService, _resourceService, _logger, _dataAccess);
        }

        #region Get Customer Setting Type Tests

        [Test]
        public void GetCustomerSettingType_Always_CallGetCustomerPT()
        {
            //// Arrange
            var queryParam = InstanceFactory.CreateQuerySettingParam();
            var productSettingType = InstanceFactory.CreateProductSettingType(1);

            //// Act
            _settingType1Service.GetCustomerSettingType(_settingContext, queryParam, productSettingType);

            //// Assert
            _dataAccess.Received().GetCustomerPT(Arg.Is<QuerySettingParam>(queryParam));
        }

        [Test]
        public void GetCustomerSettingType_Always_ReturnCustomerPT()
        {
            //// Arrange
            var queryParam = InstanceFactory.CreateQuerySettingParam();
            var productSettingType = InstanceFactory.CreateProductSettingType(1);
            var customerPT = new SettingType
            {
                Id = 1,
                SettingTemplate = 1,
                SettingLevel = 1,
                SettingItems = new List<MasterPTOnMemberItem>
                {
                   new MasterPTOnMemberItem
                   {
                       SportTypeId = 1
                   }
                }
            };

            //// Stub
            _dataAccess.GetCustomerPT(Arg.Any<QuerySettingParam>()).Returns(customerPT);

            //// Act
            var actualCustomerPT = _settingType1Service.GetCustomerSettingType(_settingContext, queryParam, productSettingType);

            //// Assert
            Assert.AreSame(customerPT, actualCustomerPT);
        }

        #endregion Get Customer Setting Type Tests

        #region Update Customer Setting Type Tests

        [Test]
        public void UpdateCustomerSettingType_Always_CallUpdateCustomerPT()
        {
            //// Arrange
            var customerId = 1;
            var customerLevel = Level.Member;
            var uplineLevel = Level.Agent;
            byte productId = 1;
            var customerPT = InstanceFactory.CreateCustomerSettingType(1);

            //// Act
            _settingType1Service.UpdateCustomerSettingType(_settingContext, customerId, customerLevel, uplineLevel, productId, customerPT);

            //// Assert
            _dataAccess.Received().UpdateCustomerPT(
                Arg.Is(_settingContext),
                Arg.Is(customerId),
                Arg.Is(customerLevel),
                Arg.Is(uplineLevel),
                Arg.Is(productId),
                Arg.Is(customerPT.ToXml()));
        }

        [Test]
        public void UpdateCustomerSettingType_UpdatedSuccessfully_ReturnSuccessResult()
        {
            //// Arrange
            var customerId = 1;
            var customerLevel = Level.Member;
            var uplineLevel = Level.Agent;
            byte productId = 1;
            var customerPT = InstanceFactory.CreateCustomerSettingType(1);
            var successResult = Result.Success;

            //// Stub
            _dataAccess.UpdateCustomerPT(
                Arg.Any<ISettingContext>(),
                Arg.Any<int>(),
                Arg.Any<Level>(),
                Arg.Any<Level>(),
                Arg.Any<byte>(),
                Arg.Any<string>()).Returns(successResult);

            //// Act
            var actualResult = _settingType1Service.UpdateCustomerSettingType(_settingContext, customerId, customerLevel, uplineLevel, productId, customerPT);

            //// Assert
            Assert.IsTrue(actualResult.IsSuccessful);
        }

        #endregion Update Customer Setting Type Tests

        #region Update Multiple Customers Setting Type

        [Test]
        public void UpdateMultipleCustomersSettingType_CustomerIsMember_AlwaysCallGetGroupedMemberIds()
        {
            //// Arrange
            var customerIds = new List<int> { 1, 2 };
            var customerLevel = Level.Member;
            var uplineLevel = Level.Agent;
            byte productId = 1;
            var customerPT = InstanceFactory.CreateCustomerSettingType(1);

            //// Act
            _settingType1Service.UpdateMultipleCustomersSettingType(_settingContext, customerIds, customerLevel, uplineLevel, productId, customerPT);

            //// Assert
            _dataAccess.Received().GetGroupedMemberIds(Arg.Is("1,2"));
        }

        [Test]
        public void UpdateMultipleCustomersSettingType_MemberIsGrouped_AlwaysCallUpdateGroupedMembersSetting()
        {
            //// Arrange
            var customerIds = new List<int> { 1, 2 };
            var customerLevel = Level.Member;
            var uplineLevel = Level.Agent;
            byte productId = 1;
            var customerPT = InstanceFactory.CreateCustomerSettingType(1);

            //// Stub
            var groupedMemberIds = new List<int> { 1 };
            _dataAccess.GetGroupedMemberIds(Arg.Any<string>()).Returns(groupedMemberIds);

            //// Act
            _settingType1Service.UpdateMultipleCustomersSettingType(_settingContext, customerIds, customerLevel, uplineLevel, productId, customerPT);

            //// Assert
            _dataAccess.Received().UpdateGroupedMembersSetting(
                Arg.Is(_settingContext.Actor.Id),
                Arg.Is(_settingContext.Actor.SubAccountId),
                Arg.Is("1"),
                Arg.Is(uplineLevel),
                Arg.Is(customerPT.ToXml()),
                Arg.Is(_settingContext.IP));
        }

        [Test]
        public void UpdateMultipleCustomersSettingType_Always_CallUpdateCustomerSettingType()
        {
            //// Arrange
            var customerIds = new List<int> { 1, 2 };
            var customerLevel = Level.Member;
            var uplineLevel = Level.Agent;
            byte productId = 1;
            var customerPT = InstanceFactory.CreateCustomerSettingType(1);

            //// Act
            _settingType1Service.UpdateMultipleCustomersSettingType(_settingContext, customerIds, customerLevel, uplineLevel, productId, customerPT);

            //// Assert
            _dataAccess.Received().UpdateCustomerPT(
                Arg.Is(_settingContext),
                Arg.Is<int>(customerId => customerIds.Contains(customerId)),
                Arg.Is(customerLevel),
                Arg.Is(uplineLevel),
                Arg.Is(productId),
                Arg.Is(customerPT.ToXml()));
        }

        [Test]
        public void UpdateMultipleCustomersSettingType_UpdatedSuccessful_ReturnSuccessResultList()
        {
            //// Arrange
            var customerIds = new List<int> { 1, 2 };
            var customerLevel = Level.Agent;
            Level uplineLevel = null;
            byte productId = 1;
            var customerPT = InstanceFactory.CreateCustomerSettingType(1);
            var successResult = Result.Success;

            //// Stub
            _dataAccess.UpdateCustomerPT(
                Arg.Any<ISettingContext>(),
                Arg.Any<int>(),
                Arg.Any<Level>(),
                Arg.Any<Level>(),
                Arg.Any<byte>(),
                Arg.Any<string>()).Returns(successResult);

            //// Act
            var actualResults = _settingType1Service.UpdateMultipleCustomersSettingType(_settingContext, customerIds, customerLevel, uplineLevel, productId, customerPT);

            //// Assert
            Assert.IsTrue(actualResults.All(actualResult => actualResult.IsSuccessful));
        }

        [Test]
        public void UpdateMultipleCustomersSettingType_MemberIsGrouped_ReturnMergedGroupedWithUnGroupedResuts()
        {
            //// Arrange
            var customerIds = new List<int> { 1, 2, 3 };
            var customerLevel = Level.Member;
            var uplineLevel = Level.Agent;
            byte productId = 1;
            var customerPT = InstanceFactory.CreateCustomerSettingType(1);

            var groupedMemberIds = new List<int> { 1, 2 };
            _dataAccess.GetGroupedMemberIds(Arg.Any<string>()).Returns(groupedMemberIds);
            _dataAccess.UpdateGroupedMembersSetting(
                Arg.Any<int>(),
                Arg.Any<int>(),
                Arg.Any<string>(),
                Arg.Any<Level>(),
                Arg.Any<string>(),
                Arg.Any<string>()).Returns(Result.Success);

            //// Act
            var actualResults = _settingType1Service.UpdateMultipleCustomersSettingType(_settingContext, customerIds, customerLevel, uplineLevel, productId, customerPT);

            //// Assert
            Assert.AreEqual(3, actualResults.Count());
        }

        #endregion Update Multiple Customers Setting Type
    }
}