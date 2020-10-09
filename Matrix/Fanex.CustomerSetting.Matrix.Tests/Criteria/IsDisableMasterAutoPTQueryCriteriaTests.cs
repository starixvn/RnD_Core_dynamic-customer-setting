namespace Fanex.CustomerSetting.Matrix.Tests.Criteria
{
    using Fanex.CustomerSetting.Matrix.Criteria;
    using NUnit.Framework;

    [TestFixture]
    public class IsDisableMasterAutoPTQueryCriteriaTests
    {
        [Test]
        public void GetSettingKey_Always_ReturnExpectedResult()
        {
            Assert.AreEqual("CustSetting_DisableMasterAutoPTSelect", new IsDisableMasterAutoPTQueryCriteria().GetSettingKey());
        }

        [Test]
        public void IsValid_ValidInput_ReturnTrue()
        {
            var sut = new IsDisableMasterAutoPTQueryCriteria
            {
                UserName = "test"
            };

            Assert.IsTrue(sut.IsValid());
        }

        [Test]
        public void IsValid_InvalidInput_ReturnFalse()
        {
            var sut = new IsDisableMasterAutoPTQueryCriteria
            {
                UserName = null
            };

            Assert.IsFalse(sut.IsValid());
        }
    }
}
