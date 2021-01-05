namespace Fanex.CustomerSetting.Tests.Criteria
{
    using Fanex.CustomerSetting.Criteria;
    using NUnit.Framework;

    [TestFixture]
    public class ProductNamesQueryCriteriaTests
    {
        [Test]
        public void GetSettingKey_Always_ReturnExpectedResult()
        {
            var criteria = new ProductNamesQueryCriteria();
            var expected = "CustSetting_ProductNamesSelect";
            var actual = criteria.GetSettingKey();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void IsValid_Always_ReturnTrue()
        {
            var criteria = new ProductNamesQueryCriteria();
            Assert.IsTrue(criteria.IsValid());
        }
    }
}
