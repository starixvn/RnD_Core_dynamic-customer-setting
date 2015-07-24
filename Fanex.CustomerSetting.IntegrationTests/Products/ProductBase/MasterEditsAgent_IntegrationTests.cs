namespace Fanex.CustomerSetting.IntegrationTests.Products.ProductBase
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.CustomerSetting.IntegrationTests.Models;
    using Fanex.CustomerSetting.IntegrationTests.Products.Product255;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Models.Enumerations;
    using Fanex.CustomerSetting.Models.Requests;
    using Fanex.CustomerSetting.Services;
    using Fanex.CustomerSetting.Services.Impl;
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
            IEnumerable<Action<ISetting>> assertGotSettingTypes,
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
            var querySettingParam = new CustomerSettingQuery(agentId, Level.Agent, productId, null);

            //// Act
            var agentSetting = _querySettingService.GetCustomerSetting(querySettingParam);

            //// Assert
            Assert.AreEqual(agentName, agentSetting.CustomerInfo.UserName);
            Assert.AreEqual(agentId, agentSetting.CustomerId);
            Assert.AreEqual(Level.Agent, agentSetting.CustomerLevel);
            Assert.AreEqual(productId, agentSetting.ProductId);
            ////Assert.AreEqual(productName, agentSetting.ProductName);
            Assert.AreEqual(numOfSettingTypes, agentSetting.SettingTypes.Count());

            foreach (Action<ISetting> assertGotSettingType in assertGotSettingTypes)
            {
                assertGotSettingType(agentSetting);
            }
        }

        protected void CompanyUpdatesAgentSetting(
            IEnumerable<Func<ISetting, ModifySettingTypeInfo>> modifySettingTypes,
            Dictionary<byte, Action<ISettingItem, ISetting>> assertUpdatedSettingTypes,
            int actorId = SettingInfo.CompanyId,
            byte productId = Product255Constants.ProductId,
            int agentId = SettingInfo.AgentId)
        {
            UpdatingAgentSetting(modifySettingTypes, assertUpdatedSettingTypes, actorId, Level.Company, agentId, productId);
        }

        protected void MasterUpdatesAgentSetting(
            IEnumerable<Func<ISetting, ModifySettingTypeInfo>> modifySettingTypes,
            Dictionary<byte, Action<ISettingItem, ISetting>> assertUpdatedSettingTypes,
            int actorId = SettingInfo.MasterId,
            byte productId = Product255Constants.ProductId,
            int agentId = SettingInfo.AgentId)
        {
            UpdatingAgentSetting(modifySettingTypes, assertUpdatedSettingTypes, actorId, Level.Master, agentId, productId);
        }

        private void UpdatingAgentSetting(
            IEnumerable<Func<ISetting, ModifySettingTypeInfo>> modifySettingTypes,
            Dictionary<byte, Action<ISettingItem, ISetting>> assertUpdatedSettingTypes,
            int actorId,
            Level actorLevel,
            int agentId,
            byte productId)
        {
            //// Arrange
            var actor = new User { Id = actorId, Level = actorLevel, Name = "ActorName" };
            _settingContext = new SettingContext(actor);
            _querySettingService = new QuerySettingService(_settingContext);
            _updateSettingService = new UpdateSettingService(_settingContext);
            var querySettingParam = new CustomerSettingQuery(agentId, Level.Agent, productId);

            var agentSetting = _querySettingService.GetCustomerSetting(querySettingParam);

            var expectedSettingTypeData = new Dictionary<byte, ISettingItem>();

            foreach (var modifySettingType in modifySettingTypes)
            {
                var modifiedSettingTypeInfo = modifySettingType(agentSetting);
                expectedSettingTypeData.Add(modifiedSettingTypeInfo.SettingTypeId, modifiedSettingTypeInfo.SettingItem);
            }

            //// Act
            var updateParam = new UpdateCustomerSettingCommand(agentSetting, querySettingParam.UplineLevel);
            var updateResult = _updateSettingService.Update(updateParam);

            //// Assert
            Assert.IsTrue(updateResult.IsSuccessful);

            var newAgentSetting = _querySettingService.GetCustomerSetting(querySettingParam);

            foreach (var assertUpdatedSettingType in assertUpdatedSettingTypes)
            {
                var expectedSettingItem = expectedSettingTypeData[assertUpdatedSettingType.Key];
                assertUpdatedSettingType.Value(expectedSettingItem, newAgentSetting);
            }
        }

        protected void MasterUpdatesMultipleAgentsSetting(
            IEnumerable<Func<ISetting, ModifySettingTypeInfo>> modifySettingTypes,
            Dictionary<byte, Action<ISettingItem, ISetting>> assertUpdatedSettingTypes,
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
            _querySettingService = new QuerySettingService(_settingContext);
            var querySettingParam = new CustomerSettingQuery(agentIds.First(), Level.Agent, productId, null);

            var agentSetting = _querySettingService.GetCustomerSetting(querySettingParam);

            var expectedSettingTypeData = new Dictionary<byte, ISettingItem>();

            foreach (var modifySettingType in modifySettingTypes)
            {
                var modifiedSettingTypeInfo = modifySettingType(agentSetting);
                expectedSettingTypeData.Add(modifiedSettingTypeInfo.SettingTypeId, modifiedSettingTypeInfo.SettingItem);
            }

            //// Act
            var updatingMultipleParam = new MultipleUpdateCustomerSettingCommand(agentSetting, agentIds, agentNames, querySettingParam.UplineLevel);
            var updateResult = _updateSettingService.Update(updatingMultipleParam);

            //// Assert
            Assert.IsTrue(updateResult.IsSuccessful);

            foreach (var agentId in agentIds)
            {
                querySettingParam.CustomerId = agentId;
                var newAgentSetting = _querySettingService.GetCustomerSetting(querySettingParam);

                foreach (var assertUpdatedSettingType in assertUpdatedSettingTypes)
                {
                    var expectedSettingItem = expectedSettingTypeData[assertUpdatedSettingType.Key];
                    assertUpdatedSettingType.Value(expectedSettingItem, newAgentSetting);
                }
            }
        }
    }
}