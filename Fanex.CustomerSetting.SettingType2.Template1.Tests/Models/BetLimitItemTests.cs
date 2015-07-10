namespace Fanex.CustomerSetting.SettingType2.Template1.Tests.Models
{
    using Fanex.CustomerSetting.SettingType2.Template1.Dtos;
    using Fanex.CustomerSetting.SettingType2.Template1.Models;
    using NUnit.Framework;

    [TestFixture]
    public class BetLimitItemTests
    {
        [TestCase(10, 0, 120, 7001)]
        [TestCase(4, 10, 7, 7002)]
        [TestCase(-1, 2, 3, 7003)]
        [TestCase(1, -2, 3, 7004)]
        [TestCase(1, 2, -3, 7005)]
        [TestCase(1, 2, 100000000000, 7006)]
        [TestCase(1, 100000000000, 2, 7006)]
        [TestCase(100000000000, 2, 1, 7006)]
        [TestCase(1, 2, 3, 0)]
        [TestCase(0, 0, 0, 0)]
        public void Validate_InvalidBetLimit_ReturnErrorCode(
            long minBet,
            long maxBet,
            long maxPerMatch,
            int errorCode)
        {
            //// Arrange
            var betLimitItem = new BetLimitItem
            {
                BetTypeId = 1,
                SportTypeId = 1,
                MinBet = minBet,
                MaxBet = maxBet,
                MaxPerMatch = maxPerMatch
            };

            //// Act
            var actualResult = betLimitItem.Validate();

            //// Assert
            Assert.AreEqual(errorCode, actualResult.Code);
        }

        [TestCase(1, 1, 10, 11, 12, "<BetLimit SportType=\"1\" BetType=\"1\" MinBet=\"10\" MaxBet=\"11\" MaxPerMatch=\"12\"/>")]
        [TestCase(10, 1, 0, 0, 0, "<BetLimit SportType=\"10\" BetType=\"1\" MinBet=\"0\" MaxBet=\"0\" MaxPerMatch=\"0\"/>")]
        [TestCase(1, 10, 101, 111, 121, "<BetLimit SportType=\"1\" BetType=\"10\" MinBet=\"101\" MaxBet=\"111\" MaxPerMatch=\"121\"/>")]
        public void ToXml_ReturnFormattedXml(
            short sportTypeId,
            short betTypeId,
            decimal minBet,
            decimal maxBet,
            decimal maxPerMatch,
            string expectedXmlFormatted)
        {
            //// Arrange
            var betLimitItem = new BetLimitItem
            {
                SportTypeId = sportTypeId,
                BetTypeId = betTypeId,
                MinBet = minBet,
                MaxBet = maxBet,
                MaxPerMatch = maxPerMatch
            };

            //// Act
            var actualXmlFormatted = betLimitItem.ToXml();

            //// Assert
            Assert.AreEqual(expectedXmlFormatted, actualXmlFormatted);
        }

        [Test]
        public void Constructor_PopulatesDataFromBetLimitItemDto()
        {
            //// Arrange
            var betLimitDto = new BetLimitItemDto
            {
                SportTypeId = 1,
                BetTypeId = 1,
                MinBet = 1,
                MaxBet = 5,
                MaxPerMatch = 10,
                MinBetMin = 0,
                MaxBetMax = 7,
                MaxPerMatchMax = 15
            };

            //// Act
            var actualBetLimitItem = new BetLimitItem(betLimitDto);

            //// Assert
            Assert.AreEqual(1, actualBetLimitItem.SportTypeId);
            Assert.AreEqual(1, actualBetLimitItem.BetTypeId);
            Assert.AreEqual(1, actualBetLimitItem.MinBet);
            Assert.AreEqual(5, actualBetLimitItem.MaxBet);
            Assert.AreEqual(10, actualBetLimitItem.MaxPerMatch);
            Assert.AreEqual(0, actualBetLimitItem.Constraint.MinBet);
            Assert.AreEqual(7, actualBetLimitItem.Constraint.MaxBet);
            Assert.AreEqual(15, actualBetLimitItem.Constraint.MaxPerMatch);
        }
    }
}