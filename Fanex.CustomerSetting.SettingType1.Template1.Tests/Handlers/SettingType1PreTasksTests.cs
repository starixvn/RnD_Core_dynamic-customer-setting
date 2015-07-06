namespace Fanex.CustomerSetting.SettingType1.Template1.Tests.Handlers
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.CustomerSetting.Dtos.Results;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.SettingType1.Template1.Handlers;
    using Fanex.CustomerSetting.SettingType1.Template1.Services;
    using Fanex.CustomerSetting.Tests;
    using Fanex.CustomerSetting.Tests.Models;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class SettingType1PreTasksTests
    {
        private IMemberGroupService _memberGroupService;
        private ISettingContext _settingContext;
        private SettingType1PreTasks _settingType1PreTasks;

        [SetUp]
        public void Setup()
        {
            _settingContext = new SettingContextStub();
            _memberGroupService = Substitute.For<IMemberGroupService>();

            _settingType1PreTasks = new SettingType1PreTasks(_memberGroupService);
        }

        [TestCase(CustomerLevel.Super)]
        [TestCase(CustomerLevel.Master)]
        [TestCase(CustomerLevel.Agent)]
        public void PreUpdateMultipleSetting_CustomerIsNotMember_ReturnSuccessResult(CustomerLevel customerLevel)
        {
            //// Act
            var result = _settingType1PreTasks.PreUpdateMultipleCustomersSettingType(_settingContext, null, customerLevel, null, null);

            //// Assert
            Assert.IsTrue(result.Result.IsSuccessful);
        }

        [Test]
        public void PreUpdateMultipleSetting_CustomerIsMember_ExecuteGetGroupedMembersOnMemberGroupService()
        {
            //// Arrange
            var customerLevel = CustomerLevel.Member;
            var settingType = FakeObjectManager.CreateSettingType(1);
            var uplineLevel = CustomerLevel.Master;
            IEnumerable<Customer> members = new List<Customer>
            {
                new Customer { Id = 1 },
                new Customer { Id = 2 }
            };

            //// Act
            _settingType1PreTasks.PreUpdateMultipleCustomersSettingType(_settingContext, members, customerLevel, settingType, uplineLevel);

            //// Assert
            _memberGroupService.Received().GetGroupedMembers(members);
        }

        [Test]
        public void PreUpdateMultipleSetting_CustomerIsMember_ExecuteUpdateGroupedMembersSettingOnMemberGroupService()
        {
            //// Arrange
            var customerLevel = CustomerLevel.Member;
            var settingType = FakeObjectManager.CreateSettingType(1);
            var uplineLevel = CustomerLevel.Master;
            IEnumerable<Customer> members = new List<Customer>
            {
                new Customer { Id = 1 },
                new Customer { Id = 2 }
            };
            IEnumerable<Customer> groupedMembers = new List<Customer> { new Customer { Id = 1 } };
            _memberGroupService.GetGroupedMembers(members).Returns(groupedMembers);

            //// Act
            _settingType1PreTasks.PreUpdateMultipleCustomersSettingType(_settingContext, members, customerLevel, settingType, uplineLevel);

            //// Assert
            _memberGroupService.Received().UpdateGroupedMembersPT(
                _settingContext,
                Arg.Is<IEnumerable<Customer>>(
                    membersReceived => membersReceived.Count() == 1
                        && membersReceived.FirstOrDefault().Id == 1),
                uplineLevel,
                settingType);
        }

        [Test]
        public void PreUpdateMultipleSetting_CustomerIsMember_ReturnResultWithUngroupedMember()
        {
            //// Arrange
            var customerLevel = CustomerLevel.Member;
            var settingType = FakeObjectManager.CreateSettingType(1);
            var uplineLevel = CustomerLevel.Master;
            IEnumerable<Customer> members = new List<Customer>
            {
                new Customer { Id = 1 },
                new Customer { Id = 2 }
            };
            IEnumerable<Customer> groupedMembers = new List<Customer> { new Customer { Id = 1 } };
            _memberGroupService.GetGroupedMembers(members).Returns(groupedMembers);
            _memberGroupService.UpdateGroupedMembersPT(_settingContext, Arg.Any<IEnumerable<Customer>>(), uplineLevel, settingType)
                .Returns(UpdatingSettingResult.SuccessUpdatingSettingResult);

            //// Act
            var actualPreTaskResult = _settingType1PreTasks.PreUpdateMultipleCustomersSettingType(_settingContext, members, customerLevel, settingType, uplineLevel);

            //// Assert
            Assert.AreEqual(1, actualPreTaskResult.CustomersNeedForSingleUpdating.Count());
            Assert.AreEqual(2, actualPreTaskResult.CustomersNeedForSingleUpdating.ElementAt(0).Id);
        }
    }
}