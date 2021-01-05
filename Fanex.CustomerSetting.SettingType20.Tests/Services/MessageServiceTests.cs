namespace Fanex.CustomerSetting.SettingType20.Tests.Services
{
    using System.Linq;
    using Constants;
    using CustomerSetting.Models.Messages;
    using CustomerSetting.Services;
    using NSubstitute;
    using NUnit.Framework;
    using Resources;
    using SettingType20.Services;

    public class MessageServiceTests
    {
        private IResourceService _resourceService;
        private IMessageService _messageService;
        private int _msgType;
        private string _rawMessage;

        [SetUp]
        public void Setup()
        {
            _resourceService = Substitute.For<IResourceService>();
            _messageService = new MessageService(_resourceService);
            _msgType = MessageTypes.BetLimitViolation;
            _rawMessage = "<Root MsgID=\"10003\" Msg=\"\">"
                            + "<Row SportType=\"1\" BetType=\"1\" />"
                        + "</Root>";
        }

        [TestCase(1, 10006)]
        [TestCase(2, 10006)]
        [TestCase(3, 10006)]
        [TestCase(4, 10006)]
        [TestCase(1, 10007)]
        [TestCase(2, 10007)]
        [TestCase(3, 10007)]
        [TestCase(4, 10007)]
        [TestCase(1, 10016)]
        [TestCase(2, 10016)]
        [TestCase(3, 10016)]
        [TestCase(4, 10016)]
        [TestCase(1, 10017)]
        [TestCase(2, 10017)]
        [TestCase(3, 10017)]
        [TestCase(4, 10017)]
        [TestCase(1, 10019)]
        [TestCase(2, 10019)]
        [TestCase(3, 10019)]
        [TestCase(4, 10019)]
        [TestCase(1, 10020)]
        [TestCase(2, 10020)]
        [TestCase(3, 10020)]
        [TestCase(4, 10020)]
        [TestCase(1, 10035)]
        [TestCase(2, 10035)]
        [TestCase(3, 10035)]
        [TestCase(4, 10035)]
        [TestCase(1, 10036)]
        [TestCase(2, 10036)]
        [TestCase(3, 10036)]
        [TestCase(4, 10036)]
        [TestCase(1, 10037)]
        [TestCase(2, 10037)]
        [TestCase(3, 10037)]
        [TestCase(4, 10037)]
        [TestCase(1, 10038)]
        [TestCase(2, 10038)]
        [TestCase(3, 10038)]
        [TestCase(4, 10038)]
        [TestCase(1, 10039)]
        [TestCase(2, 10039)]
        [TestCase(3, 10039)]
        [TestCase(4, 10039)]
        public void TranslateMessage_Always_MainMessageIsMsgIdFromRawMsg(byte settingLevel, int msgId)
        {
            // Arrange
            _rawMessage = string.Format("<Root MsgID=\"{0}\" Msg=\"\"><Row SportType=\"1\" BetType=\"1\" /></Root>", msgId);

            // Act
            var actualMessageContent = _messageService.TranslateMessage(_msgType, _rawMessage, settingLevel, Arg.Any<string>());

            // Assert
            Assert.AreEqual(Message.ResourceManager.GetString($"Msg_{msgId}"), actualMessageContent.MainMessage);
        }

        [Test]
        public void TranslateMessage_SettingLevelIsProduct_SubMessageIsEmpty()
        {
            // Arrange
            byte settingLevel = 1;

            // Act
            var actualMessageContent = _messageService.TranslateMessage(_msgType, _rawMessage, settingLevel, "en-US");

            // Assert
            Assert.IsEmpty(actualMessageContent.SubMessages.First());
        }

        [Test]
        public void TranslateMessage_SettingLevelIsSportType_SubMessageIsSportTypeName()
        {
            // Arrange
            byte settingLevel = 2;

            _resourceService.GetSportTypeName(1, Arg.Any<string>()).Returns("Sport type 1");

            // Act
            MessageContent actualMessageContent = _messageService.TranslateMessage(_msgType, _rawMessage, settingLevel, "en-US");

            // Assert
            string expectedSubMsg = "Sport type 1";

            Assert.AreEqual(expectedSubMsg, actualMessageContent.SubMessages.First());
        }

        [Test]
        public void TranslateMessage_SettingLevelIsBetType_SubMessageIsBetTypeName()
        {
            // Arrange
            byte settingLevel = 3;

            _resourceService.GetBetTypeName(1, Arg.Any<string>()).Returns("Bet type 1");

            // Act
            MessageContent actualMessageContent = _messageService.TranslateMessage(_msgType, _rawMessage, settingLevel, "en-US");

            // Assert
            string expectedSubMsg = "Bet type 1";

            Assert.AreEqual(expectedSubMsg, actualMessageContent.SubMessages.First());
        }

        [Test]
        public void TranslateMessage_SettingLevelIsSportTypeBetType_SubMessageIsSportTypeNameHyphenBetTypeName()
        {
            // Arrange
            byte settingLevel = 4;

            _resourceService.GetSportTypeName(1, Arg.Any<string>()).Returns("Sport type 1");
            _resourceService.GetBetTypeName(1, Arg.Any<string>()).Returns("Bet type 1");

            // Act
            MessageContent actualMessageContent = _messageService.TranslateMessage(_msgType, _rawMessage, settingLevel, "en-US");

            // Assert
            string expectedSubMsg = "Sport type 1 - Bet type 1";

            Assert.AreEqual(expectedSubMsg, actualMessageContent.SubMessages.First());
        }
    }
}