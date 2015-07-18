namespace Fanex.CustomerSetting.IntegrationTests.Products.ProductBase
{
    using System;
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
            int actorId, 
            Level actorLevel,
            int masterId = SettingInfo.MasterId,
            string masterUsername = SettingInfo.MasterUserName,
            byte productId = Product255Constants.ProductId,
            string productName = Product255Constants.ProductName,
            byte numOfSettingTypes = Product255Constants.NumOfSettingType)
        {
            //// Arrange
            var actor = new User { Id = actorId, Level = actorLevel, Name = "Actor Name" };
            _settingContext = new SettingContext(actor);
            _querySettingService = new QuerySettingService(_settingContext);
            _updateSettingService = new UpdateSettingService(_settingContext);
            var querySettingParam = new QuerySettingParam(masterId, Level.Master, productId);

            //// Act
            var masterSetting = _querySettingService.GetCustomerSetting(querySettingParam);

            //// Assert
            Assert.AreEqual(masterUsername, masterSetting.CustomerInfo.UserName);
            Assert.AreEqual(masterId, masterSetting.CustomerId);
            Assert.AreEqual(Level.Master, masterSetting.CustomerLevel);
            Assert.AreEqual(productId, masterSetting.ProductId);
            Assert.AreEqual(productName, masterSetting.ProductName);
            Assert.AreEqual(numOfSettingTypes, masterSetting.SettingTypes.Count());

            var masterPTItem = masterSetting.SettingTypes.ElementAt(0).SettingItems.ElementAt(0) as MasterPTItem;
            Assert.IsNotNull(masterPTItem);
            Assert.GreaterOrEqual(masterPTItem.IS3 ? masterPTItem.US3 : masterPTItem.M3, 1);
            Assert.GreaterOrEqual(masterPTItem.S3, 1);
            Assert.GreaterOrEqual(masterPTItem.N3, 0);

            var masterBetLimitItem = masterSetting.SettingTypes.ElementAt(1).SettingItems.ElementAt(0) as BetLimitItem;
            Assert.IsNotNull(masterBetLimitItem);
            Assert.GreaterOrEqual(masterBetLimitItem.MinBet, 0);
            Assert.GreaterOrEqual(masterBetLimitItem.MaxBet, 0);
            Assert.GreaterOrEqual(masterBetLimitItem.MaxPerMatch, 0);

            var masterCommissionItem = masterSetting.SettingTypes.ElementAt(2).SettingItems.ElementAt(0) as CommissionItem;
            Assert.IsNotNull(masterCommissionItem);
            Assert.GreaterOrEqual(masterCommissionItem.CommRate, 0);
            Assert.LessOrEqual(masterCommissionItem.CommRate, 0.01);
            Assert.GreaterOrEqual(masterCommissionItem.Upline.CommRate, 0);
            Assert.LessOrEqual(masterCommissionItem.Upline.CommRate, 0.01);

            var masterProductStatusItem = masterSetting.SettingTypes.ElementAt(3).SettingItems.ElementAt(0) as ProductStatusItem;
            Assert.IsNotNull(masterProductStatusItem);
            Assert.IsNotNull(masterProductStatusItem.Downline);
        }

        protected void CompanyUpdatesMasterSetting(
            int actorId = SettingInfo.CompanyId,
            byte productId = Product255Constants.ProductId,
            int masterId = SettingInfo.MasterId)
        {
            UpdatingMasterSetting(actorId, Level.Company, masterId, productId, true);
            UpdatingMasterSetting(actorId, Level.Company, masterId, productId, false);
        }

        protected void SuperUpdatesMasterSetting(
            int actorId = SettingInfo.SuperId,
            byte productId = Product255Constants.ProductId,
            int masterId = SettingInfo.MasterId)
        {
            UpdatingMasterSetting(actorId, Level.Super, masterId, productId, true);
            UpdatingMasterSetting(actorId, Level.Super, masterId, productId, false);
        }

        private void UpdatingMasterSetting(
            int actorId, 
            Level actorLevel,
            int masterId,            
            byte productId,
            bool isDisbaledProductStatus)
        {
            //// Arrange
            var actor = new User { Id = actorId, Level = actorLevel, Name = "ActorName" };
            _settingContext = new SettingContext(actor);
            _querySettingService = new QuerySettingService(_settingContext);
            _updateSettingService = new UpdateSettingService(_settingContext);
            var querySettingParam = new QuerySettingParam(masterId, Level.Master, productId);

            var masterSetting = _querySettingService.GetCustomerSetting(querySettingParam);
            var masterPTItem = masterSetting.SettingTypes.ElementAt(0).SettingItems.ElementAt(0) as MasterPTItem;
            masterPTItem.IS3 = !masterPTItem.IS3;
            if (masterPTItem.IS3)
            {
                masterPTItem.US3 = (byte)(masterPTItem.US3 == 20 ? 21 : 20);
                masterPTItem.S3 = 0;
            }
            else
            {
                masterPTItem.S3 = (byte)(masterPTItem.S3 == 20 ? 21 : 20);
                masterPTItem.US3 = 0;
            }

            masterPTItem.M3 = (byte)(masterPTItem.M3 == 65 ? 66 : 65);
            masterPTItem.N3 = (byte)(masterPTItem.N3 == 5 ? 6 : 5);

            var masterBetLimitItem = masterSetting.SettingTypes.ElementAt(1).SettingItems.ElementAt(0) as BetLimitItem;
            masterBetLimitItem.MinBet = masterBetLimitItem.MinBet == 50 ? 51 : 50;
            masterBetLimitItem.MaxBet = masterBetLimitItem.MaxBet == 500 ? 501 : 500;
            masterBetLimitItem.MaxPerMatch = masterBetLimitItem.MaxPerMatch == 450550 ? 450551 : 450550;

            var masterCommissionItem = masterSetting.SettingTypes.ElementAt(2).SettingItems.ElementAt(0) as CommissionItem;
            masterCommissionItem.CommRate = masterCommissionItem.CommRate == 0.0080m ? 0.0081m : 0.0080m;

            var masterProductStatusItem = masterSetting.SettingTypes.ElementAt(3).SettingItems.ElementAt(0) as ProductStatusItem;
            masterProductStatusItem.IsDisabled = isDisbaledProductStatus;
            masterProductStatusItem.Downline.IsDisabled = isDisbaledProductStatus;

            //// Act
            var updateParam = new UpdateSettingParam(masterSetting, querySettingParam.UplineLevel);
            var updateResult = _updateSettingService.UpdateCustomerSetting(updateParam);

            //// Assert
            Assert.IsTrue(updateResult.IsSuccessful);

            Thread.Sleep(TestConfig.NumOfSleepSecondsWaitingForReplication); //// Wait for replication data.

            var newMasterSetting = _querySettingService.GetCustomerSetting(querySettingParam);
            var newMasterPTItem = newMasterSetting.SettingTypes.ElementAt(0).SettingItems.ElementAt(0) as MasterPTItem;
            Assert.AreEqual(masterPTItem.IS3, newMasterPTItem.IS3);
            Assert.AreEqual(masterPTItem.US3, newMasterPTItem.US3);
            Assert.AreEqual(masterPTItem.S3, newMasterPTItem.S3);
            Assert.AreEqual(masterPTItem.M3, newMasterPTItem.M3);
            Assert.AreEqual(masterPTItem.N3, newMasterPTItem.N3);

            var newMasterBetLimitItem = newMasterSetting.SettingTypes.ElementAt(1).SettingItems.ElementAt(0) as BetLimitItem;
            Assert.AreEqual(masterBetLimitItem.MinBet, newMasterBetLimitItem.MinBet);
            Assert.AreEqual(masterBetLimitItem.MaxBet, newMasterBetLimitItem.MaxBet);
            Assert.AreEqual(masterBetLimitItem.MaxPerMatch, newMasterBetLimitItem.MaxPerMatch);

            var newMasterCommissionItem = newMasterSetting.SettingTypes.ElementAt(2).SettingItems.ElementAt(0) as CommissionItem;
            Assert.AreEqual(masterCommissionItem.CommRate, newMasterCommissionItem.CommRate);

            var newMasterProductStatusItem = newMasterSetting.SettingTypes.ElementAt(3).SettingItems.ElementAt(0) as ProductStatusItem;
            Assert.AreEqual(masterProductStatusItem.IsDisabled, newMasterProductStatusItem.IsDisabled);
            Assert.AreEqual(masterProductStatusItem.Downline.IsDisabled, newMasterProductStatusItem.Downline.IsDisabled);
        }

        protected void SuperUpdatesMultipleMastersSetting(
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
            _updateSettingService = new UpdateSettingService(_settingContext);
            var querySettingParam = new QuerySettingParam(masterIds.First(), Level.Master, productId);

            var masterSetting = _querySettingService.GetCustomerSetting(querySettingParam);
            var masterPTItem = masterSetting.SettingTypes.ElementAt(0).SettingItems.ElementAt(0) as MasterPTItem;
            if (masterPTItem.IS3)
            {
                masterPTItem.US3 = (byte)(masterPTItem.US3 == 20 ? 21 : 20);
            }
            else
            {
                masterPTItem.S3 = (byte)(masterPTItem.S3 == 20 ? 21 : 20);
            }

            masterPTItem.M3 = (byte)(masterPTItem.M3 == 65 ? 66 : 65);
            masterPTItem.N3 = (byte)(masterPTItem.N3 == 5 ? 6 : 5);

            var masterBetLimitItem = masterSetting.SettingTypes.ElementAt(1).SettingItems.ElementAt(0) as BetLimitItem;
            masterBetLimitItem.MinBet = masterBetLimitItem.MinBet == 50 ? 51 : 50;
            masterBetLimitItem.MaxBet = masterBetLimitItem.MaxBet == 500 ? 501 : 500;
            masterBetLimitItem.MaxPerMatch = masterBetLimitItem.MaxPerMatch == 450550 ? 450551 : 450550;

            var masterCommissionItem = masterSetting.SettingTypes.ElementAt(2).SettingItems.ElementAt(0) as CommissionItem;
            masterCommissionItem.CommRate = masterCommissionItem.CommRate == 0.0080m ? 0.0081m : 0.0080m;

            var masterProductStatusItem = masterSetting.SettingTypes.ElementAt(3).SettingItems.ElementAt(0) as ProductStatusItem;
            masterProductStatusItem.IsDisabled = false;
            masterProductStatusItem.Downline.IsDisabled = false;

            //// Act
            var updatingMultipleParam = new UpdateMultipleSettingParam(masterSetting, masterIds, masterNames, querySettingParam.UplineLevel);
            var updateResult = _updateSettingService.UpdateMultipleCustomerSetting(updatingMultipleParam);

            //// Assert
            Assert.IsTrue(updateResult.IsSuccessful);

            Thread.Sleep(TestConfig.NumOfSleepSecondsWaitingForReplication * 2); //// Wait for replication data.

            foreach (var masterId in masterIds)
            {
                querySettingParam.CustomerId = masterId;
                var newMasterSetting = _querySettingService.GetCustomerSetting(querySettingParam);
                var newMasterPTItem = newMasterSetting.SettingTypes.ElementAt(0).SettingItems.ElementAt(0) as MasterPTItem;
                Assert.AreEqual(masterPTItem.IS3, newMasterPTItem.IS3);
                Assert.AreEqual(masterPTItem.US3, newMasterPTItem.US3);
                Assert.AreEqual(masterPTItem.S3, newMasterPTItem.S3);
                Assert.AreEqual(masterPTItem.M3, newMasterPTItem.M3);
                Assert.AreEqual(masterPTItem.N3, newMasterPTItem.N3);

                var newMasterBetLimitItem = newMasterSetting.SettingTypes.ElementAt(1).SettingItems.ElementAt(0) as BetLimitItem;
                Assert.AreEqual(masterBetLimitItem.MinBet, newMasterBetLimitItem.MinBet);
                Assert.AreEqual(masterBetLimitItem.MaxBet, newMasterBetLimitItem.MaxBet);
                Assert.AreEqual(masterBetLimitItem.MaxPerMatch, newMasterBetLimitItem.MaxPerMatch);

                var newMasterCommissionItem = newMasterSetting.SettingTypes.ElementAt(2).SettingItems.ElementAt(0) as CommissionItem;
                Assert.AreEqual(masterCommissionItem.CommRate, newMasterCommissionItem.CommRate);

                var newMasterProductStatusItem = newMasterSetting.SettingTypes.ElementAt(3).SettingItems.ElementAt(0) as ProductStatusItem;
                Assert.AreEqual(masterProductStatusItem.IsDisabled, newMasterProductStatusItem.IsDisabled);
                Assert.AreEqual(masterProductStatusItem.Downline.IsDisabled, newMasterProductStatusItem.Downline.IsDisabled);
            }
        }
    }
}