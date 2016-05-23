namespace Fanex.CustomerSetting.SettingType6.Tests.Services
{
    using System.Linq;
    using Constants;
    using CustomerSetting.Services;
    using NSubstitute;
    using NUnit.Framework;
    using Resources;
    using SettingType6.Services;

    [TestFixture]
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
            _msgType = MessageType.WinLimitViolatedWithUpline;
            _rawMessage = "<Root MsgID=\"10024\" Msg=\"\">"
                            + "<Row SportType=\"1\" BetType=\"1\" />"
                        + "</Root>";
        }

        [TestCase(1, 10024)]
        [TestCase(2, 10024)]
        [TestCase(3, 10024)]
        [TestCase(4, 10024)]
        [TestCase(1, 10022)]
        [TestCase(2, 10022)]
        [TestCase(3, 10022)]
        [TestCase(4, 10022)]
        [TestCase(1, 10033)]
        [TestCase(2, 10033)]
        [TestCase(3, 10033)]
        [TestCase(4, 10033)]
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
            var actualMessageContent = _messageService.TranslateMessage(_msgType, _rawMessage, settingLevel, "en-US");

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
            var actualMessageContent = _messageService.TranslateMessage(_msgType, _rawMessage, settingLevel, "en-US");

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
            var actualMessageContent = _messageService.TranslateMessage(_msgType, _rawMessage, settingLevel, "en-US");

            // Assert
            string expectedSubMsg = "Sport type 1 - Bet type 1";
            Assert.AreEqual(expectedSubMsg, actualMessageContent.SubMessages.First());
        }
    }
}