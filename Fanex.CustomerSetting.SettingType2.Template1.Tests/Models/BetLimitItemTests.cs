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
        public void Validate_InvalidPositionTaking_ReturnErrorCode(
            decimal minBet,
            decimal maxBet,
            decimal maxPerMatch,
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
    }
}
