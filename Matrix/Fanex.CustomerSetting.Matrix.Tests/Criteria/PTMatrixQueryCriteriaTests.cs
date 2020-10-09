namespace Fanex.CustomerSetting.Matrix.Tests.Criteria
{
    using Fanex.CustomerSetting.Matrix.Criteria;
    using NUnit.Framework;

    [TestFixture]
    public class PTMatrixQueryCriteriaTests
    {
        [Test]
        public void GetSettingKey_Always_ReturnExpectedResult()
        {
            Assert.AreEqual("CustSetting_PTMatrix", new PTMatrixQueryCriteria().GetSettingKey());
        }

        [Test]
        public void IsValid_ValidInput_ReturnTrue()
        {
            var sut = new PTMatrixQueryCriteria
            {
                SportType = 1,
                BetType = 1,
                UserName = "test"
            };

            Assert.IsTrue(sut.IsValid());
        }

        [Test]
        public void IsValid_InvalidInput_ReturnFalse()
        {
            var sut = new PTMatrixQueryCriteria
            {
                UserName = null
            };

            Assert.IsFalse(sut.IsValid());

            sut = new PTMatrixQueryCriteria
            {
                SportType = 0
            };

            Assert.IsFalse(sut.IsValid());

            sut = new PTMatrixQueryCriteria
            {
                BetType = 0
            };

            Assert.IsFalse(sut.IsValid());
        }
    }
}
