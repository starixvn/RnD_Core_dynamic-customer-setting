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
    public class Product255_SuperEditsMember_IntegrationTests : SuperEditsMember_IntegrationTests
    {
        [Test]
        public void Product255_SuperViewsMemberSetting()
        {
            SuperViewsMemberSetting(SettingInfo.SuperId, Level.Super);
        }

        [Test]
        public void Product255_CompanyViewsMemberSettingBySuperView()
        {
            SuperViewsMemberSetting(SettingInfo.CompanyId, Level.Company);
        }

        [Test]
        public void Product255_SuperUpdatesPTOnMember()
        {
            SuperUpdatesPTOnMember();
        }

        [Test]
        public void Product255_CompanyUpdatesSuperPTOnMember()
        {
            CompanyUpdatesSuperPTOnMember();
        }

        [Test]
        public void Product255_SuperUpdatesMultipleMastersSetting()
        {
            SuperUpdatesPTOnMultipleMembers();
        }
    }
}
