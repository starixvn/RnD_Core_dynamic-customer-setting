namespace Fanex.CustomerSetting.IntegrationTests.Products.ProductBase
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using Fanex.CustomerSetting.Dtos.Params;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.IntegrationTests.Models;
    using Fanex.CustomerSetting.IntegrationTests.Products.Product255;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Services;
    using Fanex.CustomerSetting.Services.Impl;
    using Fanex.CustomerSetting.SettingType1.Template1.Models;
    using Fanex.CustomerSetting.SettingType2.Template1.Models;
    using Fanex.CustomerSetting.SettingType3.Template1.Models;
    using Fanex.Data.Configuration;
    using NUnit.Framework;

    public class MasterEditsMember_IntegrationTests
    {
        private IUpdateSettingService _updateSettingService;
        private IQuerySettingService _querySettingService;
        private ISettingContext _settingContext;

        [SetUp]
        public void Setup()
        {
            DbSettingProviderManager.Current.Start();
        }

        protected void MasterViewsMemberSetting(
            int actorId,
            byte actorLevelValue,
            int memberId = SettingInfo.MemberId,
            string memberName = SettingInfo.MemberUserName,
            byte productId = Product255Constants.ProductId,
            string productName = Product255Constants.ProductName,
            byte numOfSettingTypes = Product255Constants.NumOfSettingType)
        {
            //// Arrange
            var actor = new User { Id = actorId, Level = Enumeration.FromValue<UserLevel>(actorLevelValue), Name = "Actor Name" };
            _settingContext = new SettingContext(actor);
            _querySettingService = new QuerySettingService(_settingContext);
            _updateSettingService = new UpdateSettingService(_settingContext);
            var querySettingParam = new QuerySettingParam(memberId, UserLevel.Member, productId, UserLevel.Master);

            //// Act
            var memberSetting = _querySettingService.GetCustomerSetting(querySettingParam);

            //// Assert
            Assert.AreEqual(memberName, memberSetting.CustomerInfo.UserName);
            Assert.AreEqual(memberId, memberSetting.CustomerId);
            Assert.AreEqual(UserLevel.Member, memberSetting.CustomerLevel);
            Assert.AreEqual(productId, memberSetting.ProductId);
            Assert.AreEqual(productName, memberSetting.ProductName);
            Assert.AreEqual(numOfSettingTypes - 1, memberSetting.SettingTypes.Count());

            var masterPTOnMember = memberSetting.SettingTypes.ElementAt(0) as MasterPTOnMember;
            Assert.IsNotNull(masterPTOnMember);

            var masterPTOnMemberItem = memberSetting.SettingTypes.ElementAt(0).SettingItems.ElementAt(0) as MasterPTOnMemberItem;
            Assert.IsNotNull(masterPTOnMemberItem);
            Assert.GreaterOrEqual(masterPTOnMemberItem.IM1 ? masterPTOnMemberItem.UM1 : masterPTOnMemberItem.M1, 0);
            Assert.GreaterOrEqual(masterPTOnMemberItem.A1, 1);

            var memberBetLimitItem = memberSetting.SettingTypes.ElementAt(1).SettingItems.ElementAt(0) as BetLimitItem;
            Assert.IsNotNull(memberBetLimitItem);
            Assert.GreaterOrEqual(memberBetLimitItem.MinBet, 0);
            Assert.GreaterOrEqual(memberBetLimitItem.MaxBet, 0);
            Assert.GreaterOrEqual(memberBetLimitItem.MaxPerMatch, 0);

            var memberCommissionItem = memberSetting.SettingTypes.ElementAt(2).SettingItems.ElementAt(0) as CommissionItem;
            Assert.IsNotNull(memberCommissionItem);
            Assert.GreaterOrEqual(memberCommissionItem.CommRate, 0);
            Assert.LessOrEqual(memberCommissionItem.CommRate, 0.01);
            //// Agent comm
            Assert.GreaterOrEqual(memberCommissionItem.Upline.CommRate, 0);
            Assert.LessOrEqual(memberCommissionItem.Upline.CommRate, 0.01);
        }

        protected void MasterUpdatesPTOnMember(
            int actorId = SettingInfo.MasterId,
            byte productId = Product255Constants.ProductId,
            int memberId = SettingInfo.MemberId)
        {
            //// Update two time for auto pt and not auto pt
            MasterUpdatesPTOnMember(actorId, UserLevel.Master, memberId, productId, true);
            MasterUpdatesPTOnMember(actorId, UserLevel.Master, memberId, productId, false);
        }

        protected void CompanyUpdatesMasterPTOnMember(
            int actorId = SettingInfo.CompanyId,
            byte productId = Product255Constants.ProductId,
            int memberId = SettingInfo.MemberId)
        {
            //// Update two time for auto pt and not auto pt
            MasterUpdatesPTOnMember(actorId, UserLevel.Company, memberId, productId, true);
            MasterUpdatesPTOnMember(actorId, UserLevel.Company, memberId, productId, false);
        }

        private void MasterUpdatesPTOnMember(
            int actorId,
            UserLevel actorLevel,
            int memberId,
            byte productId,
            bool isDisbaledProductStatus)
        {
            //// Arrange
            var actor = new User { Id = actorId, Level = actorLevel, Name = "ActorName" };
            _settingContext = new SettingContext(actor);
            _querySettingService = new QuerySettingService(_settingContext);
            _updateSettingService = new UpdateSettingService(_settingContext);
            var querySettingParam = new QuerySettingParam(memberId, UserLevel.Member, productId, UserLevel.Master);

            var memberSetting = _querySettingService.GetCustomerSetting(querySettingParam);
            var masterPTOnMember = memberSetting.SettingTypes.ElementAt(0) as MasterPTOnMember;
            var masterPTOnMemberItem = memberSetting.SettingTypes.ElementAt(0).SettingItems.ElementAt(0) as MasterPTOnMemberItem;
            if (!masterPTOnMember.IsDisableMasterAutoPT)
            {
                if (masterPTOnMemberItem.IM2)
                {
                    masterPTOnMemberItem.IM1 = true;
                }
                else
                {
                    masterPTOnMemberItem.IM1 = !masterPTOnMemberItem.IM1;
                }
            }

            if (masterPTOnMemberItem.IM1)
            {
                masterPTOnMemberItem.UM1 = (byte)(masterPTOnMemberItem.UM1 == 21 ? 22 : 21);
                masterPTOnMemberItem.M1 = 0;
            }
            else
            {
                masterPTOnMemberItem.M1 = (byte)(masterPTOnMemberItem.M1 == 21 ? 22 : 21);
                masterPTOnMemberItem.UM1 = 0;
            }

            memberSetting.SettingTypes = memberSetting.SettingTypes.Take(1);

            //// Act
            var updateParam = new UpdatingSettingParam(memberSetting, querySettingParam.UplineLevel);
            var updateResult = _updateSettingService.UpdateCustomerSetting(updateParam);

            //// Assert
            Assert.IsTrue(updateResult.IsSuccessful);

            Thread.Sleep(TestConfig.NumOfSleepSecondsWaitingForReplication); //// Wait for replication data.

            var newMemberSetting = _querySettingService.GetCustomerSetting(querySettingParam);
            var newMasterOnMemberPTItem = newMemberSetting.SettingTypes.ElementAt(0).SettingItems.ElementAt(0) as MasterPTOnMemberItem;
            if (masterPTOnMemberItem.IM1)
            {
                Assert.AreEqual(masterPTOnMemberItem.UM1, newMasterOnMemberPTItem.UM1);
            }
            else
            {
                Assert.AreEqual(masterPTOnMemberItem.M1, newMasterOnMemberPTItem.M1);
            }
        }

        protected void MasterUpdatesPTOnMultipleMembers(
            int masterId = SettingInfo.MasterId,
            byte productId = Product255Constants.ProductId,
            IEnumerable<int> memberIds = null,
            IEnumerable<string> memberNames = null)
        {
            memberIds = memberIds ?? SettingInfo.MemberIds;
            memberNames = memberNames ?? SettingInfo.MemberNames;

            //// Arrange
            var actor = new User { Id = masterId, Level = UserLevel.Master, Name = "ActorName" };
            _settingContext = new SettingContext(actor);
            _querySettingService = new QuerySettingService(_settingContext);
            _updateSettingService = new UpdateSettingService(_settingContext);
            var querySettingParam = new QuerySettingParam(memberIds.First(), UserLevel.Member, productId, UserLevel.Master);

            var memberSetting = _querySettingService.GetCustomerSetting(querySettingParam);
            var masterPTOnMember = memberSetting.SettingTypes.ElementAt(0) as MasterPTOnMember;
            var masterPTOnMemberItem = memberSetting.SettingTypes.ElementAt(0).SettingItems.ElementAt(0) as MasterPTOnMemberItem;

            if (!masterPTOnMember.IsDisableMasterAutoPT)
            {
                if (masterPTOnMemberItem.IM2)
                {
                    masterPTOnMemberItem.IM1 = true;
                }
                else
                {
                    masterPTOnMemberItem.IM1 = !masterPTOnMemberItem.IM1;
                }
            }

            if (masterPTOnMemberItem.IM1)
            {
                masterPTOnMemberItem.UM1 = (byte)(masterPTOnMemberItem.UM1 == 21 ? 22 : 21);
                masterPTOnMemberItem.M1 = 0;
            }
            else
            {
                masterPTOnMemberItem.M1 = (byte)(masterPTOnMemberItem.M1 == 21 ? 22 : 21);
                masterPTOnMemberItem.UM1 = 0;
            }

            memberSetting.SettingTypes = memberSetting.SettingTypes.Take(1);

            //// Act
            var updatingMultipleParam = new UpdatingMultipleSettingParam(memberSetting, memberIds, memberNames, querySettingParam.UplineLevel);
            var updateResult = _updateSettingService.UpdateMultipleCustomerSetting(updatingMultipleParam);

            //// Assert
            Assert.IsTrue(updateResult.IsSuccessful);

            Thread.Sleep(TestConfig.NumOfSleepSecondsWaitingForReplication * 2); //// Wait for replication data.

            foreach (var memberId in memberIds)
            {
                querySettingParam.CustomerId = memberId;
                var newMemberSetting = _querySettingService.GetCustomerSetting(querySettingParam);
                var newMasterOnMemberPTItem = newMemberSetting.SettingTypes.ElementAt(0).SettingItems.ElementAt(0) as MasterPTOnMemberItem;
                if (masterPTOnMemberItem.IM1)
                {
                    Assert.AreEqual(masterPTOnMemberItem.UM1, newMasterOnMemberPTItem.UM1);
                }
                else
                {
                    Assert.AreEqual(masterPTOnMemberItem.M1, newMasterOnMemberPTItem.M1);
                }
            }
        }
    }
}