namespace Fanex.CustomerSetting.SettingType1.Template1.Tests.Models
{
    using Fanex.CustomerSetting.SettingType1.Template1.Models;
    using NUnit.Framework;

    [TestFixture]
    public class SuperOnMemberPositionTakingItemTests
    {
        private SuperOnMemberPositionTakingItem _superOnMemberPositionTakingItem;

        [SetUp]
        public void SetUp()
        {
            _superOnMemberPositionTakingItem = new SuperOnMemberPositionTakingItem
            {
                BetTypeId = 1,
                SportTypeId = 1
            };
        }

        [TestCase(2, true, 1, 1010)]
        [TestCase(4, false, 4, 1011)]
        public void Validate_InvalidPositionTaking_ReturnErrorCode(
            byte s1,
            bool is1,
            byte us1,
            int errorCode)
        {
            //// Arrange
            _superOnMemberPositionTakingItem.S1 = s1;
            _superOnMemberPositionTakingItem.IS1 = is1;
            _superOnMemberPositionTakingItem.US1 = us1;

            //// Act
            var actualResult = _superOnMemberPositionTakingItem.Validate();

            //// Assert
            Assert.AreEqual(errorCode, actualResult.Code);
        }

        [Test]
        public void ToXml_ReturnFormattedXml()
        {
            //// Arrange
            _superOnMemberPositionTakingItem = new SuperOnMemberPositionTakingItem
            {
                S1 = 0,
                IS1 = true,
                US1 = 10,
                IsLive = true,
                SportTypeId = 1,
                BetTypeId = 1
            };

            //// Act
            var actualXmlFormatted = _superOnMemberPositionTakingItem.ToXml();

            //// Assert
            var expectedXmlFormatted = "<PT SportType=\"1\" BetType=\"1\" IsLive=\"1\" SuperPT=\"0\" AutoSuperPT=\"10\" IsAutoSuperPT=\"1\"/>";
            Assert.AreEqual(expectedXmlFormatted, actualXmlFormatted);
        }
    }
}