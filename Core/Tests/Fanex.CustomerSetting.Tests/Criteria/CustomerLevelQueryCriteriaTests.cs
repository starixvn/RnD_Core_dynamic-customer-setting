namespace Fanex.CustomerSetting.Tests.Criteria
{
    using Fanex.CustomerSetting.Criteria;
    using NUnit.Framework;

    [TestFixture]
    public class CustomerLevelQueryCriteriaTests
    {
        [Test]
        public void GetSettingKey_Always_ReturnExpectedResult()
        {
            var criteria = new CustomerLevelQueryCriteria();
            var expected = "CustSetting_CustomerLevelSelect";
            var actual = criteria.GetSettingKey();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void IsValid_ValidCustId_ReturnTrue()
        {
            var criteria = new CustomerLevelQueryCriteria() { CustId = 1 };
            Assert.IsTrue(criteria.IsValid());
        }

        [Test]
        public void IsValid_InvalidCustId_ReturnFalse()
        {
            var criteria = new CustomerLevelQueryCriteria() { CustId = 0 };
            Assert.IsFalse(criteria.IsValid());
        }
    }
}
