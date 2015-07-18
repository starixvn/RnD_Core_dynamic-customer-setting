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

    public class SuperEditsMember_IntegrationTests
    {
        private IUpdateSettingService _updateSettingService;
        private IQuerySettingService _querySettingService;
        private ISettingContext _settingContext;

        [SetUp]
        public void Setup()
        {
            DbSettingProviderManager.Current.Start();
        }

        protected void SuperViewsMemberSetting(
            int actorId,
            Level actorLevel,
            int memberId = SettingInfo.MemberId,
            string memberName = SettingInfo.MemberUserName,
            byte productId = Product255Constants.ProductId,
            string productName = Product255Constants.ProductName,
            byte numOfSettingTypes = Product255Constants.NumOfSettingType)
        {
            //// Arrange
            var actor = new User { Id = actorId, Level = actorLevel, Name = "Actor Name" };
            _settingContext = new SettingContext(actor);
            _querySettingService = new QuerySettingService(_settingContext);
            _updateSettingService = new UpdateSettingService(_settingContext);
            var querySettingParam = new QuerySettingParam(memberId, Level.Member, productId, Level.Super);

            //// Act
            var memberSetting = _querySettingService.GetCustomerSetting(querySettingParam);

            //// Assert
            Assert.AreEqual(memberName, memberSetting.CustomerInfo.UserName);
            Assert.AreEqual(memberId, memberSetting.CustomerId);
            Assert.AreEqual(Level.Member, memberSetting.CustomerLevel);
            Assert.AreEqual(productId, memberSetting.ProductId);
            Assert.AreEqual(productName, memberSetting.ProductName);
            Assert.AreEqual(numOfSettingTypes - 1, memberSetting.SettingTypes.Count());

            var superPTOnMember = memberSetting.SettingTypes.ElementAt(0).SettingItems.ElementAt(0) as SuperPTOnMemberItem;
            Assert.IsNotNull(superPTOnMember);
            Assert.GreaterOrEqual(superPTOnMember.IS1 ? superPTOnMember.US1 : superPTOnMember.S1, 0);
            Assert.GreaterOrEqual(superPTOnMember.M1, 1);
            Assert.GreaterOrEqual(superPTOnMember.A1, 1);

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
            //// Master Comm
            ////Assert.GreaterOrEqual(memberCommissionItem.Upline.Upline.CommRate, 0);
            ////Assert.LessOrEqual(memberCommissionItem.Upline.Upline.CommRate, 0.01);
        }

        protected void SuperUpdatesPTOnMember(
            int actorId = SettingInfo.SuperId,
            byte productId = Product255Constants.ProductId,
            int memberId = SettingInfo.MemberId)
        {
            SuperUpdatesPTOnMember(actorId, Level.Super, memberId, productId, true);
            SuperUpdatesPTOnMember(actorId, Level.Super, memberId, productId, false);
        }

        protected void CompanyUpdatesSuperPTOnMember(
            int actorId = SettingInfo.CompanyId,
            byte productId = Product255Constants.ProductId,
            int memberId = SettingInfo.MemberId)
        {
            SuperUpdatesPTOnMember(actorId, Level.Company, memberId, productId, true);
            SuperUpdatesPTOnMember(actorId, Level.Company, memberId, productId, false);
        }

        private void SuperUpdatesPTOnMember(
            int actorId,
            Level actorLevel,
            int memberId,
            byte productId,
            bool isDisabledProductStatus)
        {
            //// Arrange
            var actor = new User { Id = actorId, Level = actorLevel, Name = "ActorName" };
            _settingContext = new SettingContext(actor);
            _querySettingService = new QuerySettingService(_settingContext);
            _updateSettingService = new UpdateSettingService(_settingContext);

            var querySettingParam = new QuerySettingParam(memberId, Level.Member, productId, Level.Super);

            var memberSetting = _querySettingService.GetCustomerSetting(querySettingParam);
            var superPTOnMemberItem = memberSetting.SettingTypes.ElementAt(0).SettingItems.ElementAt(0) as SuperPTOnMemberItem;

            if (superPTOnMemberItem.IS3)
            {
                superPTOnMemberItem.IS1 = true;
            }
            else
            {
                superPTOnMemberItem.IS1 = !superPTOnMemberItem.IS1;
            }

            if (superPTOnMemberItem.IS1)
            {
                superPTOnMemberItem.US1 = (byte)(superPTOnMemberItem.US1 == 15 ? 16 : 15);
                superPTOnMemberItem.S1 = 0;
            }
            else
            {
                superPTOnMemberItem.S1 = (byte)(superPTOnMemberItem.S1 == 15 ? 16 : 15);
                superPTOnMemberItem.US1 = 0;
            }

            memberSetting.SettingTypes = memberSetting.SettingTypes.Take(1);

            //// Act
            var updateParam = new UpdateSettingParam(memberSetting, querySettingParam.UplineLevel);
            var updateResult = _updateSettingService.UpdateCustomerSetting(updateParam);

            //// Assert
            Assert.IsTrue(updateResult.IsSuccessful);

            Thread.Sleep(TestConfig.NumOfSleepSecondsWaitingForReplication); //// Wait for replication data.

            var newMemberSetting = _querySettingService.GetCustomerSetting(querySettingParam);
            var newSuperOnMemberPTItem = newMemberSetting.SettingTypes.ElementAt(0).SettingItems.ElementAt(0) as SuperPTOnMemberItem;
            if (superPTOnMemberItem.IS1)
            {
                Assert.AreEqual(superPTOnMemberItem.US1, newSuperOnMemberPTItem.US1);
            }
            else
            {
                Assert.AreEqual(superPTOnMemberItem.S1, newSuperOnMemberPTItem.S1);
            }
        }

        protected void SuperUpdatesPTOnMultipleMembers(
            int superId = SettingInfo.SuperId,
            byte productId = Product255Constants.ProductId,
            IEnumerable<int> memberIds = null,
            IEnumerable<string> memberNames = null)
        {
            memberIds = memberIds ?? SettingInfo.MemberIds;
            memberNames = memberNames ?? SettingInfo.MemberNames;

            //// Arrange
            var actor = new User { Id = superId, Level = Level.Super, Name = "ActorName" };
            _settingContext = new SettingContext(actor);
            _querySettingService = new QuerySettingService(_settingContext);
            _updateSettingService = new UpdateSettingService(_settingContext);

            var querySettingParam = new QuerySettingParam(memberIds.First(), Level.Member, productId, Level.Super);

            var memberSetting = _querySettingService.GetCustomerSetting(querySettingParam);
            var superPTOnMemberItem = memberSetting.SettingTypes.ElementAt(0).SettingItems.ElementAt(0) as SuperPTOnMemberItem;

            if (superPTOnMemberItem.IS3)
            {
                superPTOnMemberItem.IS1 = true;
            }
            else
            {
                superPTOnMemberItem.IS1 = !superPTOnMemberItem.IS1;
            }

            if (superPTOnMemberItem.IS1)
            {
                superPTOnMemberItem.US1 = (byte)(superPTOnMemberItem.US1 == 15 ? 16 : 15);
                superPTOnMemberItem.S1 = 0;
            }
            else
            {
                superPTOnMemberItem.S1 = (byte)(superPTOnMemberItem.S1 == 15 ? 16 : 15);
                superPTOnMemberItem.US1 = 0;
            }

            memberSetting.SettingTypes = memberSetting.SettingTypes.Take(1);

            //// Act
            var updatingMultipleParam = new UpdateMultipleSettingParam(memberSetting, memberIds, memberNames, querySettingParam.UplineLevel);
            var updateResult = _updateSettingService.UpdateMultipleCustomerSetting(updatingMultipleParam);

            //// Assert
            Assert.IsTrue(updateResult.IsSuccessful);

            Thread.Sleep(TestConfig.NumOfSleepSecondsWaitingForReplication * 2); //// Wait for replication data.

            foreach (var memberId in memberIds)
            {
                querySettingParam.CustomerId = memberId;
                var newMemberSetting = _querySettingService.GetCustomerSetting(querySettingParam);
                var newSuperOnMemberPTItem = newMemberSetting.SettingTypes.ElementAt(0).SettingItems.ElementAt(0) as SuperPTOnMemberItem;
                if (superPTOnMemberItem.IS1)
                {
                    Assert.AreEqual(superPTOnMemberItem.US1, newSuperOnMemberPTItem.US1);
                }
                else
                {
                    Assert.AreEqual(superPTOnMemberItem.S1, newSuperOnMemberPTItem.S1);
                }
            }
        }
    }
}