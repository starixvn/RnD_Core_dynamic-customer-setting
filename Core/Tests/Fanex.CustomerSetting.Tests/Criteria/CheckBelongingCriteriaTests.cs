namespace Fanex.CustomerSetting.Tests.Criteria
{
    using Fanex.CustomerSetting.Criteria;
    using NUnit.Framework;

    [TestFixture]
    public class CheckBelongingCriteriaTests
    {
        [Test]
        public void GetSettingKey_Always_ReturnExpectedResult()
        {
            var criteria = new CheckBelongingCriteria();
            var expected = "CustSetting_SameLineCheck";
            var actual = criteria.GetSettingKey();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void IsValid_ValidCustId_ReturnTrue()
        {
            var criteria = new CheckBelongingCriteria() { ActorId = 1, CustomerId = 1 };
            Assert.IsTrue(criteria.IsValid());
        }

        [Test]
        [TestCase(0, 0)]
        [TestCase(0, 1)]
        [TestCase(1, 0)]
        public void IsValid_InvalidCustId_ReturnFalse(int customerId, int actorId)
        {
            var criteria = new CheckBelongingCriteria() { ActorId = actorId, CustomerId = customerId };
            Assert.IsFalse(criteria.IsValid());
        }
    }
}
