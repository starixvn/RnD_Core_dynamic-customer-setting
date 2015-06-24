namespace Fanex.CustomerSetting.Tests.Models
{
    using Fanex.CustomerSetting.Models;
    using NUnit.Framework;

    [TestFixture]
    public class DefaultSettingItemTests
    {
        [TestCase(0, 1)]
        [TestCase(-1, 1)]
        [TestCase(1, -1)]
        [TestCase(1, 0)]
        public void Validate_SportTypeOrBetTypeIsNegativeOrZero_ReturnErrorResult(
            short sportTypeId,
            short betTypeId)
        {
            //// Arrange
            var settingItem = new DefaultSettingItem
                {
                    SportTypeId = sportTypeId,
                    BetTypeId = betTypeId
                };

            //// Act
            var actualResult = settingItem.Validate();

            //// Assert
            Assert.IsFalse(actualResult.IsSuccessful);
        }

        [TestCase(1, 10)]
        [TestCase(10, 1)]
        public void Validate_DataIsValid_ReturnSuccessResult(
            short sportTypeId,
            short betTypeId)
        {
            //// Arrange
            var settingItem = new DefaultSettingItem
            {
                SportTypeId = sportTypeId,
                BetTypeId = betTypeId
            };

            //// Act
            var actualResult = settingItem.Validate();

            //// Assert
            Assert.IsTrue(actualResult.IsSuccessful);
        }

        [TestCase(1, 1, "SportType=\"1\" BetType=\"1\"")]
        [TestCase(10, 1, "SportType=\"10\" BetType=\"1\"")]
        [TestCase(1, 10, "SportType=\"1\" BetType=\"10\"")]
        public void ToXml_ReturnFormattedXml(
            short sportTypeId,
            short betTypeId,
            string expectedXml)
        {
            //// Arrange
            var settingItem = new DefaultSettingItem
            {
                SportTypeId = sportTypeId,
                BetTypeId = betTypeId
            };

            //// Act
            var actualXml = settingItem.ToXml();

            //// Assert
            Assert.AreEqual(expectedXml, actualXml);
        }
    }
}