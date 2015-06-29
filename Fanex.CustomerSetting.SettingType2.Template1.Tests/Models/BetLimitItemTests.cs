namespace Fanex.CustomerSetting.SettingType2.Template1.Tests.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.CustomerSetting.SettingType2.Template1.Models;
    using NUnit.Framework;

    [TestFixture]
    public class BetLimitItemTests
    {
        private BetLimitItem _betLimitItem;

        [SetUp]
        public void Setup()
        {
            _betLimitItem = new BetLimitItem
            {
                BetTypeId = 1,
                SportTypeId = 1
            };
        }

        [TestCase(10, 0, 120, 7001)]
        [TestCase(4, 10, 7, 7002)]
        [TestCase(-1, 2, 3, 7003)]
        [TestCase(1, -2, 3, 7004)]
        [TestCase(1, 2, -3, 7005)]
        [TestCase(1, 2, 100000000000, 7006)]
        [TestCase(1, 100000000000, 2, 7006)]
        [TestCase(100000000000, 2, 1, 7006)]
        [TestCase(1, 2, 3, 0)]
        public void Validate_InvalidBetLimit_ReturnErrorCode(
            long minBet,
            long maxBet,
            long maxPerMatch,
            int errorCode)
        {
            //// Arrange
            _betLimitItem.MinBet = minBet;
            _betLimitItem.MaxBet = maxBet;
            _betLimitItem.MaxPerMatch = maxPerMatch;

            //// Act
            var actualResult = _betLimitItem.Validate();

            //// Assert
            Assert.AreEqual(errorCode, actualResult.Code);
        }

        [Test]
        public void ToXml_ReturnFormattedXml()
        {
            //// Arrange
            _betLimitItem = new BetLimitItem
            {
                SportTypeId = 1,
                BetTypeId = 1,
                MinBet = 10,
                MaxBet = 11,
                MaxPerMatch = 12
            };
            
            //// Act
            var actualXmlFormatted = _betLimitItem.ToXml();

            //// Assert
            var expectedXmlFormatted = "<BetLimit SportType=\"1\" BetType=\"1\" MinBet=\"10\" MaxBet=\"11\" MaxPerMatch=\"12\"/>";
            Assert.AreEqual(expectedXmlFormatted, actualXmlFormatted);
        }
    }
}
