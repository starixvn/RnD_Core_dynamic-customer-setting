namespace Fanex.CustomerSetting.IntegrationTests.Products.Product255
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.CustomerSetting.IntegrationTests.Products.ProductBase;
    using NUnit.Framework;

    [TestFixture]
    public class Product255_ViewCommissionMatrix_IntegrationTests : ViewCommissionMatrix_IntegrationTests
    {
        [Test]
        public void Product255_ViewSuperCommissionMatrix()
        {
            ViewSuperCommissionMatrix();
        }

        [Test]
        public void Product255_ViewMasterCommissionMatrix()
        {
            ViewMasterCommissionMatrix();
        }

        [Test]
        public void Product255_ViewAgentCommissionMatrix()
        {
            ViewAgentCommissionMatrix();
        }

        [Test]
        public void Product255_ViewMemberCommissionMatrix()
        {
            ViewMemberCommissionMatrix();
        }
    }
}
