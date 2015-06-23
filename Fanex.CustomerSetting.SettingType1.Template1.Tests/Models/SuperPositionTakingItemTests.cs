namespace Fanex.CustomerSetting.SettingType1.Template1.Tests.Models
{
    using Fanex.CustomerSetting.SettingType1.Template1.Models;
    using NUnit.Framework;

    [TestFixture]
    public class SuperPositionTakingItemTests
    {
        private SuperPositionTakingItem _superPositionTakingItem;

        [SetUp]
        public void Setup()
        {
            _superPositionTakingItem = new SuperPositionTakingItem
            {
                SportTypeId = 1,
                BetTypeId = 1
            };
        }

        [TestCase(2, 4, 1, 1, 1001)]
        [TestCase(4, 3, 4, 1, 1002)]
        [TestCase(4, 3, 2, 10, 1003)]
        public void Validate_InvalidPositionTaking_ReturnErrorCode(
            byte s4,
            byte m4,
            byte a4,
            byte n4,
            int errorCode)
        {
            //// Arrange
            _superPositionTakingItem.S4 = s4;
            _superPositionTakingItem.M4 = m4;
            _superPositionTakingItem.A4 = a4;
            _superPositionTakingItem.N4 = n4;

            //// Act
            var actualResult = _superPositionTakingItem.Validate();

            //// Assert
            Assert.AreEqual(errorCode, actualResult.Code);
        }

        [Test]
        public void ToXml_ReturnFormattedXml()
        {
            //// Arrange
            _superPositionTakingItem = new SuperPositionTakingItem
            {
                S4 = 4,
                M4 = 3,
                A4 = 2,
                N4 = 1,
                IsLive = true,
                SportTypeId = 1,
                BetTypeId = 1
            };

            //// Act
            var actualXmlFormatted = _superPositionTakingItem.ToXml();

            //// Assert
            var expectedXmlFormatted = "<PT SportType=\"1\" BetType=\"1\" IsLive=\"1\" SuperPT=\"4\" MasterPT=\"3\" AgentPT=\"2\" SuperMinPT=\"1\"/>";
            Assert.AreEqual(expectedXmlFormatted, actualXmlFormatted);
        }
    }
}