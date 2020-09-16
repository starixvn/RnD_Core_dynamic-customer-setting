namespace Fanex.CustomerSetting.SettingType18.Tests.Models
{
    using NUnit.Framework;
    using SettingType18.Models;

    [TestFixture]
    public class SuperPTOnMemberItemTests
    {
        private SuperPTOnMemberItem _superPTOnMemberItem;

        [SetUp]
        public void SetUp()
        {
            _superPTOnMemberItem = new SuperPTOnMemberItem
            {
                BetTypeId = 1,
                SportTypeId = 1
            };
        }

        [TestCase(0, true, 1, 0)]
        public void Validate_InValidPT_ReturnErrorCode(
            byte s1,
            bool is1,
            byte us1,
            int errorCode)
        {
            //// Arrange
            _superPTOnMemberItem.S1 = s1;
            _superPTOnMemberItem.IS1 = is1;
            _superPTOnMemberItem.US1 = us1;

            //// Act
            var actualResult = _superPTOnMemberItem.Validate();

            //// Assert
            Assert.AreEqual(errorCode, actualResult.Code);
        }

        [Test]
        public void ToXml_ReturnFormattedXml()
        {
            //// Arrange
            _superPTOnMemberItem = new SuperPTOnMemberItem
            {
                S1 = 0,
                IS1 = true,
                US1 = 10,
                IsLive = true,
                SportTypeId = 1,
                BetTypeId = 1
            };

            //// Act
            var actualXmlFormatted = _superPTOnMemberItem.ToXml();

            //// Assert
            var expectedXmlFormatted = "<PT SportType=\"1\" BetType=\"1\" IsLive=\"1\" SuperPT=\"0\" AutoSuperPT=\"10\" IsAutoSuperPT=\"1\"/>";
            Assert.AreEqual(expectedXmlFormatted, actualXmlFormatted);
        }
    }
}