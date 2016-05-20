namespace Fanex.CustomerSetting.SettingType1.Tests.Models
{
    using Fanex.CustomerSetting.SettingType1.Models;
    using NUnit.Framework;

    [TestFixture]
    public class AgentPTItemTests
    {
        private static object[] agentPTItems =
        {
            new object[]
            {
                (byte)0,
                true,
                (byte)1,
                true,
                true,
                "<PT SportType=\"1\" BetType=\"1\" " +
                "IsLive=\"1\" MasterPT=\"0\" IsAutoMasterPT=\"1\" AutoMasterPT=\"1\" AgentPT=\"10\" MinPT=\"4\" IsUpdAllMems=\"1\" UpdAllMems_Value=\"10\"/>"
            },
            new object[]
            {
                (byte)0,
                true,
                (byte)1,
                false,
                false,
                "<PT SportType=\"1\" BetType=\"1\" " +
                "IsLive=\"0\" MasterPT=\"0\" IsAutoMasterPT=\"1\" AutoMasterPT=\"1\" AgentPT=\"10\" MinPT=\"4\" IsUpdAllMems=\"0\" UpdAllMems_Value=\"10\"/>"
            },
            new object[]
            {
                (byte)1,
                false,
                (byte)0,
                true,
                true,
                "<PT SportType=\"1\" BetType=\"1\" " +
                "IsLive=\"1\" MasterPT=\"1\" IsAutoMasterPT=\"0\" AutoMasterPT=\"0\" AgentPT=\"10\" MinPT=\"4\" IsUpdAllMems=\"1\" UpdAllMems_Value=\"10\"/>"
            }
        };

        private AgentPTItem _agentPTItem;

        [SetUp]
        public void Setup()
        {
            _agentPTItem = new AgentPTItem
            {
                SportTypeId = 1,
                BetTypeId = 1
            };
        }

        [TestCase(0, true, 3, 4, 10, 1009)]
        [TestCase(0, true, 3, 4, 1, 0)]
        public void Validate_InValidPT_ReturnErrorCode(
            byte m2,
            bool im2,
            byte um2,
            byte a2,
            byte n2,
            int errorCode)
        {
            //// Arrange
            _agentPTItem.M2 = m2;
            _agentPTItem.IM2 = im2;
            _agentPTItem.UM2 = um2;
            _agentPTItem.A2 = a2;
            _agentPTItem.N2 = n2;

            //// Act
            var actualResult = _agentPTItem.Validate();

            //// Assert
            Assert.AreEqual(errorCode, actualResult.Code);
        }

        [Test, TestCaseSource("agentPTItems")]
        public void ToXml_ReturnFormattedXml(
            byte m2,
            bool im2,
            byte um2,
            bool isLive,
            bool isUpdateMasterPTOnMember,
            string expectedXmlFormatted)
        {
            //// Arrange
            _agentPTItem = new AgentPTItem
            {
                M2 = m2,
                IM2 = im2,
                UM2 = um2,
                A2 = (byte)10,
                N2 = (byte)4,
                IsLive = isLive,
                SportTypeId = 1,
                BetTypeId = 1,
                IsUpdateMasterPTOnMembers = isUpdateMasterPTOnMember,
                MasterPTOnMembers = (byte)10
            };

            //// Act
            var actualXmlFormatted = _agentPTItem.ToXml();

            //// Assert
            Assert.AreEqual(expectedXmlFormatted, actualXmlFormatted);
        }
    }
}