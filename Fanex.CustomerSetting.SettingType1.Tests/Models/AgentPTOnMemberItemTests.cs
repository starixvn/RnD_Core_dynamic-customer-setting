namespace Fanex.CustomerSetting.SettingType1.Tests.Models
{
    using Fanex.CustomerSetting.SettingType1.Models;
    using NUnit.Framework;

    [TestFixture]
    public class AgentPTOnMemberItemTests
    {
        private AgentPTOnMemberItem _agentPTOnMemberItem;

        [SetUp]
        public void Setup()
        {
            _agentPTOnMemberItem = new AgentPTOnMemberItem
            {
                BetTypeId = 1,
                SportTypeId = 1
            };
        }

        [TestCase(10, 0)]
        public void Validate_InValidPT_ReturnErrorCode(
            byte a1,
            int errorCode)
        {
            //// Arrange
            _agentPTOnMemberItem.A1 = a1;

            //// Act
            var actualResult = _agentPTOnMemberItem.Validate();

            //// Assert
            Assert.AreEqual(errorCode, actualResult.Code);
        }

        [Test]
        public void ToXml_ReturnFormattedXml()
        {
            //// Arrange
            _agentPTOnMemberItem = new AgentPTOnMemberItem
            {
                A1 = 10,
                IsLive = true,
                SportTypeId = 1,
                BetTypeId = 1
            };

            //// Act
            var actualXmlFormatted = _agentPTOnMemberItem.ToXml();

            //// Assert
            var expectedXmlFormatted = "<PT SportType=\"1\" BetType=\"1\" IsLive=\"1\" AgentPT=\"10\"/>";
            Assert.AreEqual(expectedXmlFormatted, actualXmlFormatted);
        }
    }
}