namespace Fanex.CustomerSetting.SettingType1.Tests.Models
{
    using Fanex.CustomerSetting.SettingType1.Models;
    using NUnit.Framework;

    [TestFixture]
    public class SuperPTItemTests
    {
        private SuperPTItem _superPTItem;

        [SetUp]
        public void Setup()
        {
            _superPTItem = new SuperPTItem
            {
                SportTypeId = 1,
                BetTypeId = 1
            };
        }

        [TestCase(2, 4, 1, 1, 1001)]
        [TestCase(4, 3, 4, 1, 1002)]
        [TestCase(4, 3, 2, 10, 1003)]
        [TestCase(2, 1, 1, 1, 0)]
        public void Validate_InValidPT_ReturnErrorCode(
            byte s4,
            byte m4,
            byte a4,
            byte n4,
            int errorCode)
        {
            //// Arrange
            _superPTItem.S4 = s4;
            _superPTItem.M4 = m4;
            _superPTItem.A4 = a4;
            _superPTItem.N4 = n4;

            //// Act
            var actualResult = _superPTItem.Validate();

            //// Assert
            Assert.AreEqual(errorCode, actualResult.Code);
        }

        [Test]
        public void ToXml_ReturnFormattedXml()
        {
            //// Arrange
            _superPTItem = new SuperPTItem
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
            var actualXmlFormatted = _superPTItem.ToXml();

            //// Assert
            var expectedXmlFormatted = "<PT SportType=\"1\" BetType=\"1\" IsLive=\"1\" SuperPT=\"4\" MasterPT=\"3\" AgentPT=\"2\" MinPT=\"1\"/>";
            Assert.AreEqual(expectedXmlFormatted, actualXmlFormatted);
        }
    }
}