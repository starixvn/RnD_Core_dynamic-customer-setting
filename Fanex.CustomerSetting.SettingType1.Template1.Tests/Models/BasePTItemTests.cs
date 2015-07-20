namespace Fanex.CustomerSetting.SettingType1.Template1.Tests.Models
{
    using Fanex.CustomerSetting.SettingType1.Template1.Models;
    using NUnit.Framework;

    [TestFixture]
    public class BasePTItemTests
    {
        [TestCase(false, "SportType=\"1\" BetType=\"1\" IsLive=\"0\"")]
        [TestCase(true, "SportType=\"1\" BetType=\"1\" IsLive=\"1\"")]
        public void ToXml_ReturnFormattedXml(bool isLive, string expectedXml)
        {
            //// Arrange
            var positionTakingItem = new PTItem
                {
                    SportTypeId = 1,
                    BetTypeId = 1,
                    IsLive = isLive
                };

            //// Act
            var actualXmlFormatted = positionTakingItem.ToXml();

            //// Assert
            Assert.AreEqual(expectedXml, actualXmlFormatted);
        }
    }
}