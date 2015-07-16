namespace Fanex.CustomerSetting.IntegrationTests.Services.Product255
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using Fanex.CustomerSetting.Dtos.Params;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.IntegrationTests.Models;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Services;
    using Fanex.CustomerSetting.Services.Impl;
    using Fanex.CustomerSetting.SettingType1.Template1.Models;
    using Fanex.CustomerSetting.SettingType2.Template1.Models;
    using Fanex.CustomerSetting.SettingType3.Template1.Models;
    using Fanex.CustomerSetting.SettingType4.Template1.Models;
    using Fanex.Data.Configuration;
    using NUnit.Framework;

    public class Product255_CustomerIsMaster_IntegrationTests
    {
        private const int MasterId = 14425209;
        private const string MasterUserName = "supertest9001";
        private const int CompanyId = 8;
        private const int SuperId = 14424987;

        private IEnumerable<int> _masterIds = new List<int> { MasterId, 14763802, 14965577 };
        private IEnumerable<string> _masterNames = new List<string> { MasterUserName, "SUPERTEST9000", "supertest9001Raf" };

        private IUpdateSettingService _updateSettingService;
        private IQuerySettingService _querySettingService;
        private ISettingContext _settingContext;

        [SetUp]
        public void Setup()
        {
            DbSettingProviderManager.Current.Start();
        }

        [TestCase(CompanyId, 5)]
        [TestCase(SuperId, 4)]
        public void GetCustomerSetting_ViewingMasterSetting(int actorId, int actorLevelValue)
        {
            //// Arrange
            var actor = new User { Id = actorId, Level = Enumeration.FromValue<UserLevel>(actorLevelValue), Name = "Actor Name" };
            _settingContext = new SettingContext(actor);
            _querySettingService = new QuerySettingService(_settingContext);
            _updateSettingService = new UpdateSettingService(_settingContext);
            var querySettingParam = new QuerySettingParam(MasterId, CustomerLevel.Master, Product255Constants.ProductId, null);

            //// Act
            var masterSetting = _querySettingService.GetCustomerSetting(querySettingParam);

            //// Assert
            Assert.AreEqual(MasterUserName, masterSetting.CustomerInfo.UserName);
            Assert.AreEqual(MasterId, masterSetting.CustomerId);
            Assert.AreEqual(CustomerLevel.Master, masterSetting.CustomerLevel);
            Assert.AreEqual(Product255Constants.ProductId, masterSetting.ProductId);
            Assert.AreEqual(Product255Constants.ProductName, masterSetting.ProductName);
            Assert.AreEqual(Product255Constants.NumOfSettingType, masterSetting.SettingTypes.Count());

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

        [Test]
        public void UpdateCustomerSetting_CompanyUpdatesMasterSetting()
        {
            UpdateCustomerSetting_UpdatingMasterSetting(CompanyId, UserLevel.Company, true);
            UpdateCustomerSetting_UpdatingMasterSetting(CompanyId, UserLevel.Company, false);
        }

        [Test]
        public void UpdateCustomerSetting_SuperUpdatesMasterSetting()
        {
            UpdateCustomerSetting_UpdatingMasterSetting(SuperId, UserLevel.Super, true);
            UpdateCustomerSetting_UpdatingMasterSetting(SuperId, UserLevel.Super, false);
        }

        private void UpdateCustomerSetting_UpdatingMasterSetting(int actorId, UserLevel actorLevel, bool isDisbaledProductStatus)
        {
            //// Arrange
            var actor = new User { Id = actorId, Level = actorLevel, Name = "ActorName" };
            _settingContext = new SettingContext(actor);
            _querySettingService = new QuerySettingService(_settingContext);
            _updateSettingService = new UpdateSettingService(_settingContext);
            var querySettingParam = new QuerySettingParam(MasterId, CustomerLevel.Master, Product255Constants.ProductId, null);

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
            var updateParam = new UpdatingSettingParam(masterSetting, querySettingParam.UplineLevel);
            var updateResult = _updateSettingService.UpdateCustomerSetting(updateParam);

            //// Assert
            Assert.IsTrue(updateResult.IsSuccessful);

            Thread.Sleep(Product255Constants.NumOfSleepSecondsWaitingForReplication); //// Wait for replication data.

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

        [Test]
        public void UpdateMultipleCustomerSetting_SuperUpdatesMultipleMastersSetting()
        {
            //// Arrange
            var actor = new User { Id = SuperId, Level = UserLevel.Super, Name = "ActorName" };
            _settingContext = new SettingContext(actor);
            _updateSettingService = new UpdateSettingService(_settingContext);
            var querySettingParam = new QuerySettingParam(MasterId, CustomerLevel.Master, Product255Constants.ProductId);

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
            var updatingMultipleParam = new UpdatingMultipleSettingParam(masterSetting, _masterIds, _masterNames, querySettingParam.UplineLevel);
            var updateResult = _updateSettingService.UpdateMultipleCustomerSetting(updatingMultipleParam);

            //// Assert
            Assert.IsTrue(updateResult.IsSuccessful);

            Thread.Sleep(Product255Constants.NumOfSleepSecondsWaitingForReplication * 2); //// Wait for replication data.

            foreach (var masterId in _masterIds)
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