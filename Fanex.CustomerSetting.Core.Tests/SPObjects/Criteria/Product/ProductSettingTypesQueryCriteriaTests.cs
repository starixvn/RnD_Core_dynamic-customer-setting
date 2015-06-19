namespace Fanex.CustomerSetting.Core.Tests.SPObjects.Criteria.Product
{
    using Fanex.CustomerSetting.Core.SPObjects.Criteria.Product;
    using NUnit.Framework;

    [TestFixture]
    public class ProductSettingTypesQueryCriteriaTests
    {
        [TestCase(0)]
        public void IsValid_ProductIdIsZero_ReturnFalse(byte productId)
        {
            //// Arrange
            var criteria = CreateProductSettingTypesQueryCriteriaFakeObject();
            criteria.ProductId = productId;

            //// Act
            var actualResult = criteria.IsValid();

            //// Arrange
            Assert.IsFalse(actualResult);
        }

        [TestCase(1)]
        [TestCase(125)]
        public void IsValid_ProductIdIsGreaterThanZero_ReturnTrue(byte productId)
        {
            //// Arrange
            var criteria = CreateProductSettingTypesQueryCriteriaFakeObject();
            criteria.ProductId = productId;

            //// Act
            var actualResult = criteria.IsValid();

            //// Arrange
            Assert.IsTrue(actualResult);
        }

        private ProductSettingTypesQueryCriteria CreateProductSettingTypesQueryCriteriaFakeObject()
        {
            return new ProductSettingTypesQueryCriteria
            {
                ProductId = 1
            };
        }
    }
}