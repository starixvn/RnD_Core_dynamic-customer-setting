namespace Fanex.CustomerSetting.IntegrationTests.Services
{
    using System.Linq;
    using Fanex.CustomerSetting.Dtos.Params;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.IntegrationTests.Models;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Services;
    using Fanex.CustomerSetting.Services.Impl;
    using Fanex.CustomerSetting.SettingType1.Template1.Models;
    using Fanex.CustomerSetting.SettingType2.Template1.Models;
    using Fanex.Data.Configuration;
    using NUnit.Framework;

    [TestFixture]
    public class Product255SettingServiceTests
    {
        private const int CompanyId = 8;
        private const int SuperId = 2;
        private const string SuperIds = "2,3,4";
        private const string SuperNames = "SA-RM,SA-US$,SA-TB";
        private const int MasterId = 4174;
        private const string MasterIds = "4174,4300,4356";
        private const string MasterNames = "mrm,mbrm,MYRM";
        private const int AgentId = 1262;
        private const string AgentIds = "1262,1653,2400";
        private const string AgentNames = "wsrm,SKRM,216COM";
        private const int MemberId = 1277;
        private const string MemberIds = "1277,1284,1290";
        private const string MemberNames = "428RM,305RM,302RM";
        private const string Language = "en-US";
        private const string IP = "192.168.1.1";
        private const int SystemId = 1;
        private const string Domain = "www.demo.com";
        private const byte ProductId = 255;
        private const string ProductName = "Product 255";
        private const byte NumOfSettingType = 4;

        private ISettingService _settingService;
        private ISettingContext _settingContext;

        [SetUp]
        public void Setup()
        {
            DbSettingProviderManager.Current.Start();
        }

        [Test]
        public void GetCustomerSetting_CompanyViewSuperSetting()
        {
            //// Arrange
            var actor = new User { Id = SuperId, Level = UserLevel.Company, Name = "Company Name" };
            _settingContext = new SettingContext(actor, Language, IP, SystemId, Domain);
            _settingService = new SettingService(_settingContext);
            var querySettingParam = new QuerySettingParam
            {
                CustomerId = SuperId,
                CustomerLevel = CustomerLevel.Super,
                ProductId = ProductId,
                UplineLevel = null
            };

            //// Act
            var superSetting = _settingService.GetCustomerSetting(querySettingParam);

            //// Assert
            Assert.AreEqual("SA-RM", superSetting.CustomerInfo.UserName);
            Assert.AreEqual(SuperId, superSetting.CustomerId);
            Assert.AreEqual(CustomerLevel.Super, superSetting.CustomerLevel);
            Assert.AreEqual(ProductId, superSetting.ProductId);
            Assert.AreEqual(ProductName, superSetting.ProductName);
            Assert.AreEqual(NumOfSettingType, superSetting.SettingTypes.Count());

            var superPT = superSetting.SettingTypes.ElementAt(0).SettingItems.ElementAt(0) as SuperPTItem;
            Assert.IsNotNull(superPT);
            Assert.GreaterOrEqual(superPT.A4, 0);
            Assert.GreaterOrEqual(superPT.DefaultMaxPT, 0);
            Assert.GreaterOrEqual(superPT.M4, 0);
            Assert.GreaterOrEqual(superPT.S4, 0);
            Assert.GreaterOrEqual(superPT.N4, 0);

            var superBetLimit = superSetting.SettingTypes.ElementAt(1).SettingItems.ElementAt(0) as BetLimitItem;
            Assert.IsNotNull(superBetLimit);
            Assert.GreaterOrEqual(superBetLimit.MaxPerMatch, 0);
        }
    }
}