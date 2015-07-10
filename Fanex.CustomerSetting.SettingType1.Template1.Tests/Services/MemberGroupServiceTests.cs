namespace Fanex.CustomerSetting.SettingType1.Template1.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.CustomerSetting.Dtos.Results;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.SettingType1.Template1.DataAccesses;
    using Fanex.CustomerSetting.SettingType1.Template1.Dtos;
    using Fanex.CustomerSetting.SettingType1.Template1.Services;
    using Fanex.CustomerSetting.Tests;
    using Fanex.CustomerSetting.Tests.Models;
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

            _memberGroupService = new MemberGroupService(_memberGroupDataAccess);
        }

        #region Get Grouped Members

        [Test]
        public void GetGroupedMembers_MembersIsNull_ThrowArgumentNullException()
        {
            //// Arrange
            IEnumerable<int> memberIds = null;

            //// Act
            Assert.Throws<ArgumentNullException>(
                new TestDelegate(
                    () => _memberGroupService.GetGroupedMembers(memberIds)));
        }

        [Test]
        public void GetGroupedMembers_MembersIsEmpty_ThrowArgumentOutOfRangeException()
        {
            //// Arrange
            IEnumerable<int> memberIds = new List<int>();

            //// Act
            Assert.Throws<ArgumentOutOfRangeException>(
                new TestDelegate(
                    () => _memberGroupService.GetGroupedMembers(memberIds)));
        }

        [Test]
        public void GetGroupedMembers_MemberGroupDataAccessExecutesGetGroupedMembersMethod()
        {
            //// Arrange
            IEnumerable<int> members = new List<int> { 1, 2 };

            //// Act
            _memberGroupService.GetGroupedMembers(members);

            //// Assert
            _memberGroupDataAccess.Received().GetGroupedMembers(
                Arg.Is<string>(memberIds => memberIds == string.Join(",", members)));
        }

        [Test]
        public void GetGroupedMembers_ReturnGroupedMembers()
        {
            //// Arrange
            IEnumerable<int> members = new List<int> { 1, 2 };
            _memberGroupDataAccess.GetGroupedMembers(Arg.Any<string>()).Returns(new List<int> { 1 });

            //// Act
            var actualGroupedMembers = _memberGroupService.GetGroupedMembers(members);

            //// Assert
            Assert.AreEqual(1, actualGroupedMembers.First());
        }

        #endregion Get Grouped Members

        #region Update Grouped Members Setting

        [Test]
        public void UpdateGroupedMembersSetting_MembersIsNull_ThrowArgumentNullException()
        {
            //// Arrange
            IEnumerable<int> memberIds = null;

            //// Act
            Assert.Throws<ArgumentNullException>(
                new TestDelegate(
                    () => _memberGroupService.UpdateGroupedMembersPT(_settingContext, memberIds, 0, null)));
        }

        [Test]
        public void UpdateGroupedMembersSetting_SettingTypeIsNull_ThrowArgumentNullException()
        {
            //// Arrange
            IEnumerable<int> memberIds = new List<int>();

            //// Act
            Assert.Throws<ArgumentNullException>(
                new TestDelegate(
                    () => _memberGroupService.UpdateGroupedMembersPT(_settingContext, memberIds, 0, null)));
        }

        [Test]
        public void UpdateGroupedMembersSetting_MembersIsEmpty_ThrowArgumentOutOfRangeException()
        {
            //// Arrange
            IEnumerable<int> memberIds = new List<int>();

            //// Act
            Assert.Throws<ArgumentOutOfRangeException>(
                new TestDelegate(
                    () => _memberGroupService.UpdateGroupedMembersPT(_settingContext, memberIds, 0, new SettingType())));
        }

        [Test]
        public void UpdateGroupedMembersSetting_MemberGroupDataAccessExecutesUpdateGroupedMembersSetting()
        {
            //// Arrange
            IEnumerable<int> memberIds = new List<int> { 1, 2 };
            var uplineLevel = CustomerLevel.Master;
            var settingType = InstanceFactory.CreateCustomerSettingType(1);

            //// Act
            _memberGroupService.UpdateGroupedMembersPT(_settingContext, memberIds, CustomerLevel.Master, settingType);

            //// Assert
            _memberGroupDataAccess.Received().UpdateGroupedMembersSetting(
                Arg.Is<UpdateMultipleMemberPTParam>(
                    parameters => parameters.CustIds == string.Join(",", memberIds)
                        && parameters.UserId == _settingContext.Actor.Id
                        && parameters.UplineLevel == uplineLevel
                        && parameters.SubAccId == _settingContext.Actor.SubAccountId
                        && parameters.IP == _settingContext.IP
                        && parameters.PTXml == settingType.ToXml()));
        }

        [Test]
        public void UpdateGroupedMembersSetting_UpdatedSuccessful_ReturnSuccessResult()
        {
            //// Arrange
            IEnumerable<int> memberIds = new List<int> { 1, 2 };
            var settingType = InstanceFactory.CreateCustomerSettingType(1);
            _memberGroupDataAccess.UpdateGroupedMembersSetting(Arg.Any<UpdateMultipleMemberPTParam>())
                .Returns(Result.Success);

            //// Act
            var actualResult = _memberGroupService.UpdateGroupedMembersPT(_settingContext, memberIds, CustomerLevel.Master, settingType);

            //// Assert
            Assert.IsTrue(actualResult.IsSuccessful);
        }

        [Test]
        public void UpdateGroupedMembersSetting_UpdatedSuccessful_ReturnForEachCustomer()
        {
            //// Arrange
            IEnumerable<int> memberIds = new List<int> { 1, 2 };
            var settingType = InstanceFactory.CreateCustomerSettingType(1);
            _memberGroupDataAccess.UpdateGroupedMembersSetting(Arg.Any<UpdateMultipleMemberPTParam>())
                .Returns(Result.Success);

            //// Act
            var actualResult = _memberGroupService.UpdateGroupedMembersPT(_settingContext, memberIds, CustomerLevel.Master, settingType);

            //// Assert
            var updatingResult = actualResult as UpdatingSettingResult;
            Assert.AreEqual(1, updatingResult.SubResults.First().CustomerId);
        }

        #endregion Update Grouped Members Setting
    }
}