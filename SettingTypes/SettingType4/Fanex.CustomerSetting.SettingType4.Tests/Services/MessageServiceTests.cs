namespace Fanex.CustomerSetting.SettingType4.Tests.Services
{
    using System.Linq;
    using Fanex.CustomerSetting.Services;
    using Fanex.CustomerSetting.SettingType4.Resources;
    using Fanex.CustomerSetting.SettingType4.Services;
    using Fanex.CustomerSetting.Tests.Models;
    using Fanex.CustomerSetting.Tests.Services;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class MessageServiceTests : MessageServiceBaseTests
    {
        [SetUp]
        public void Setup()
        {
            SettingContext = new SettingContextStub();
            ResourceService = Substitute.For<IResourceService>();
            MessageService = new MessageService(ResourceService);
        }

        [Test]
        public override void TranslateMessage_MessageIdIsNotFound_ReturnUndefinedMessage()
        {
            base.TranslateMessage_MessageIdIsNotFound_ReturnUndefinedMessage();
        }

        [Test]
        public override void TranslateMessage_MessageTypeIsException_ReturnMatchedMessageContent()
        {
            base.TranslateMessage_MessageTypeIsException_ReturnMatchedMessageContent();
        }

        [Test]
        public override void TranslateMessage_MessageTypeIsOther_ReturnRawMessage()
        {
            base.TranslateMessage_MessageTypeIsOther_ReturnRawMessage();
        }

        [TestCase(1, "", "", "")]
        [TestCase(2, "SportType1", "", "SportType1")]
        [TestCase(3, "", "BetType1", "BetType1")]
        [TestCase(4, "SportType1", "BetType1", "SportType1 - BetType1")]
        public void TranslateMessage_MessageTypeIsWrongProductStatus_ReturnMatchedMessageContent(
            byte settingLevel,
            string sportTypeName,
            string betTypeName,
            string expectedSubMessage)
        {
            /// Arrange
            int msgType = 91; //// Wrong Product Status
            var rawMessage = "<Root MsgID=\"60033\" Msg=\"\">"
                                + "<Row SportType=\"1\" BetType=\"1\" />"
                                + "</Root>";
            ResourceService.GetSportTypeName(1, Arg.Any<string>()).Returns(sportTypeName);
            ResourceService.GetBetTypeName(1, Arg.Any<string>()).Returns(betTypeName);

            //// Act
            var actualMessageContent = MessageService.TranslateMessage(msgType, rawMessage, settingLevel, SettingContext.Language);

            //// Assert
            Assert.AreEqual(Message.Msg_60033, actualMessageContent.MainMessage);
            Assert.AreEqual(expectedSubMessage, actualMessageContent.SubMessages.First());
        }
    }
}