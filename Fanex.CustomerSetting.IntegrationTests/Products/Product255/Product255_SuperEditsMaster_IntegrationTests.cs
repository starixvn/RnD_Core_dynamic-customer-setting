namespace Fanex.CustomerSetting.IntegrationTests.Products.Product255
{
    using Fanex.CustomerSetting.Constants;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.IntegrationTests.Products.ProductBase;
    using NUnit.Framework;

    [TestFixture]
    public class Product255_SuperEditsMaster_IntegrationTests : SuperEditsMaster_IntegrationTests
    {
        [Test]
        public void Product255_SuperViewsMasterSetting()
        {
            ViewingMasterSetting(SettingInfo.SuperId, Level.Super);
        }

        [Test]
        public void Product255_CompanyViewsMasterSetting()
        {
            ViewingMasterSetting(SettingInfo.CompanyId, Level.Company);
        }

        [Test]
        public void Product255_SuperUpdatesMasterSetting()
        {
            SuperUpdatesMasterSetting();
        }

        [Test]
        public void Product255_CompanyUpdatesMasterSetting()
        {
            CompanyUpdatesMasterSetting();
        }

        [Test]
        public void Product255_SuperUpdatesMultipleMastersSetting()
        {
            SuperUpdatesMultipleMastersSetting();
        }
    }
}