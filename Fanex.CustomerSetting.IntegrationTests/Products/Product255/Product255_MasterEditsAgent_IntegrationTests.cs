namespace Fanex.CustomerSetting.IntegrationTests.Products.Product255
{
    using Fanex.CustomerSetting.Constants;
    using Fanex.CustomerSetting.IntegrationTests.Products.ProductBase;
    using NUnit.Framework;

    [TestFixture]
    public class Product255_MasterEditsAgent_IntegrationTests : MasterEditsAgent_IntegrationTests
    {
        [Test]
        public void Product255_MasterViewsAgentSetting()
        {
            ViewingAgentSetting(SettingInfo.MasterId, Level.Master);
        }

        [Test]
        public void Product255_CompanyViewsAgentSetting()
        {
            ViewingAgentSetting(SettingInfo.CompanyId, Level.Company);
        }

        [Test]
        public void Product255_MasterUpdatesAgentSetting()
        {
            MasterUpdatesAgentSetting();
        }

        [Test]
        public void Product255_CompanyUpdatesAgentSetting()
        {
            CompanyUpdatesAgentSetting();
        }

        [Test]
        public void Product255_MasterUpdatesMultipleAgentsSetting()
        {
            MasterUpdatesMultipleAgentsSetting();
        }
    }
}