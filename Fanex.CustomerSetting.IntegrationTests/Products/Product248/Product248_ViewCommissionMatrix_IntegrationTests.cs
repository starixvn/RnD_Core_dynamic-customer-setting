namespace Fanex.CustomerSetting.IntegrationTests.Products.Product248
{
    using Fanex.CustomerSetting.IntegrationTests.Products.ProductBase;
    using NUnit.Framework;

    [TestFixture]
    public class Product248_ViewCommissionMatrix_IntegrationTests : ViewCommissionMatrix_IntegrationTests
    {
        [Test]
        public void Product248_ViewSuperCommissionMatrix()
        {
            ViewSuperCommissionMatrix(Product248Constants.ProductId);
        }

        [Test]
        public void Product248_ViewMasterCommissionMatrix()
        {
            ViewMasterCommissionMatrix(Product248Constants.ProductId);
        }

        [Test]
        public void Product248_ViewAgentCommissionMatrix()
        {
            ViewAgentCommissionMatrix(Product248Constants.ProductId);
        }

        [Test]
        public void Product248_ViewMemberCommissionMatrix()
        {
            ViewMemberCommissionMatrix(Product248Constants.ProductId);
        }
    }
}