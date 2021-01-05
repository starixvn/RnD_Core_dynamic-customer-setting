namespace Fanex.CustomerSetting.Matrix.Tests.Criteria
{
    using Fanex.CustomerSetting.Matrix.Criteria;
    using NUnit.Framework;

    [TestFixture]
    public class GetSettingTypeLevelCriteriaTests
    {
        [Test]
        public void GetSettingKey_Always_ReturnExpectedResult()
        {
            Assert.AreEqual("CustSetting_SettingLevel", new GetSettingTypeLevelCriteria().GetSettingKey());
        }

        [Test]
        public void IsValid_ValidInput_ReturnTrue()
        {
            var sut = new GetSettingTypeLevelCriteria
            {
                ProductId = 1,
                SportTypeId = 0,
                SettingTypeId = 1
            };

            Assert.IsTrue(sut.IsValid());
        }

        [Test]
        public void IsValid_InvalidInput_ReturnFalse()
        {
            var sut = new GetSettingTypeLevelCriteria
            {
                ProductId = 0,
                SportTypeId = 0
            };

            Assert.IsFalse(sut.IsValid());
        }
    }
}
