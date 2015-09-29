namespace Fanex.CustomerSetting.SettingType1.Tests.Models
{
    using Fanex.CustomerSetting.SettingType1.Models;
    using NUnit.Framework;

    [TestFixture]
    public class MasterPTItemTests
    {
        private MasterPTItem _masterPTItem;

        [SetUp]
        public void Setup()
        {
            _masterPTItem = new MasterPTItem
            {
                SportTypeId = 1,
                BetTypeId = 1
            };
        }

        [TestCase(10, true, 3, 4, 1, 1004)]
        [TestCase(10, false, 3, 4, 1, 1005)]
        [TestCase(0, true, 3, 4, 10, 1006)]
        [TestCase(0, true, 3, 4, 1, 0)]
        public void Validate_InValidPT_ReturnErrorCode(
            byte s3,
            bool is3,
            byte us3,
            byte m3,
            byte n3,
            int errorCode)
        {
            //// Arrange
            _masterPTItem.S3 = s3;
            _masterPTItem.IS3 = is3;
            _masterPTItem.US3 = us3;
            _masterPTItem.M3 = m3;
            _masterPTItem.N3 = n3;

            //// Act
            var actualResult = _masterPTItem.Validate();

            //// Assert
            Assert.AreEqual(errorCode, actualResult.Code);
        }

        [Test]
        public void ToXml_ReturnFormattedXml()
        {
            //// Arrange
            _masterPTItem = new MasterPTItem
            {
                S3 = 0,
                IS3 = true,
                US3 = 10,
                M3 = 10,
                N3 = 4,
                IsLive = true,
                SportTypeId = 1,
                BetTypeId = 1,
                IsUpdateSuperPTOnMembers = true,
                SuperPTOnMembers = 10
            };

            //// Act
            var actualXmlFormatted = _masterPTItem.ToXml();

            //// Assert
            var expectedXmlFormatted = "<PT SportType=\"1\" BetType=\"1\" IsLive=\"1\" SuperPT=\"0\" IsAutoSuperPT=\"1\" AutoSuperPT=\"10\" MasterPT=\"10\" MinPT=\"4\" IsUpdAllMems=\"1\" UpdAllMems_Value=\"10\"/>";
            Assert.AreEqual(expectedXmlFormatted, actualXmlFormatted);
        }
    }
}