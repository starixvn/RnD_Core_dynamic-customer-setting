namespace Fanex.CustomerSetting.SettingType5.Tests.Services
{
    using System.Linq;
    using CustomerSetting.Services;
    using NSubstitute;
    using NUnit.Framework;
    using Resources;
    using SettingType5.Services;

    [TestFixture]
    public class MessageServiceTests
    {
        private IResourceService _resourceService;
        private IMessageService _messageService;

        [SetUp]
        public void Setup()
        {
            _resourceService = Substitute.For<IResourceService>();
            _messageService = new MessageService(_resourceService);
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public void TranslateMessage_Always_MainMessageIsMsg1(byte settingLevel)
        {
            // Arrange
            var msgType = 51;
            var rawMessage = "<Root MsgID=\"1\" Msg=\"\">"
                                + "<Row SportType=\"1\" BetType=\"1\" />"
                                + "</Root>";

            // Act
            var actualMessageContent = _messageService.TranslateMessage(msgType, rawMessage, settingLevel, Arg.Any<string>());

            // Assert
            Assert.AreEqual(Message.Msg_1, actualMessageContent.MainMessage);
        }

        [Test]
        public void TranslateMessage_SettingLevelIsProduct_SubMessageIsEmpty()
        {
            // Arrange
            var msgType = 51;
            byte settingLevel = 1;
            var rawMessage = "<Root MsgID=\"1\" Msg=\"\">"
                                + "<Row SportType=\"1\" BetType=\"1\" />"
                                + "</Root>";

            // Act
            var actualMessageContent = _messageService.TranslateMessage(msgType, rawMessage, settingLevel, "en-US");

            // Assert
            Assert.IsEmpty(actualMessageContent.SubMessages.First());
        }

        [Test]
        public void TranslateMessage_SettingLevelIsSportType_SubMessageIsSportTypeName()
        {
            // Arrange
            var msgType = 51;
            byte settingLevel = 2;
            var rawMessage = "<Root MsgID=\"1\" Msg=\"\">"
                                + "<Row SportType=\"1\" BetType=\"1\" />"
                                + "</Root>";
            _resourceService.GetSportTypeName(1, Arg.Any<string>()).Returns("Sport type 1");

            // Act
            var actualMessageContent = _messageService.TranslateMessage(msgType, rawMessage, settingLevel, "en-US");

            // Assert
            string expectedSubMsg = "Sport type 1";
            Assert.AreEqual(expectedSubMsg, actualMessageContent.SubMessages.First());
        }

        [Test]
        public void TranslateMessage_SettingLevelIsBetType_SubMessageIsBetTypeName()
        {
            // Arrange
            var msgType = 51;
            byte settingLevel = 3;
            var rawMessage = "<Root MsgID=\"1\" Msg=\"\">"
                                + "<Row SportType=\"1\" BetType=\"1\" />"
                                + "</Root>";
            _resourceService.GetBetTypeName(1, Arg.Any<string>()).Returns("Bet type 1");

            // Act
            var actualMessageContent = _messageService.TranslateMessage(msgType, rawMessage, settingLevel, "en-US");

            // Assert
            string expectedSubMsg = "Bet type 1";
            Assert.AreEqual(expectedSubMsg, actualMessageContent.SubMessages.First());
        }

        [Test]
        public void TranslateMessage_SettingLevelIsSportTypeBetType_SubMessageIsSportTypeNameHyphenBetTypeName()
        {
            // Arrange
            var msgType = 51;
            byte settingLevel = 4;
            var rawMessage = "<Root MsgID=\"1\" Msg=\"\">"
                                + "<Row SportType=\"1\" BetType=\"1\" />"
                                + "</Root>";
            _resourceService.GetSportTypeName(1, Arg.Any<string>()).Returns("Sport type 1");
            _resourceService.GetBetTypeName(1, Arg.Any<string>()).Returns("Bet type 1");

            // Act
            var actualMessageContent = _messageService.TranslateMessage(msgType, rawMessage, settingLevel, "en-US");

            // Assert
            string expectedSubMsg = "Sport type 1 - Bet type 1";
            Assert.AreEqual(expectedSubMsg, actualMessageContent.SubMessages.First());
        }
    }
}