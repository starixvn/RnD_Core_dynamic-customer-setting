namespace Fanex.CustomerSetting.NumberGames2.Tests.Criteria
{
    using Fanex.CustomerSetting.NumberGames2.Criteria;
    using NUnit.Framework;

    [TestFixture]
    class DirectUplineCriteriaTests
    {
        private readonly DirectUplineCriteria directUplineCriteria = new DirectUplineCriteria();

        [Test]
        public void GetSettingKey_ReturnExpectedResult()
        {
            var expectedResult = "Acc_CustomerDirectUplineSel";
            var actualResult = directUplineCriteria.GetSettingKey();
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        public void IsValid_CustIdGreaterThanZero_ReturnTrue()
        {
            directUplineCriteria.CustId = 1;
            Assert.IsTrue(directUplineCriteria.IsValid());
        }

        [Test]
        public void IsValid_CustIdIsZero_ReturnFalse()
        {
            directUplineCriteria.CustId = 0;
            Assert.IsFalse(directUplineCriteria.IsValid());
        }
    }
}
