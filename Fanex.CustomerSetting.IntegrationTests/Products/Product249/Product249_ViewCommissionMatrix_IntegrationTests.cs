namespace Fanex.CustomerSetting.IntegrationTests.Products.Product249
{
    using Fanex.CustomerSetting.IntegrationTests.Products.ProductBase;
    using NUnit.Framework;

    [TestFixture]
    public class Product249_ViewCommissionMatrix_IntegrationTests : ViewCommissionMatrix_IntegrationTests
    {
        [Test]
        public void Product249_ViewSuperCommissionMatrix()
        {
            ViewSuperCommissionMatrix(Product249Constants.ProductId);
        }

        [Test]
        public void Product249_ViewMasterCommissionMatrix()
        {
            ViewMasterCommissionMatrix(Product249Constants.ProductId);
        }

        [Test]
        public void Product249_ViewAgentCommissionMatrix()
        {
            ViewAgentCommissionMatrix(Product249Constants.ProductId);
        }

        [Test]
        public void Product249_ViewMemberCommissionMatrix()
        {
            ViewMemberCommissionMatrix(Product249Constants.ProductId);
        }
    }
}