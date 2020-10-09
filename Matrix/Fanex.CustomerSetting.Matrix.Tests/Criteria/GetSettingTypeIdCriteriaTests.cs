namespace Fanex.CustomerSetting.Matrix.Tests.Criteria
{
    using Fanex.CustomerSetting.Matrix.Criteria;
    using NUnit.Framework;

    [TestFixture]
    public class GetSettingTypeIdCriteriaTests
    {
        [Test]
        public void GetSettingKey_Always_ReturnExpectedResult()
        {
            Assert.AreEqual("CustSetting_ViewMatrixInfo", new GetSettingTypeIdCriteria().GetSettingKey());
        }

        [Test]
        public void IsValid_ValidInput_ReturnTrue()
        {
            var sut = new GetSettingTypeIdCriteria
            {
                ProductId = 1,
                SportTypeId = 0,
            };

            Assert.IsTrue(sut.IsValid());
        }

        [Test]
        public void IsValid_InvalidInput_ReturnFalse()
        {
            var sut = new GetSettingTypeIdCriteria
            {
                SportTypeId = -1
            };

            Assert.IsFalse(sut.IsValid());
        }
    }
}
