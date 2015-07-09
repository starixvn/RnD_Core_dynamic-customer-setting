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
            var settingType = InstanceCreator.CreateCustomerSettingType(1);
            var uplineLevel = CustomerLevel.Master;
            IEnumerable<int> memberIds = new List<int> { 1, 2 };

            //// Act
            _settingType1PreTasks.PreUpdateMultipleCustomersSettingType(_settingContext, memberIds, customerLevel, settingType, uplineLevel);

            //// Assert
            _memberGroupService.Received().GetGroupedMembers(
                Arg.Is<IEnumerable<int>>(actualMemberIds => actualMemberIds.Count() == 2 && actualMemberIds.First() == 1));
        }

        [Test]
        public void PreUpdateMultipleSetting_CustomerIsMember_ExecuteUpdateGroupedMembersSettingOnMemberGroupService()
        {
            //// Arrange
            var customerLevel = CustomerLevel.Member;
            var settingType = InstanceCreator.CreateCustomerSettingType(1);
            var uplineLevel = CustomerLevel.Master;
            IEnumerable<int> memberIds = new List<int> { 1, 2 };
            IEnumerable<int> groupedMemberIds = new List<int> { 1 };
            _memberGroupService.GetGroupedMembers(memberIds).Returns(groupedMemberIds);

            //// Act
            _settingType1PreTasks.PreUpdateMultipleCustomersSettingType(_settingContext, memberIds, customerLevel, settingType, uplineLevel);

            //// Assert
            _memberGroupService.Received().UpdateGroupedMembersPT(
                _settingContext,
                Arg.Is<IEnumerable<int>>(
                    membersReceived => membersReceived.Count() == 1
                        && membersReceived.FirstOrDefault() == 1),
                uplineLevel,
                settingType);
        }

        [Test]
        public void PreUpdateMultipleSetting_CustomerIsMember_ReturnResultWithUngroupedMember()
        {
            //// Arrange
            var customerLevel = CustomerLevel.Member;
            var settingType = InstanceCreator.CreateCustomerSettingType(1);
            var uplineLevel = CustomerLevel.Master;
            IEnumerable<int> memberIds = new List<int> { 1, 2 };
            IEnumerable<int> groupedMemberIds = new List<int> { 1 };
            _memberGroupService.GetGroupedMembers(memberIds).Returns(groupedMemberIds);
            _memberGroupService.UpdateGroupedMembersPT(_settingContext, Arg.Any<IEnumerable<int>>(), uplineLevel, settingType)
                .Returns(new UpdatingSettingResult(Result.Success));

            //// Act
            var actualPreTaskResult = _settingType1PreTasks.PreUpdateMultipleCustomersSettingType(_settingContext, memberIds, customerLevel, settingType, uplineLevel);

            //// Assert
            Assert.AreEqual(1, actualPreTaskResult.CustomersNeedForSingleUpdating.Count());
            Assert.AreEqual(2, actualPreTaskResult.CustomersNeedForSingleUpdating.ElementAt(0));
        }
    }
}