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

    public class SuperEditsMaster_IntegrationTests
    {
        private IUpdateSettingService _updateSettingService;
        private IQuerySettingService _querySettingService;
        private ISettingContext _settingContext;

        [SetUp]
        public void Setup()
        {
            DbSettingProviderManager.Current.Start();
        }

        protected void ViewingMasterSetting(
            IEnumerable<Action<ISetting>> assertGotSettingTypes,
            int actorId, 
            Level actorLevel,
            int masterId = SettingInfo.MasterId,
            string masterUsername = SettingInfo.MasterUserName,
            byte productId = Product255Constants.ProductId,
            byte numOfSettingTypes = Product255Constants.NumOfSettingType)
        {
            //// Arrange
            var actor = new User { Id = actorId, Level = actorLevel, Name = "Actor Name" };
            _settingContext = new SettingContext(actor);
            _querySettingService = new QuerySettingService(_settingContext);
            _updateSettingService = new UpdateSettingService(_settingContext);
            var querySettingParam = new CustomerSettingQuery(masterId, Level.Master, productId);

            //// Act
            var masterSetting = _querySettingService.GetCustomerSetting(querySettingParam);

            //// Assert
            Assert.AreEqual(masterUsername, masterSetting.CustomerInfo.UserName);
            Assert.AreEqual(masterId, masterSetting.CustomerId);
            Assert.AreEqual(Level.Master, masterSetting.CustomerLevel);
            Assert.AreEqual(productId, masterSetting.ProductId);
            Assert.IsNotNullOrEmpty(masterSetting.ProductName);
            Assert.AreEqual(numOfSettingTypes, masterSetting.SettingTypes.Count());

            foreach (Action<ISetting> assertGotSettingType in assertGotSettingTypes)
            {
                assertGotSettingType(masterSetting);
            }
        }

        protected void CompanyUpdatesMasterSetting(
            IEnumerable<Func<ISetting, ModifySettingTypeInfo>> modifySettingTypes,
            Dictionary<byte, Action<ISettingItem, ISetting>> assertUpdatedSettingTypes,
            int actorId = SettingInfo.CompanyId,
            byte productId = Product255Constants.ProductId,
            int masterId = SettingInfo.MasterId)
        {
            UpdatingMasterSetting(modifySettingTypes, assertUpdatedSettingTypes, actorId, Level.Company, masterId, productId);

            //// UpdatingMasterSetting(modifySettingTypes, assertUpdatedSettingTypes, actorId, Level.Company, masterId, productId);
        }

        protected void SuperUpdatesMasterSetting(
            IEnumerable<Func<ISetting, ModifySettingTypeInfo>> modifySettingTypes,
            Dictionary<byte, Action<ISettingItem, ISetting>> assertUpdatedSettingTypes,
            int actorId = SettingInfo.SuperId,
            byte productId = Product255Constants.ProductId,
            int masterId = SettingInfo.MasterId)
        {
            UpdatingMasterSetting(modifySettingTypes, assertUpdatedSettingTypes, actorId, Level.Super, masterId, productId);

            //// UpdatingMasterSetting(modifySettingTypes, assertUpdatedSettingTypes, actorId, Level.Super, masterId, productId);
        }

        private void UpdatingMasterSetting(
            IEnumerable<Func<ISetting, ModifySettingTypeInfo>> modifySettingTypes,
            Dictionary<byte, Action<ISettingItem, ISetting>> assertUpdatedSettingTypes,
            int actorId, 
            Level actorLevel,
            int masterId,            
            byte productId)
        {
            //// Arrange
            var actor = new User { Id = actorId, Level = actorLevel, Name = "ActorName" };
            _settingContext = new SettingContext(actor);
            _querySettingService = new QuerySettingService(_settingContext);
            _updateSettingService = new UpdateSettingService(_settingContext);
            var querySettingParam = new CustomerSettingQuery(masterId, Level.Master, productId);

            var masterSetting = _querySettingService.GetCustomerSetting(querySettingParam);
            var expectedSettingTypeData = new Dictionary<byte, ISettingItem>();

            foreach (var modifySettingType in modifySettingTypes)
            {
                var modifiedSettingTypeInfo = modifySettingType(masterSetting);
                expectedSettingTypeData.Add(modifiedSettingTypeInfo.SettingTypeId, modifiedSettingTypeInfo.SettingItem);
            }

            //// Act
            var updateParam = new UpdateCustomerSettingCommand(masterSetting, querySettingParam.UplineLevel);
            var updateResult = _updateSettingService.Update(updateParam);

            //// Assert
            Assert.IsTrue(updateResult.IsSuccessful);

            var newMasterSetting = _querySettingService.GetCustomerSetting(querySettingParam);

            foreach (var assertUpdatedSettingType in assertUpdatedSettingTypes)
            {
                var expectedSettingItem = expectedSettingTypeData[assertUpdatedSettingType.Key];
                assertUpdatedSettingType.Value(expectedSettingItem, newMasterSetting);
            }
        }

        protected void SuperUpdatesMultipleMastersSetting(
            IEnumerable<Func<ISetting, ModifySettingTypeInfo>> modifySettingTypes,
            Dictionary<byte, Action<ISettingItem, ISetting>> assertUpdatedSettingTypes,
            int superId = SettingInfo.SuperId,
            byte productId = Product255Constants.ProductId,
            IEnumerable<int> masterIds = null,
            IEnumerable<string> masterNames = null)
        {
            //// Arrange
            masterIds = masterIds ?? SettingInfo.MasterIds;
            masterNames = masterNames ?? SettingInfo.MasterNames;

            var actor = new User { Id = superId, Level = Level.Super, Name = "ActorName" };
            _settingContext = new SettingContext(actor);
            _querySettingService = new QuerySettingService(_settingContext);
            _updateSettingService = new UpdateSettingService(_settingContext);
            var querySettingParam = new CustomerSettingQuery(masterIds.First(), Level.Master, productId);

            var masterSetting = _querySettingService.GetCustomerSetting(querySettingParam);

            var expectedSettingTypeData = new Dictionary<byte, ISettingItem>();

            foreach (var modifySettingType in modifySettingTypes)
            {
                var modifiedSettingTypeInfo = modifySettingType(masterSetting);
                expectedSettingTypeData.Add(modifiedSettingTypeInfo.SettingTypeId, modifiedSettingTypeInfo.SettingItem);
            }

            //// Act
            var updatingMultipleParam = new MultipleUpdateCustomerSettingCommand(masterSetting, masterIds, masterNames, querySettingParam.UplineLevel);
            var updateResult = _updateSettingService.Update(updatingMultipleParam);

            //// Assert
            Assert.IsTrue(updateResult.IsSuccessful);

            foreach (var masterId in masterIds)
            {
                querySettingParam.CustomerId = masterId;
                var newMasterSetting = _querySettingService.GetCustomerSetting(querySettingParam);

                foreach (var assertUpdatedSettingType in assertUpdatedSettingTypes)
                {
                    var expectedSettingItem = expectedSettingTypeData[assertUpdatedSettingType.Key];
                    assertUpdatedSettingType.Value(expectedSettingItem, newMasterSetting);
                }
            }
        }
    }
}