namespace Fanex.CustomerSetting.IntegrationTests.Services.Product255
{
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

    public class Product255_CustomerIsSuper_IntegrationTests
    {
        private const int CompanyId = 8;
        private const int SuperId = 14424987;
        private const string SuperUserName = "supertest90";

        private IUpdateSettingService _updateSettingService;
        private IQuerySettingService _querySettingService;
        private ISettingContext _settingContext;

        [SetUp]
        public void Setup()
        {
            DbSettingProviderManager.Current.Start();
        }

        [Test]
        public void GetCustomerSetting_CompanyViewsSuperSetting()
        {
            //// Arrange
            var actor = new User { Id = SuperId, Level = UserLevel.Company, Name = "Company Name" };
            _settingContext = new SettingContext(actor);
            _updateSettingService = new UpdateSettingService(_settingContext);
            _querySettingService = new QuerySettingService(_settingContext);
            var querySettingParam = new QuerySettingParam(SuperId, CustomerLevel.Super, Product255Constants.ProductId, null);

            //// Act
            var superSetting = _querySettingService.GetCustomerSetting(querySettingParam);

            //// Assert
            Assert.AreEqual(SuperUserName, superSetting.CustomerInfo.UserName);
            Assert.AreEqual(SuperId, superSetting.CustomerId);
            Assert.AreEqual(CustomerLevel.Super, superSetting.CustomerLevel);
            Assert.AreEqual(Product255Constants.ProductId, superSetting.ProductId);
            Assert.AreEqual(Product255Constants.ProductName, superSetting.ProductName);
            Assert.AreEqual(Product255Constants.NumOfSettingType, superSetting.SettingTypes.Count());

            var superPTITem = superSetting.SettingTypes.ElementAt(0).SettingItems.ElementAt(0) as SuperPTItem;
            Assert.IsNotNull(superPTITem);
            Assert.GreaterOrEqual(superPTITem.A4, 1);
            Assert.GreaterOrEqual(superPTITem.DefaultMaxPT, 1);
            Assert.GreaterOrEqual(superPTITem.M4, 1);
            Assert.GreaterOrEqual(superPTITem.S4, 1);
            Assert.GreaterOrEqual(superPTITem.N4, 0);

            var superBetLimitItem = superSetting.SettingTypes.ElementAt(1).SettingItems.ElementAt(0) as BetLimitItem;
            Assert.IsNotNull(superBetLimitItem);
            Assert.GreaterOrEqual(superBetLimitItem.MinBet, 0);
            Assert.GreaterOrEqual(superBetLimitItem.MaxBet, 0);
            Assert.GreaterOrEqual(superBetLimitItem.MaxPerMatch, 0);

            var superCommissionItem = superSetting.SettingTypes.ElementAt(2).SettingItems.ElementAt(0) as CommissionItem;
            Assert.IsNotNull(superCommissionItem);
            Assert.GreaterOrEqual(superCommissionItem.CommRate, 0);
            Assert.LessOrEqual(superCommissionItem.CommRate, 0.01);

            var superProductStatusItem = superSetting.SettingTypes.ElementAt(3).SettingItems.ElementAt(0) as ProductStatusItem;
            Assert.IsNotNull(superProductStatusItem);
            Assert.IsNotNull(superProductStatusItem.Downline);
        }

        [Test]
        public void UpdateCustomerSetting_CompanyUpdatesSuperSetting()
        {
            //// Arrange
            var actor = new User { Id = SuperId, Level = UserLevel.Company, Name = "Company Name" };
            _settingContext = new SettingContext(actor);
            _updateSettingService = new UpdateSettingService(_settingContext);
            _querySettingService = new QuerySettingService(_settingContext);
            var querySettingParam = new QuerySettingParam(SuperId, CustomerLevel.Super, Product255Constants.ProductId, null);

            var superSetting = _querySettingService.GetCustomerSetting(querySettingParam);
            var superPTITem = superSetting.SettingTypes.ElementAt(0).SettingItems.ElementAt(0) as SuperPTItem;
            superPTITem.S4 = (byte)(superPTITem.S4 == 90 ? 89 : 90);
            superPTITem.M4 = (byte)(superPTITem.M4 == 88 ? 87 : 88);
            superPTITem.A4 = (byte)(superPTITem.A4 == 73 ? 72 : 73);
            superPTITem.N4 = (byte)(superPTITem.N4 == 3 ? 4 : 3);

            var superBetLimitItem = superSetting.SettingTypes.ElementAt(1).SettingItems.ElementAt(0) as BetLimitItem;
            superBetLimitItem.MinBet = superBetLimitItem.MinBet == 45 ? 44 : 45;
            superBetLimitItem.MaxBet = superBetLimitItem.MaxBet == 550 ? 551 : 550;
            superBetLimitItem.MaxPerMatch = superBetLimitItem.MaxPerMatch == 550550 ? 550551 : 550550;

            var superCommissionItem = superSetting.SettingTypes.ElementAt(2).SettingItems.ElementAt(0) as CommissionItem;
            superCommissionItem.CommRate = superCommissionItem.CommRate == 0.0095m ? 0.0096m : 0.0095m;

            var superProductStatusItem = superSetting.SettingTypes.ElementAt(3).SettingItems.ElementAt(0) as ProductStatusItem;
            superProductStatusItem.IsDisabled = false;
            superProductStatusItem.Downline.IsDisabled = false;

            //// Act
            var updateResult = _updateSettingService.UpdateCustomerSetting(new UpdatingSettingParam(superSetting, querySettingParam.UplineLevel));

            //// Assert
            Assert.IsTrue(updateResult.IsSuccessful);
            Thread.Sleep(Product255Constants.NumOfSleepSecondsWaitingForReplication); //// Wait for replication data.

            var newSuperSetting = _querySettingService.GetCustomerSetting(querySettingParam);
            var newSuperPTItem = newSuperSetting.SettingTypes.ElementAt(0).SettingItems.ElementAt(0) as SuperPTItem;
            Assert.AreEqual(superPTITem.S4, newSuperPTItem.S4);
            Assert.AreEqual(superPTITem.M4, newSuperPTItem.M4);
            Assert.AreEqual(superPTITem.A4, newSuperPTItem.A4);
            Assert.AreEqual(superPTITem.N4, newSuperPTItem.N4);

            var newSuperBetLimitItem = newSuperSetting.SettingTypes.ElementAt(1).SettingItems.ElementAt(0) as BetLimitItem;
            Assert.AreEqual(superBetLimitItem.MinBet, newSuperBetLimitItem.MinBet);
            Assert.AreEqual(superBetLimitItem.MaxBet, newSuperBetLimitItem.MaxBet);
            Assert.AreEqual(superBetLimitItem.MaxPerMatch, newSuperBetLimitItem.MaxPerMatch);

            var newSuperCommissionItem = newSuperSetting.SettingTypes.ElementAt(2).SettingItems.ElementAt(0) as CommissionItem;
            Assert.AreEqual(superCommissionItem.CommRate, newSuperCommissionItem.CommRate);

            var newSuperProductStatusItem = newSuperSetting.SettingTypes.ElementAt(3).SettingItems.ElementAt(0) as ProductStatusItem;
            Assert.AreEqual(superProductStatusItem.IsDisabled, newSuperProductStatusItem.IsDisabled);
            Assert.AreEqual(superProductStatusItem.Downline.IsDisabled, newSuperProductStatusItem.Downline.IsDisabled);
        }
    }
}