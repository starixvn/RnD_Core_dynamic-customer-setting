namespace Fanex.CustomerSetting.SettingType1.Template1.Tests.Services
{
    using System.Linq;
    using Fanex.CustomerSetting.Services;
    using Fanex.CustomerSetting.SettingType1.Template1.Resources;
    using Fanex.CustomerSetting.SettingType1.Template1.Services;
    using Fanex.CustomerSetting.Tests.Models;
    using Fanex.CustomerSetting.Tests.Services;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class MessageServiceTests : BaseMessageServiceTests
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

        [TestCase(1, false, "", "", "")]
        [TestCase(1, true, "", "", " (Live)")]
        [TestCase(2, false, "SportType1", "", "SportType1")]
        [TestCase(2, true, "SportType1", "", "SportType1 (Live)")]
        [TestCase(3, false, "", "BetType1", "BetType1")]
        [TestCase(3, true, "", "BetType1", "BetType1 (Live)")]
        [TestCase(4, false, "SportType1", "BetType1", "SportType1 - BetType1")]
        [TestCase(4, true, "SportType1", "BetType1", "SportType1 - BetType1 (Live)")]
        public void TranslateMessage_MessageTypeIsWrongPositionTaking_ReturnMatchedMessageContent(
            byte settingLevel,
            bool isLive,
            string sportTypeName,
            string betTypeName,
            string expectedSubMessage)
        {
            /// Arrange
            int msgType = 5; //// WrongPositionTaking
            var rawMessage = "<Root MsgID=\"20001\" Msg=\"Wrong position taking.\">"
                                + "<Row SportType=\"1\" BetType=\"1\" IsLive=\""
                                + (isLive ? "1" : "0")
                                + "\"/>"
                                + "</Root>";
            ResourceService.GetSportTypeName(1, Arg.Any<string>()).Returns(sportTypeName);
            ResourceService.GetBetTypeName(1, Arg.Any<string>()).Returns(betTypeName);

            //// Act
            var actualMessageContent = MessageService.TranslateMessage(msgType, rawMessage, settingLevel, SettingContext.Language);

            //// Assert
            Assert.AreEqual(PTMessage.Msg_20001, actualMessageContent.MainMessage);
            Assert.AreEqual(expectedSubMessage, actualMessageContent.SubMessages.First());
        }

        [TestCase(1, false, "", "", " - Min: 0.12")]
        [TestCase(1, true, "", "", " (Live) - Min Live: 0.12")]
        [TestCase(2, false, "SportType1", "", "SportType1 - Min: 0.12")]
        [TestCase(2, true, "SportType1", "", "SportType1 (Live) - Min Live: 0.12")]
        [TestCase(3, false, "", "BetType1", "BetType1 - Min: 0.12")]
        [TestCase(3, true, "", "BetType1", "BetType1 (Live) - Min Live: 0.12")]
        [TestCase(4, false, "SportType1", "BetType1", "SportType1 - BetType1 - Min: 0.12")]
        [TestCase(4, true, "SportType1", "BetType1", "SportType1 - BetType1 (Live) - Min Live: 0.12")]
        public void TranslateMessage_MessageTypeIsMinPTViolation_ReturnMatchedMessageContent(
            byte settingLevel,
            bool isLive,
            string sportTypeName,
            string betTypeName,
            string expectedSubMessage)
        {
            /// Arrange
            int msgType = 2; //// MinPTViolation
            var rawMessage = "<Root MsgID=\"20029\" Msg=\"Wrong position taking.\">"
                                + "<Row SportType=\"1\" BetType=\"1\" IsLive=\""
                                + (isLive ? "1" : "0")
                                + "\" MinPT=\"12\"/>"
                                + "</Root>";
            ResourceService.GetSportTypeName(1, Arg.Any<string>()).Returns(sportTypeName);
            ResourceService.GetBetTypeName(1, Arg.Any<string>()).Returns(betTypeName);

            //// Act
            var actualMessageContent = MessageService.TranslateMessage(msgType, rawMessage, settingLevel, SettingContext.Language);

            //// Assert
            Assert.AreEqual(PTMessage.Msg_20029, actualMessageContent.MainMessage);
            Assert.AreEqual(expectedSubMessage, actualMessageContent.SubMessages.First());
        }

        [TestCase(1, false, "", "", " - Max: 0.12")]
        [TestCase(1, true, "", "", " (Live) - Max Live: 0.12")]
        [TestCase(2, false, "SportType1", "", "SportType1 - Max: 0.12")]
        [TestCase(2, true, "SportType1", "", "SportType1 (Live) - Max Live: 0.12")]
        [TestCase(3, false, "", "BetType1", "BetType1 - Max: 0.12")]
        [TestCase(3, true, "", "BetType1", "BetType1 (Live) - Max Live: 0.12")]
        [TestCase(4, false, "SportType1", "BetType1", "SportType1 - BetType1 - Max: 0.12")]
        [TestCase(4, true, "SportType1", "BetType1", "SportType1 - BetType1 (Live) - Max Live: 0.12")]
        public void TranslateMessage_MessageTypeIsMaxPTViolation_ReturnMatchedMessageContent(
            byte settingLevel,
            bool isLive,
            string sportTypeName,
            string betTypeName,
            string expectedSubMessage)
        {
            /// Arrange
            int msgType = 3; //// MaxPTViolation
            var rawMessage = "<Root MsgID=\"20028\" Msg=\"Wrong position taking.\">"
                                + "<Row SportType=\"1\" BetType=\"1\" IsLive=\""
                                + (isLive ? "1" : "0")
                                + "\" MaxPT=\"12\"/>"
                                + "</Root>";
            ResourceService.GetSportTypeName(1, Arg.Any<string>()).Returns(sportTypeName);
            ResourceService.GetBetTypeName(1, Arg.Any<string>()).Returns(betTypeName);

            //// Act
            var actualMessageContent = MessageService.TranslateMessage(msgType, rawMessage, settingLevel, SettingContext.Language);

            //// Assert
            Assert.AreEqual(PTMessage.Msg_20028, actualMessageContent.MainMessage);
            Assert.AreEqual(expectedSubMessage, actualMessageContent.SubMessages.First());
        }

        [TestCase(1, false, "", "", " - Min: 0.11 - Max: 0.13")]
        [TestCase(1, true, "", "", " (Live) - Min Live: 0.11 - Max Live: 0.13")]
        [TestCase(2, false, "SportType1", "", "SportType1 - Min: 0.11 - Max: 0.13")]
        [TestCase(2, true, "SportType1", "", "SportType1 (Live) - Min Live: 0.11 - Max Live: 0.13")]
        [TestCase(3, false, "", "BetType1", "BetType1 - Min: 0.11 - Max: 0.13")]
        [TestCase(3, true, "", "BetType1", "BetType1 (Live) - Min Live: 0.11 - Max Live: 0.13")]
        [TestCase(4, false, "SportType1", "BetType1", "SportType1 - BetType1 - Min: 0.11 - Max: 0.13")]
        [TestCase(4, true, "SportType1", "BetType1", "SportType1 - BetType1 (Live) - Min Live: 0.11 - Max Live: 0.13")]
        public void TranslateMessage_MessageTypeIsMinMaxPTViolation_ReturnMatchedMessageContent(
            byte settingLevel,
            bool isLive,
            string sportTypeName,
            string betTypeName,
            string expectedSubMessage)
        {
            /// Arrange
            int msgType = 4; //// MinMaxPTViolation
            var rawMessage = "<Root MsgID=\"20033\" Msg=\"Wrong position taking.\">"
                                + "<Row SportType=\"1\" BetType=\"1\" IsLive=\""
                                + (isLive ? "1" : "0")
                                + "\" MinPT=\"11\" MaxPT=\"13\"/>"
                                + "</Root>";
            ResourceService.GetSportTypeName(1, Arg.Any<string>()).Returns(sportTypeName);
            ResourceService.GetBetTypeName(1, Arg.Any<string>()).Returns(betTypeName);

            //// Act
            var actualMessageContent = MessageService.TranslateMessage(msgType, rawMessage, settingLevel, SettingContext.Language);

            //// Assert
            Assert.AreEqual(PTMessage.Msg_20033, actualMessageContent.MainMessage);
            Assert.AreEqual(expectedSubMessage, actualMessageContent.SubMessages.First());
        }

        [TestCase(1, false, "", "", " - UserNames: username1, username2")]
        [TestCase(1, true, "", "", " (Live) - UserNames: username1, username2")]
        [TestCase(2, false, "SportType1", "", "SportType1 - UserNames: username1, username2")]
        [TestCase(2, true, "SportType1", "", "SportType1 (Live) - UserNames: username1, username2")]
        [TestCase(3, false, "", "BetType1", "BetType1 - UserNames: username1, username2")]
        [TestCase(3, true, "", "BetType1", "BetType1 (Live) - UserNames: username1, username2")]
        [TestCase(4, false, "SportType1", "BetType1", "SportType1 - BetType1 - UserNames: username1, username2")]
        [TestCase(4, true, "SportType1", "BetType1", "SportType1 - BetType1 (Live) - UserNames: username1, username2")]
        public void TranslateMessage_MessageTypeIsDownlinePTViolation_ReturnMatchedMessageContent(
            byte settingLevel,
            bool isLive,
            string sportTypeName,
            string betTypeName,
            string expectedSubMessage)
        {
            /// Arrange
            int msgType = 1; //// DownlinePTViolation
            var rawMessage = "<Root MsgID=\"20001\" Msg=\"Wrong position taking.\">"
                                + "<Row SportType=\"1\" BetType=\"1\" IsLive=\""
                                + (isLive ? "1" : "0")
                                + "\" UserName=\"username1\"/> "
                                + "<Row SportType=\"1\" BetType=\"1\" IsLive=\""
                                + (isLive ? "1" : "0")
                                + "\" UserName=\"username2\"/> "
                                + "</Root>";
            ResourceService.GetSportTypeName(1, Arg.Any<string>()).Returns(sportTypeName);
            ResourceService.GetBetTypeName(1, Arg.Any<string>()).Returns(betTypeName);

            //// Act
            var actualMessageContent = MessageService.TranslateMessage(msgType, rawMessage, settingLevel, SettingContext.Language);

            //// Assert
            Assert.AreEqual(PTMessage.Msg_20001, actualMessageContent.MainMessage);
            Assert.AreEqual(expectedSubMessage, actualMessageContent.SubMessages.First());
        }
    }
}