namespace Fanex.CustomerSetting.SettingType1.Template1.Tests.Models
{
    using Fanex.CustomerSetting.SettingType1.Template1.Models;
    using NUnit.Framework;

    [TestFixture]
    public class AgentPositionTakingItemTests
    {
        private AgentPositionTakingItem _agentPositionTakingItem;

        [SetUp]
        public void Setup()
        {
            _agentPositionTakingItem = new AgentPositionTakingItem
            {
                SportTypeId = 1,
                BetTypeId = 1
            };
        }

        [TestCase(10, true, 3, 4, 1, 1007)]
        [TestCase(10, false, 3, 4, 1, 1008)]
        [TestCase(0, true, 3, 4, 10, 1009)]
        public void Validate_InvalidPositionTaking_ReturnErrorCode(
            byte m2,
            bool im2,
            byte um2,
            byte a2,
            byte n2,
            int errorCode)
        {
            //// Arrange
            _agentPositionTakingItem.M2 = m2;
            _agentPositionTakingItem.IM2 = im2;
            _agentPositionTakingItem.UM2 = um2;
            _agentPositionTakingItem.A2 = a2;
            _agentPositionTakingItem.N2 = n2;

            //// Act
            var actualResult = _agentPositionTakingItem.Validate();

            //// Assert
            Assert.AreEqual(errorCode, actualResult.Code);
        }

        [Test]
        public void ToXml_ReturnFormattedXml()
        {
            //// Arrange
            _agentPositionTakingItem = new AgentPositionTakingItem
            {
                M2 = 0,
                IM2 = true,
                UM2 = 10,
                A2 = 10,
                N2 = 4,
                IsLive = true,
                SportTypeId = 1,
                BetTypeId = 1,
                IsUpdateMasterPTOnMembers = true,
                MasterPTOnMembers = 10
            };

            //// Act
            var actualXmlFormatted = _agentPositionTakingItem.ToXml();

            //// Assert
            var expectedXmlFormatted = "<PT SportType=\"1\" BetType=\"1\" IsLive=\"1\" MasterPT=\"0\" IsAutoMasterPT=\"1\" AutoMasterPT=\"10\" AgentPT=\"10\" AgentMinPT=\"4\" UpdAllMems=\"1\" NewMasterPT=\"10\"/>";
            Assert.AreEqual(expectedXmlFormatted, actualXmlFormatted);
        }
    }
}