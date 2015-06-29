namespace Fanex.CustomerSetting.SettingType1.Template1.Tests.Services
{
    using System.Linq;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Resource.Enumerations;
    using Fanex.CustomerSetting.Resource.Resources;
    using Fanex.CustomerSetting.Resource.Services;
    using Fanex.CustomerSetting.SettingType1.Template1.Resources;
    using Fanex.CustomerSetting.SettingType1.Template1.Services;
    using Fanex.CustomerSetting.Tests.Models;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class PositionTakingMessageServiceTests
    {
        private ISettingContext _settingContext;
        private IResourceService _resourceService;
        private IMessageService _positionTakingMessageService;

        [SetUp]
        public void Setup()
        {
            _settingContext = new SettingContextStub();
            _resourceService = Substitute.For<IResourceService>();
            _positionTakingMessageService = new PositionTakingMessageService(_resourceService);
        }

        #region Simple Message

        [Test]
        public void TranslateMessage_MessageTypeIsSimpleMessage_ReturnMatchedMessageContent()
        {
            //// Arrange
            int msgType = (int)MessageType.Simple;
            string rawMessage = "<Root MsgID=\"90001\" Msg=\"Update process is unsuccessful!\"/>";
            byte settingLevel = 1;

            //// Act
            var actualMessageContent = _positionTakingMessageService.TranslateMessage(msgType, rawMessage, settingLevel, _settingContext.Language);

            //// Assert
            Assert.AreEqual(Error.Msg_90001, actualMessageContent.MainMessage);
        }

        [Test]
        public void TranslateMessage_MessageIdIsNotFound_ReturnUndefinedMessage()
        {
            //// Arrange
            int msgType = (int)MessageType.Simple;
            string rawMessage = "<Root MsgID=\"121212\"/>";
            byte settingLevel = 1;

            //// Act
            var actualMessageContent = _positionTakingMessageService.TranslateMessage(msgType, rawMessage, settingLevel, _settingContext.Language);

            //// Assert
            Assert.AreEqual("Undefined error message 121212", actualMessageContent.MainMessage);
        }

        #endregion Simple Message

        #region Other Message

        [Test]
        public void TranslateMessage_MessageIdIsOther_ReturnRawMessage()
        {
            //// Arrange
            int msgType = (int)MessageType.Other;
            string rawMessage = "Raw Message";
            byte settingLevel = 1;

            //// Act
            var actualMessageContent = _positionTakingMessageService.TranslateMessage(msgType, rawMessage, settingLevel, _settingContext.Language);

            //// Assert
            Assert.AreEqual(rawMessage, actualMessageContent.MainMessage);
        }

        #endregion Other Message

        #region Position Taking Message

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
            _resourceService.GetSportTypeName(1, Arg.Any<string>()).Returns(sportTypeName);
            _resourceService.GetBetTypeName(1, Arg.Any<string>()).Returns(betTypeName);

            //// Act
            var actualMessageContent = _positionTakingMessageService.TranslateMessage(msgType, rawMessage, settingLevel, _settingContext.Language);

            //// Assert
            Assert.AreEqual(PositionTakingError.Msg_20001, actualMessageContent.MainMessage);
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
            _resourceService.GetSportTypeName(1, Arg.Any<string>()).Returns(sportTypeName);
            _resourceService.GetBetTypeName(1, Arg.Any<string>()).Returns(betTypeName);

            //// Act
            var actualMessageContent = _positionTakingMessageService.TranslateMessage(msgType, rawMessage, settingLevel, _settingContext.Language);

            //// Assert
            Assert.AreEqual(PositionTakingError.Msg_20029, actualMessageContent.MainMessage);
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
            _resourceService.GetSportTypeName(1, Arg.Any<string>()).Returns(sportTypeName);
            _resourceService.GetBetTypeName(1, Arg.Any<string>()).Returns(betTypeName);

            //// Act
            var actualMessageContent = _positionTakingMessageService.TranslateMessage(msgType, rawMessage, settingLevel, _settingContext.Language);

            //// Assert
            Assert.AreEqual(PositionTakingError.Msg_20028, actualMessageContent.MainMessage);
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
            _resourceService.GetSportTypeName(1, Arg.Any<string>()).Returns(sportTypeName);
            _resourceService.GetBetTypeName(1, Arg.Any<string>()).Returns(betTypeName);

            //// Act
            var actualMessageContent = _positionTakingMessageService.TranslateMessage(msgType, rawMessage, settingLevel, _settingContext.Language);

            //// Assert
            Assert.AreEqual(PositionTakingError.Msg_20033, actualMessageContent.MainMessage);
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
            _resourceService.GetSportTypeName(1, Arg.Any<string>()).Returns(sportTypeName);
            _resourceService.GetBetTypeName(1, Arg.Any<string>()).Returns(betTypeName);

            //// Act
            var actualMessageContent = _positionTakingMessageService.TranslateMessage(msgType, rawMessage, settingLevel, _settingContext.Language);

            //// Assert
            Assert.AreEqual(PositionTakingError.Msg_20001, actualMessageContent.MainMessage);
            Assert.AreEqual(expectedSubMessage, actualMessageContent.SubMessages.First());
        }

        #endregion Position Taking Message
    }
}