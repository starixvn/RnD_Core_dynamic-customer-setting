namespace Fanex.CustomerSetting.Tests.Criteria
{
    using Fanex.CustomerSetting.Criteria;
    using NUnit.Framework;

    [TestFixture]
    public class ActorStatusQueryCriteriaTests
    {
        [Test]
        public void GetSettingKey_Always_ReturnExpectedResult()
        {
            var criteria = new ActorStatusQueryCriteria();
            var expected = "CustSetting_ActorStatusSelect";
            var actual = criteria.GetSettingKey();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void IsValid_ValidCustId_ReturnTrue()
        {
            var criteria = new ActorStatusQueryCriteria() { CustId = 1 };
            Assert.IsTrue(criteria.IsValid());
        }

        [Test]
        public void IsValid_InvalidCustId_ReturnFalse()
        {
            var criteria = new ActorStatusQueryCriteria() { CustId = 0 };
            Assert.IsFalse(criteria.IsValid());
        }
    }
}
