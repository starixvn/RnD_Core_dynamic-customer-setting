namespace Fanex.CustomerSetting.Matrix.Tests.Criteria
{
    using Fanex.CustomerSetting.Matrix.Criteria;
    using NUnit.Framework;

    [TestFixture]
    public class CustomerLineQueryCriteriaTests
    {
        [Test]
        public void GetSettingKey_Always_ReturnExpectedResult()
        {
            Assert.AreEqual("CustSetting_CustomerLineInfoSelect", new CustomerLineQueryCriteria().GetSettingKey());
        }

        [Test]
        public void IsValid_ValidInput_ReturnTrue()
        {
            var sut = new CustomerLineQueryCriteria
            {
                UserName = "test"
            };

            Assert.IsTrue(sut.IsValid());
        }

        [Test]
        public void IsValid_InvalidInput_ReturnFalse()
        {
            var sut = new CustomerLineQueryCriteria
            {
                UserName = null,
            };

            Assert.IsFalse(sut.IsValid());
        }
    }
}
