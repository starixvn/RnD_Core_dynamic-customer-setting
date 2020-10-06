namespace Fanex.CustomerSetting.Tests.Criteria
{
    using Fanex.CustomerSetting.Criteria;
    using NUnit.Framework;

    [TestFixture]
    public class UnderMaintainanceQueryCriteriaTests
    {
        [Test]
        public void GetSettingKey_Always_ReturnExpectedResult()
        {
            var criteria = new UnderMaintainanceQueryCriteria();
            var expected = "Acc_CheckUM";
            var actual = criteria.GetSettingKey();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FunctionType_Always_Equal1()
        {
            var criteria = new UnderMaintainanceQueryCriteria() { ProductId = 1, CustId = 1, SettingTypeId = null };
            Assert.IsTrue(criteria.FunctionType == 1);
        }

        [Test]
        public void IsValid_ValidCustId_ReturnTrue()
        {
            var criteria = new UnderMaintainanceQueryCriteria() { ProductId = 1, CustId = 1, SettingTypeId = null};
            Assert.IsTrue(criteria.IsValid());
        }

        [Test]
        [TestCase(0, 0)]
        [TestCase(0, 1)]
        [TestCase(1, 0)]
        public void IsValid_InvalidCustId_ReturnFalse(byte productId, int custId)
        {
            var criteria = new UnderMaintainanceQueryCriteria() { ProductId = productId, CustId = custId };
            Assert.IsFalse(criteria.IsValid());
        }
    }
}
