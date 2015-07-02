namespace Fanex.CustomerSetting.SettingType1.Template1.Tests.Models
{
    using Fanex.CustomerSetting.SettingType1.Template1.Models;
    using NUnit.Framework;

    [TestFixture]
    public class MasterPTOnMemberItemTests
    {
        private MasterPTOnMemberItem _masterPTOnMemberItem;

        public void SetUp()
        {
            _masterPTOnMemberItem = new MasterPTOnMemberItem
            {
                BetTypeId = 1,
                SportTypeId = 1
            };
        }

        [TestCase(2, true, 1, 1012)]
        [TestCase(4, false, 4, 1013)]
        [TestCase(0, false, 100, 1014)]
        [TestCase(100, true, 4, 1014)]
        [TestCase(0, true, 4, 0)]
        public void Validate_InvalidPT_ReturnErrorCode(
            byte m1,
            bool im1,
            byte um1,
            int errorCode)
        {
            //// Arrange
            _masterPTOnMemberItem.M1 = m1;
            _masterPTOnMemberItem.IM1 = im1;
            _masterPTOnMemberItem.UM1 = um1;

            //// Act
            var actualResult = _masterPTOnMemberItem.Validate();

            //// Assert
            Assert.AreEqual(errorCode, actualResult.Code);
        }

        [Test]
        public void ToXml_ReturnFormattedXml()
        {
            //// Arrange
            _masterPTOnMemberItem = new MasterPTOnMemberItem
            {
                M1 = 0,
                IM1 = true,
                UM1 = 10,
                IsLive = true,
                SportTypeId = 1,
                BetTypeId = 1
            };

            //// Act
            var actualXmlFormatted = _masterPTOnMemberItem.ToXml();

            //// Assert
            var expectedXmlFormatted = "<PT SportType=\"1\" BetType=\"1\" IsLive=\"1\" MasterPT=\"0\" AutoMasterPT=\"10\" IsAutoMasterPT=\"1\"/>";
            Assert.AreEqual(expectedXmlFormatted, actualXmlFormatted);
        }
    }
}