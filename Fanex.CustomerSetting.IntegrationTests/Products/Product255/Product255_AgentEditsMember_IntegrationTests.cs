namespace Fanex.CustomerSetting.IntegrationTests.Products.Product255
{
    using Fanex.CustomerSetting.Constants;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.IntegrationTests.Products.ProductBase;
    using NUnit.Framework;

    [TestFixture]
    public class Product255_AgentEditsMember_IntegrationTests : AgentEditsMember_IntegrationTests
    {
        [Test]
        public void Product255_AgentViewsMemberSetting()
        {
            AgentViewsMemberSetting(SettingInfo.AgentId, Level.Agent);
        }

        [Test]
        public void Product255_CompanyViewsMemberSettingByAgentView()
        {
            AgentViewsMemberSetting(SettingInfo.CompanyId, Level.Company);
        }

        [Test]
        public void Product255_AgentUpdatesMemberSetting()
        {
            AgentUpdatesMemberSetting();
        }

        [Test]
        public void Product255_CompanyUpdatesMemberSettingByAgentView()
        {
            CompanyUpdatesMemberSettingByAgentView();
        }

        [Test]
        public void Product255_AgentUpdatesMultipleMembersSetting()
        {
            AgentUpdatesMultipleMembersSetting();
        }
    }
}