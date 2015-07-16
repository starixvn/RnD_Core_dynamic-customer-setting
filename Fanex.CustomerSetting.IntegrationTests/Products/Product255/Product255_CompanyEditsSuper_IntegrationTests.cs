namespace Fanex.CustomerSetting.IntegrationTests.Products.Product255
{
    using Fanex.CustomerSetting.IntegrationTests.Products.ProductBase;
    using NUnit.Framework;

    [TestFixture]
    public class Product255_CompanyEditsSuper_IntegrationTests : CompanyEditsSuper_IntegrationTests
    {
        [Test]
        public void Product255_CompanyViewsSuperSetting()
        {
            CompanyViewsSuperSetting();
        }

        [Test]
        public void Product255_CompanyUpdatesSuperSetting()
        {
            CompanyUpdatesSuperSetting();
        }
    }
}