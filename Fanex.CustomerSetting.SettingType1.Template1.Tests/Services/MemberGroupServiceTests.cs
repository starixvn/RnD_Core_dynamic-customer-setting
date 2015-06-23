namespace Fanex.CustomerSetting.SettingType1.Template1.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Tests;
    using Fanex.CustomerSetting.Tests.Models;
    using Fanex.CustomerSetting.SettingType1.Template1.DataAccesses;
    using Fanex.CustomerSetting.SettingType1.Template1.Dtos;
    using Fanex.CustomerSetting.SettingType1.Template1.Services;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class MemberGroupServiceTests
    {
        private ISettingContext _settingContext;
        private IMemberGroupService _memberGroupService;
        private IMemberGroupDataAccess _memberGroupDataAccess;

        [SetUp]
        public void Setup()
        {
            _settingContext = new SettingContextStub();
            _memberGroupDataAccess = Substitute.For<IMemberGroupDataAccess>();

            _memberGroupService = new MemberGroupService(_settingContext, _memberGroupDataAccess);
        }

        #region Get Grouped Members

        [Test]
        public void GetGroupedMembers_MembersIsNull_ThrowArgumentNullException()
        {
            //// Arrange
            IEnumerable<Customer> customers = null;

            //// Act
            Assert.Throws<ArgumentNullException>(
                new TestDelegate(
                    () => _memberGroupService.GetGroupedMembers(customers)));
        }

        [Test]
        public void GetGroupedMembers_MembersIsEmpty_ThrowArgumentOutOfRangeException()
        {
            //// Arrange
            IEnumerable<Customer> members = new List<Customer>();

            //// Act
            Assert.Throws<ArgumentOutOfRangeException>(
                new TestDelegate(
                    () => _memberGroupService.GetGroupedMembers(members)));
        }

        [Test]
        public void GetGroupedMembers_ExecuteGetGroupedMembersOnMemberGroupDataAccess()
        {
            //// Arrange
            IEnumerable<Customer> members = new List<Customer>
            {
                new Customer { Id = 1 },
                new Customer { Id = 2 }
            };

            //// Act
            _memberGroupService.GetGroupedMembers(members);

            //// Assert
            _memberGroupDataAccess.Received().GetGroupedMembers(
                Arg.Is<string>(memberIds => memberIds == string.Join(",", members.Select(member => member.Id))));
        }

        #endregion Get Grouped Members

        #region Update Grouped Members Setting

        [Test]
        public void UpdateGroupedMembersSetting_MembersIsNull_ThrowArgumentNullException()
        {
            //// Arrange
            IEnumerable<Customer> members = null;

            //// Act
            Assert.Throws<ArgumentNullException>(
                new TestDelegate(
                    () => _memberGroupService.UpdateGroupedMembersSetting(members, 0, null)));
        }

        [Test]
        public void UpdateGroupedMembersSetting_MembersIsEmpty_ThrowArgumentOutOfRangeException()
        {
            //// Arrange
            IEnumerable<Customer> members = new List<Customer>();

            //// Act
            Assert.Throws<ArgumentOutOfRangeException>(
                new TestDelegate(
                    () => _memberGroupService.UpdateGroupedMembersSetting(members, 0, null)));
        }

        [Test]
        public void UpdateGroupedMembersSetting_ExecuteUpdateGroupedMembersSettingOnMemberGroupDataAccess()
        {
            //// Arrange
            IEnumerable<Customer> members = new List<Customer>
            {
                new Customer { Id = 1 },
                new Customer { Id = 2 }
            };
            var uplineLevel = CustomerLevel.Master;
            var settingType = FakeObjectManager.CreateSettingTypeFakeObject(1);

            //// Act
            _memberGroupService.UpdateGroupedMembersSetting(members, CustomerLevel.Master, settingType);

            //// Assert
            _memberGroupDataAccess.Received().UpdateGroupedMembersSetting(
                Arg.Is<UpdateMultipleMemberPositionTakingParams>(
                    parameters => parameters.CustIds == string.Join(",", members.Select(member => member.Id))
                        && parameters.UserId == _settingContext.Actor.Id
                        && parameters.UplineLevel == uplineLevel
                        && parameters.SubAccId == _settingContext.SubAccount.Id
                        && parameters.IP == _settingContext.IP
                        && parameters.PositionTakingXml == settingType.ToXml()));
        }

        #endregion Update Grouped Members Setting
    }
}