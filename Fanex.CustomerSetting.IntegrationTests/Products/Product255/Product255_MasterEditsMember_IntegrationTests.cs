namespace Fanex.CustomerSetting.IntegrationTests.Products.Product255
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.CustomerSetting.Constants;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.IntegrationTests.Products.ProductBase;
    using NUnit.Framework;

    [TestFixture]
    public class Product255_MasterEditsMember_IntegrationTests : MasterEditsMember_IntegrationTests
    {
        [Test]
        public void Product255_MasterViewsMemberSetting()
        {
            MasterViewsMemberSetting(SettingInfo.MasterId, Level.Master);
        }

        [Test]
        public void Product255_CompanyViewsMemberSettingByMasterView()
        {
            MasterViewsMemberSetting(SettingInfo.CompanyId, Level.Company);
        }

        [Test]
        public void Product255_MasterUpdatesPTOnMember()
        {
            MasterUpdatesPTOnMember();
        }

        [Test]
        public void Product255_CompanyUpdatesMasterPTOnMember()
        {
            CompanyUpdatesMasterPTOnMember();
        }

        [Test]
        public void Product255_MasterUpdatesPTOnMultipleMembers()
        {
            MasterUpdatesPTOnMultipleMembers();
        }
    }
}
