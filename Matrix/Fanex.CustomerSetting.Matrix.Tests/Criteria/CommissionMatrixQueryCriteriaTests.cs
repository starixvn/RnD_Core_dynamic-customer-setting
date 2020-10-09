namespace Fanex.CustomerSetting.Matrix.Tests.Criteria
{
    using Fanex.CustomerSetting.Matrix.Criteria;
    using NUnit.Framework;

    [TestFixture]
    public class CommissionMatrixQueryCriteriaTests
    {
        [Test]
        public void GetSettingKey_Always_ReturnExpectedResult()
        {
            Assert.AreEqual("CustSetting_CommMatrix", new CommissionMatrixQueryCriteria().GetSettingKey());
        }

        [Test]
        public void IsValid_ValidInput_ReturnTrue()
        {
            var sut = new CommissionMatrixQueryCriteria {
                ProductId = 1,
                UserName = "test"
            };

            Assert.IsTrue(sut.IsValid());
        }

        [Test]
        public void IsValid_InvalidInput_ReturnFalse()
        {
            var sut = new CommissionMatrixQueryCriteria {
                ProductId = 0,
                UserName = null,
            };

            Assert.IsFalse(sut.IsValid());
        }
    }
}
