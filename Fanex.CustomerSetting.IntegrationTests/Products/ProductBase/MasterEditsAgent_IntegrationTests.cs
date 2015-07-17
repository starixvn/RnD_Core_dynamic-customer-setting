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

    public class MasterEditsAgent_IntegrationTests
    {
        private IUpdateSettingService _updateSettingService;
        private IQuerySettingService _querySettingService;
        private ISettingContext _settingContext;

        [SetUp]
        public void Setup()
        {
            DbSettingProviderManager.Current.Start();
        }

        protected void ViewingAgentSetting(
            int actorId,
            Level actorLevel,
            int agentId = SettingInfo.AgentId,
            string agentName = SettingInfo.AgentUserName,
            byte productId = Product255Constants.ProductId,
            string productName = Product255Constants.ProductName,
            byte numOfSettingTypes = Product255Constants.NumOfSettingType)
        {
            //// Arrange
            var actor = new User { Id = actorId, Level = actorLevel, Name = "Actor Name" };
            _settingContext = new SettingContext(actor);
            _querySettingService = new QuerySettingService(_settingContext);
            _updateSettingService = new UpdateSettingService(_settingContext);
            var querySettingParam = new QuerySettingParam(agentId, Level.Agent, productId, null);

            //// Act
            var agentSetting = _querySettingService.GetCustomerSetting(querySettingParam);

            //// Assert
            Assert.AreEqual(agentName, agentSetting.CustomerInfo.UserName);
            Assert.AreEqual(agentId, agentSetting.CustomerId);
            Assert.AreEqual(Level.Agent, agentSetting.CustomerLevel);
            Assert.AreEqual(productId, agentSetting.ProductId);
            Assert.AreEqual(productName, agentSetting.ProductName);
            Assert.AreEqual(numOfSettingTypes, agentSetting.SettingTypes.Count());

            var agentPT = agentSetting.SettingTypes.ElementAt(0) as PTSettingType;
            Assert.IsNotNull(agentPT);

            var agentPTItem = agentSetting.SettingTypes.ElementAt(0).SettingItems.ElementAt(0) as AgentPTItem;
            Assert.IsNotNull(agentPTItem);
            Assert.GreaterOrEqual(agentPTItem.IM2 ? agentPTItem.UM2 : agentPTItem.M2, 1);
            Assert.GreaterOrEqual(agentPTItem.A2, 1);
            Assert.GreaterOrEqual(agentPTItem.N2, 0);

            var agentBetLimitItem = agentSetting.SettingTypes.ElementAt(1).SettingItems.ElementAt(0) as BetLimitItem;
            Assert.IsNotNull(agentBetLimitItem);
            Assert.GreaterOrEqual(agentBetLimitItem.MinBet, 0);
            Assert.GreaterOrEqual(agentBetLimitItem.MaxBet, 0);
            Assert.GreaterOrEqual(agentBetLimitItem.MaxPerMatch, 0);

            var masterCommissionItem = agentSetting.SettingTypes.ElementAt(2).SettingItems.ElementAt(0) as CommissionItem;
            Assert.IsNotNull(masterCommissionItem);
            Assert.GreaterOrEqual(masterCommissionItem.CommRate, 0);
            Assert.LessOrEqual(masterCommissionItem.CommRate, 0.01);
            Assert.GreaterOrEqual(masterCommissionItem.Upline.CommRate, 0);
            Assert.LessOrEqual(masterCommissionItem.Upline.CommRate, 0.01);

            var agentProductStatusItem = agentSetting.SettingTypes.ElementAt(3).SettingItems.ElementAt(0) as ProductStatusItem;
            Assert.IsNotNull(agentProductStatusItem);
            Assert.IsNotNull(agentProductStatusItem.Downline);
        }

        protected void CompanyUpdatesAgentSetting(
            int actorId = SettingInfo.CompanyId,
            byte productId = Product255Constants.ProductId,
            int agentId = SettingInfo.AgentId)
        {
            UpdatingAgentSetting(actorId, Level.Company, agentId, productId, true);
            UpdatingAgentSetting(actorId, Level.Company, agentId, productId, false);
        }

        protected void MasterUpdatesAgentSetting(
            int actorId = SettingInfo.MasterId,
            byte productId = Product255Constants.ProductId,
            int agentId = SettingInfo.AgentId)
        {
            UpdatingAgentSetting(actorId, Level.Master, agentId, productId, true);
            UpdatingAgentSetting(actorId, Level.Master, agentId, productId, false);
        }

        private void UpdatingAgentSetting(
            int actorId,
            Level actorLevel,
            int agentId,
            byte productId,
            bool isDisabledProductStatus)
        {
            //// Arrange
            var actor = new User { Id = actorId, Level = actorLevel, Name = "ActorName" };
            _settingContext = new SettingContext(actor);
            _querySettingService = new QuerySettingService(_settingContext);
            _updateSettingService = new UpdateSettingService(_settingContext);
            var querySettingParam = new QuerySettingParam(agentId, Level.Agent, productId);

            var agentSetting = _querySettingService.GetCustomerSetting(querySettingParam);
            var agentPT = agentSetting.SettingTypes.ElementAt(0) as PTSettingType;
            var agentPTItem = agentSetting.SettingTypes.ElementAt(0).SettingItems.ElementAt(0) as AgentPTItem;

            if (!agentPT.IsDisableMasterAutoPT)
            {
                agentPTItem.IM2 = !agentPTItem.IM2;
            }

            if (agentPTItem.IM2)
            {
                agentPTItem.UM2 = (byte)(agentPTItem.UM2 == 20 ? 21 : 20);
                agentPTItem.M2 = 0;
            }
            else
            {
                agentPTItem.M2 = (byte)(agentPTItem.M2 == 20 ? 21 : 20);
                agentPTItem.UM2 = 0;
            }

            agentPTItem.A2 = (byte)(agentPTItem.A2 == 35 ? 36 : 35);
            agentPTItem.N2 = (byte)(agentPTItem.N2 == 5 ? 6 : 5);

            var agentBetLimitItem = agentSetting.SettingTypes.ElementAt(1).SettingItems.ElementAt(0) as BetLimitItem;
            agentBetLimitItem.MinBet = agentBetLimitItem.MinBet == 60 ? 61 : 60;
            agentBetLimitItem.MaxBet = agentBetLimitItem.MaxBet == 400 ? 401 : 400;
            agentBetLimitItem.MaxPerMatch = agentBetLimitItem.MaxPerMatch == 350550 ? 350551 : 350550;

            var agentCommissionItem = agentSetting.SettingTypes.ElementAt(2).SettingItems.ElementAt(0) as CommissionItem;
            agentCommissionItem.CommRate = agentCommissionItem.CommRate == 0.0055m ? 0.0056m : 0.0055m;

            var agentProductStatusItem = agentSetting.SettingTypes.ElementAt(3).SettingItems.ElementAt(0) as ProductStatusItem;
            agentProductStatusItem.IsDisabled = isDisabledProductStatus;
            agentProductStatusItem.Downline.IsDisabled = isDisabledProductStatus;

            //// Act
            var updateParam = new UpdatingSettingParam(agentSetting, querySettingParam.UplineLevel);
            var updateResult = _updateSettingService.UpdateCustomerSetting(updateParam);

            //// Assert
            Assert.IsTrue(updateResult.IsSuccessful);

            Thread.Sleep(TestConfig.NumOfSleepSecondsWaitingForReplication); //// Wait for replication data.

            var newAgentSetting = _querySettingService.GetCustomerSetting(querySettingParam);
            var newAgentPTItem = newAgentSetting.SettingTypes.ElementAt(0).SettingItems.ElementAt(0) as AgentPTItem;
            Assert.AreEqual(agentPTItem.IM2, newAgentPTItem.IM2);
            Assert.AreEqual(agentPTItem.UM2, newAgentPTItem.UM2);
            Assert.AreEqual(agentPTItem.M2, newAgentPTItem.M2);
            Assert.AreEqual(agentPTItem.A2, newAgentPTItem.A2);
            Assert.AreEqual(agentPTItem.N2, newAgentPTItem.N2);

            var newAgentBetLimitItem = newAgentSetting.SettingTypes.ElementAt(1).SettingItems.ElementAt(0) as BetLimitItem;
            Assert.AreEqual(agentBetLimitItem.MinBet, newAgentBetLimitItem.MinBet);
            Assert.AreEqual(agentBetLimitItem.MaxBet, newAgentBetLimitItem.MaxBet);
            Assert.AreEqual(agentBetLimitItem.MaxPerMatch, newAgentBetLimitItem.MaxPerMatch);

            var newAgentCommissionItem = newAgentSetting.SettingTypes.ElementAt(2).SettingItems.ElementAt(0) as CommissionItem;
            Assert.AreEqual(agentCommissionItem.CommRate, newAgentCommissionItem.CommRate);

            var newAgentProductStatusItem = newAgentSetting.SettingTypes.ElementAt(3).SettingItems.ElementAt(0) as ProductStatusItem;
            Assert.AreEqual(agentProductStatusItem.IsDisabled, newAgentProductStatusItem.IsDisabled);
            Assert.AreEqual(agentProductStatusItem.Downline.IsDisabled, newAgentProductStatusItem.Downline.IsDisabled);
        }

        protected void MasterUpdatesMultipleAgentsSetting(
            int masterId = SettingInfo.MasterId,
            byte productId = Product255Constants.ProductId,
            IEnumerable<int> agentIds = null,
            IEnumerable<string> agentNames = null)
        {
            agentIds = agentIds ?? SettingInfo.AgentIds;
            agentNames = agentNames ?? SettingInfo.AgentNames;

            //// Arrange
            var actor = new User { Id = masterId, Level = Level.Master, Name = "ActorName" };
            _settingContext = new SettingContext(actor);
            _updateSettingService = new UpdateSettingService(_settingContext);
            var querySettingParam = new QuerySettingParam(agentIds.First(), Level.Agent, productId, null);

            var agentSetting = _querySettingService.GetCustomerSetting(querySettingParam);
            var agentPT = agentSetting.SettingTypes.ElementAt(0) as PTSettingType;
            var agentPTItem = agentSetting.SettingTypes.ElementAt(0).SettingItems.ElementAt(0) as AgentPTItem;
            
            if (!agentPT.IsDisableMasterAutoPT)
            {
                agentPTItem.IM2 = !agentPTItem.IM2;
            }

            if (agentPTItem.IM2)
            {
                agentPTItem.UM2 = (byte)(agentPTItem.UM2 == 20 ? 21 : 20);
                agentPTItem.M2 = 0;
            }
            else
            {
                agentPTItem.M2 = (byte)(agentPTItem.M2 == 20 ? 21 : 20);
                agentPTItem.UM2 = 0;
            }

            agentPTItem.A2 = (byte)(agentPTItem.A2 == 35 ? 36 : 35);
            agentPTItem.N2 = (byte)(agentPTItem.N2 == 5 ? 6 : 5);

            var agentBetLimitItem = agentSetting.SettingTypes.ElementAt(1).SettingItems.ElementAt(0) as BetLimitItem;
            agentBetLimitItem.MinBet = agentBetLimitItem.MinBet == 60 ? 61 : 60;
            agentBetLimitItem.MaxBet = agentBetLimitItem.MaxBet == 400 ? 401 : 400;
            agentBetLimitItem.MaxPerMatch = agentBetLimitItem.MaxPerMatch == 350550 ? 350551 : 350550;

            var agentCommissionItem = agentSetting.SettingTypes.ElementAt(2).SettingItems.ElementAt(0) as CommissionItem;
            agentCommissionItem.CommRate = agentCommissionItem.CommRate == 0.0055m ? 0.0056m : 0.0055m;

            var agentProductStatusItem = agentSetting.SettingTypes.ElementAt(3).SettingItems.ElementAt(0) as ProductStatusItem;
            agentProductStatusItem.IsDisabled = false;
            agentProductStatusItem.Downline.IsDisabled = false;

            //// Act
            var updatingMultipleParam = new UpdatingMultipleSettingParam(agentSetting, agentIds, agentNames, querySettingParam.UplineLevel);
            var updateResult = _updateSettingService.UpdateMultipleCustomerSetting(updatingMultipleParam);

            //// Assert
            Assert.IsTrue(updateResult.IsSuccessful);

            Thread.Sleep(TestConfig.NumOfSleepSecondsWaitingForReplication * 2); //// Wait for replication data.

            foreach (var agentId in agentIds)
            {
                querySettingParam.CustomerId = agentId;
                var newAgentSetting = _querySettingService.GetCustomerSetting(querySettingParam);
                var newAgentPTItem = newAgentSetting.SettingTypes.ElementAt(0).SettingItems.ElementAt(0) as AgentPTItem;
                Assert.AreEqual(agentPTItem.IM2, newAgentPTItem.IM2);
                Assert.AreEqual(agentPTItem.UM2, newAgentPTItem.UM2);
                Assert.AreEqual(agentPTItem.M2, newAgentPTItem.M2);
                Assert.AreEqual(agentPTItem.A2, newAgentPTItem.A2);
                Assert.AreEqual(agentPTItem.N2, newAgentPTItem.N2);

                var newAgentBetLimitItem = newAgentSetting.SettingTypes.ElementAt(1).SettingItems.ElementAt(0) as BetLimitItem;
                Assert.AreEqual(agentBetLimitItem.MinBet, newAgentBetLimitItem.MinBet);
                Assert.AreEqual(agentBetLimitItem.MaxBet, newAgentBetLimitItem.MaxBet);
                Assert.AreEqual(agentBetLimitItem.MaxPerMatch, newAgentBetLimitItem.MaxPerMatch);

                var newAgentCommissionItem = newAgentSetting.SettingTypes.ElementAt(2).SettingItems.ElementAt(0) as CommissionItem;
                Assert.AreEqual(agentCommissionItem.CommRate, newAgentCommissionItem.CommRate);

                var newAgentProductStatusItem = newAgentSetting.SettingTypes.ElementAt(3).SettingItems.ElementAt(0) as ProductStatusItem;
                Assert.AreEqual(agentProductStatusItem.IsDisabled, newAgentProductStatusItem.IsDisabled);
                Assert.AreEqual(agentProductStatusItem.Downline.IsDisabled, newAgentProductStatusItem.Downline.IsDisabled);
            }
        }
    }
}