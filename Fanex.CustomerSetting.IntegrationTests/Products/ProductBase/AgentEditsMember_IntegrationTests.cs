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
    using Fanex.CustomerSetting.SettingType4.Template1.Models;
    using Fanex.Data.Configuration;
    using NUnit.Framework;

    public class AgentEditsMember_IntegrationTests
    {
        private IUpdateSettingService _updateSettingService;
        private IQuerySettingService _querySettingService;
        private ISettingContext _settingContext;

        [SetUp]
        public void Setup()
        {
            DbSettingProviderManager.Current.Start();
        }

        protected void AgentViewsMemberSetting(
            int actorId,
            Level actorLevel,
            int memberId = SettingInfo.MemberId,
            string memberUsername = SettingInfo.MemberUserName,
            byte productId = Product255Constants.ProductId,
            string productName = Product255Constants.ProductName,
            byte numOfSettingTypes = Product255Constants.NumOfSettingType)
        {
            //// Arrange
            var actor = new User { Id = actorId, Level = actorLevel, Name = "Actor Name" };
            _settingContext = new SettingContext(actor);
            _querySettingService = new QuerySettingService(_settingContext);
            _updateSettingService = new UpdateSettingService(_settingContext);
            var querySettingParam = new QuerySettingParam(memberId, Level.Member, productId, Level.Agent);

            //// Act
            var memberSetting = _querySettingService.GetCustomerSetting(querySettingParam);

            //// Assert
            Assert.AreEqual(memberUsername, memberSetting.CustomerInfo.UserName);
            Assert.AreEqual(memberId, memberSetting.CustomerId);
            Assert.AreEqual(Level.Member, memberSetting.CustomerLevel);
            Assert.AreEqual(productId, memberSetting.ProductId);
            Assert.AreEqual(productName, memberSetting.ProductName);
            Assert.AreEqual(numOfSettingTypes, memberSetting.SettingTypes.Count());

            var agentPTOnMemberItem = memberSetting.SettingTypes.ElementAt(0).SettingItems.ElementAt(0) as AgentPTOnMemberItem;
            Assert.IsNotNull(agentPTOnMemberItem);
            Assert.GreaterOrEqual(agentPTOnMemberItem.A1, 1);

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

        protected void AgentUpdatesMemberSetting(
            int actorId = SettingInfo.AgentId,
            byte productId = Product255Constants.ProductId,
            int memberId = SettingInfo.MemberId)
        {
            AgentUpdatesMemberSetting(actorId, Level.Agent, memberId, productId, true);
            AgentUpdatesMemberSetting(actorId, Level.Agent, memberId, productId, false);
        }

        protected void CompanyUpdatesMemberSettingByAgentView(
            int actorId = SettingInfo.CompanyId,
            byte productId = Product255Constants.ProductId,
            int memberId = SettingInfo.MemberId)
        {
            AgentUpdatesMemberSetting(actorId, Level.Company, memberId, productId, true);
            AgentUpdatesMemberSetting(actorId, Level.Company, memberId, productId, false);
        }

        private void AgentUpdatesMemberSetting(
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
            var querySettingParam = new QuerySettingParam(memberId, Level.Member, productId, Level.Agent);

            var memberSetting = _querySettingService.GetCustomerSetting(querySettingParam);
            var agentPTOnMemberItem = memberSetting.SettingTypes.ElementAt(0).SettingItems.ElementAt(0) as AgentPTOnMemberItem;
            agentPTOnMemberItem.A1 = (byte)(agentPTOnMemberItem.A2 == 14 ? 15 : 14);

            var memberBetLimitItem = memberSetting.SettingTypes.ElementAt(1).SettingItems.ElementAt(0) as BetLimitItem;
            memberBetLimitItem.MinBet = memberBetLimitItem.MinBet == 70 ? 71 : 70;
            memberBetLimitItem.MaxBet = memberBetLimitItem.MaxBet == 300 ? 301 : 300;
            memberBetLimitItem.MaxPerMatch = memberBetLimitItem.MaxPerMatch == 250550 ? 250551 : 250550;

            var memberCommissionItem = memberSetting.SettingTypes.ElementAt(2).SettingItems.ElementAt(0) as CommissionItem;
            memberCommissionItem.CommRate = memberCommissionItem.CommRate == 0.0033m ? 0.0032m : 0.0033m;

            var memberProductStatusItem = memberSetting.SettingTypes.ElementAt(3).SettingItems.ElementAt(0) as ProductStatusItem;
            memberProductStatusItem.IsDisabled = isDisabledProductStatus;

            //// Act
            var updateParam = new UpdateSettingParam(memberSetting, querySettingParam.UplineLevel);
            var updateResult = _updateSettingService.UpdateCustomerSetting(updateParam);

            //// Assert
            Assert.IsTrue(updateResult.IsSuccessful);

            var newMemberSetting = _querySettingService.GetCustomerSetting(querySettingParam);
            var newAgentOnMemberPTItem = newMemberSetting.SettingTypes.ElementAt(0).SettingItems.ElementAt(0) as AgentPTOnMemberItem;
            Assert.AreEqual(agentPTOnMemberItem.A1, newAgentOnMemberPTItem.A1);

            var newMemberBetLimitItem = newMemberSetting.SettingTypes.ElementAt(1).SettingItems.ElementAt(0) as BetLimitItem;
            Assert.AreEqual(memberBetLimitItem.MinBet, newMemberBetLimitItem.MinBet);
            Assert.AreEqual(memberBetLimitItem.MaxBet, newMemberBetLimitItem.MaxBet);
            Assert.AreEqual(memberBetLimitItem.MaxPerMatch, newMemberBetLimitItem.MaxPerMatch);

            var newMemberCommissionItem = newMemberSetting.SettingTypes.ElementAt(2).SettingItems.ElementAt(0) as CommissionItem;
            Assert.AreEqual(memberCommissionItem.CommRate, newMemberCommissionItem.CommRate);

            var newMemberProductStatusItem = newMemberSetting.SettingTypes.ElementAt(3).SettingItems.ElementAt(0) as ProductStatusItem;
            Assert.AreEqual(memberProductStatusItem.IsDisabled, newMemberProductStatusItem.IsDisabled);
        }

        protected void AgentUpdatesMultipleMembersSetting(
            int agentId = SettingInfo.AgentId,
            byte productId = Product255Constants.ProductId,
            IEnumerable<int> memberIds = null,
            IEnumerable<string> memberNames = null)
        {
            memberIds = memberIds ?? SettingInfo.MemberIds;
            memberNames = memberNames ?? SettingInfo.MemberNames;

            //// Arrange
            var actor = new User { Id = agentId, Level = Level.Agent, Name = "ActorName" };
            _settingContext = new SettingContext(actor);
            _querySettingService = new QuerySettingService(_settingContext);
            _updateSettingService = new UpdateSettingService(_settingContext);

            var querySettingParam = new QuerySettingParam(memberIds.First(), Level.Member, productId, Level.Agent);

            var memberSetting = _querySettingService.GetCustomerSetting(querySettingParam);
            var agentPTOnMemberItem = memberSetting.SettingTypes.ElementAt(0).SettingItems.ElementAt(0) as AgentPTOnMemberItem;
            agentPTOnMemberItem.A1 = (byte)(agentPTOnMemberItem.A2 == 14 ? 15 : 14);

            var memberBetLimitItem = memberSetting.SettingTypes.ElementAt(1).SettingItems.ElementAt(0) as BetLimitItem;
            memberBetLimitItem.MinBet = memberBetLimitItem.MinBet == 70 ? 71 : 70;
            memberBetLimitItem.MaxBet = memberBetLimitItem.MaxBet == 300 ? 301 : 300;
            memberBetLimitItem.MaxPerMatch = memberBetLimitItem.MaxPerMatch == 250550 ? 250551 : 250550;

            var memberCommissionItem = memberSetting.SettingTypes.ElementAt(2).SettingItems.ElementAt(0) as CommissionItem;
            memberCommissionItem.CommRate = memberCommissionItem.CommRate == 0.0033m ? 0.0032m : 0.0033m;

            var memberProductStatusItem = memberSetting.SettingTypes.ElementAt(3).SettingItems.ElementAt(0) as ProductStatusItem;
            memberProductStatusItem.IsDisabled = false;

            //// Act
            var updatingMultipleParam = new UpdateMultipleSettingParam(memberSetting, memberIds, memberNames, querySettingParam.UplineLevel);
            var updateResult = _updateSettingService.UpdateCustomerSetting(updatingMultipleParam);

            //// Assert
            Assert.IsTrue(updateResult.IsSuccessful);

            foreach (var memberId in memberIds)
            {
                querySettingParam.CustomerId = memberId;
                var newMemberSetting = _querySettingService.GetCustomerSetting(querySettingParam);
                var newAgentOnMemberPTItem = newMemberSetting.SettingTypes.ElementAt(0).SettingItems.ElementAt(0) as AgentPTOnMemberItem;
                Assert.AreEqual(agentPTOnMemberItem.A1, newAgentOnMemberPTItem.A1);

                var newMemberBetLimitItem = newMemberSetting.SettingTypes.ElementAt(1).SettingItems.ElementAt(0) as BetLimitItem;
                Assert.AreEqual(memberBetLimitItem.MinBet, newMemberBetLimitItem.MinBet);
                Assert.AreEqual(memberBetLimitItem.MaxBet, newMemberBetLimitItem.MaxBet);
                Assert.AreEqual(memberBetLimitItem.MaxPerMatch, newMemberBetLimitItem.MaxPerMatch);

                var newMemberCommissionItem = newMemberSetting.SettingTypes.ElementAt(2).SettingItems.ElementAt(0) as CommissionItem;
                Assert.AreEqual(memberCommissionItem.CommRate, newMemberCommissionItem.CommRate);

                var newMemberProductStatusItem = newMemberSetting.SettingTypes.ElementAt(3).SettingItems.ElementAt(0) as ProductStatusItem;
                Assert.AreEqual(memberProductStatusItem.IsDisabled, newMemberProductStatusItem.IsDisabled);
            }
        }
    }
}