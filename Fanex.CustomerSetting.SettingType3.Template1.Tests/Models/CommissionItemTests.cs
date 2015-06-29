namespace Fanex.CustomerSetting.SettingType3.Template1.Tests.Models
{
    using Fanex.CustomerSetting.SettingType3.Template1.Models;
    using NUnit.Framework;

    [TestFixture]
    public class CommissionItemTests
    {
        private CommissionItem _commissionItem;

        [SetUp]
        public void Setup()
        {
            _commissionItem = new CommissionItem
            {
                SportTypeId = 1,
                BetTypeId = 1
            };
        }

        [TestCase(-1, 5101)]
        [TestCase(-10, 5101)]
        [TestCase(1.1, 5102)]
        [TestCase(0.9, 0)]
        public void Validate_InvalidCommission_ReturnErrorCode(
            decimal commissionRate,
            int errorCode)
        {
            //// Arrange
            _commissionItem.CommRate = commissionRate;

            //// Act
            var actualResult = _commissionItem.Validate();

            //// Assert
            Assert.AreEqual(errorCode, actualResult.Code);
        }

        [Test]
        public void ToXml_ReturnFormattedXml()
        {
            //// Arrange
            _commissionItem = new CommissionItem
            {
                SportTypeId = 1,
                BetTypeId = 1,
                CommRate = 0.12m
            };

            //// Act
            var actualXmlFormatted = _commissionItem.ToXml();

            //// Assert
            var expectedXmlFormatted = "<Discount SportType=\"1\" BetType=\"1\" Discount=\"0.12\"/>";
            Assert.AreEqual(expectedXmlFormatted, actualXmlFormatted);
        }
    }
}