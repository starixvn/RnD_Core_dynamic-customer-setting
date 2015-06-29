namespace Fanex.CustomerSetting.SettingType1.Template1.Tests.Models
{
    using Fanex.CustomerSetting.SettingType1.Template1.Models;
    using NUnit.Framework;

    [TestFixture]
    public class AgentOnMemberPositionTakingItemTests
    {
        private AgentOnMemberPositionTakingItem _agentOnMemberPositionTakingItem;

        [SetUp]
        public void Setup()
        {
            _agentOnMemberPositionTakingItem = new AgentOnMemberPositionTakingItem
            {
                BetTypeId = 1,
                SportTypeId = 1
            };
        }

        [TestCase(100, 1014)]
        [TestCase(10, 0)]
        public void Validate_InvalidPositionTaking_ReturnErrorCode(
            byte a1,
            int errorCode)
        {
            //// Arrange
            _agentOnMemberPositionTakingItem.A1 = a1;

            //// Act
            var actualResult = _agentOnMemberPositionTakingItem.Validate();

            //// Assert
            Assert.AreEqual(errorCode, actualResult.Code);
        }

        [Test]
        public void ToXml_ReturnFormattedXml()
        {
            //// Arrange
            _agentOnMemberPositionTakingItem = new AgentOnMemberPositionTakingItem
            {
                A1 = 10,
                IsLive = true,
                SportTypeId = 1,
                BetTypeId = 1
            };

            //// Act
            var actualXmlFormatted = _agentOnMemberPositionTakingItem.ToXml();

            //// Assert
            var expectedXmlFormatted = "<PT SportType=\"1\" BetType=\"1\" IsLive=\"1\" AgentPT=\"10\"/>";
            Assert.AreEqual(expectedXmlFormatted, actualXmlFormatted);
        }
    }
}